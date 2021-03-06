﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using XNode;
using XNodeEditor;

namespace CometUI
{
    /// <summary>Node for View</summary>
    [NodeTint("#336666")]
    public class ViewNode : BaseNode
    {
        [HideInInspector]
        public RectTransform rt;

        public void Build(BaseView v)
        {
            name = v.GetType().Name;
            this.rt = v.RectTransform;
            GrabInfoAboutView(v.transform as RectTransform);

            AddDynamic(typeof(BindInputPort), "This");
        }

        public void Build(RectTransform rt)
        {
            name = rt.name;
            this.rt = rt;
            GrabInfoAboutView(rt);

            AddDynamic(typeof(BindInputPort), "This");
        }

        public static event Action<ViewNode> DoubleClicked;
        public static event Action<ViewNode> RenameRequest;

        public override void OnDoubleClick()
        {
            DoubleClicked?.Invoke(this);
        }

        public override IEnumerable<(Type, string)> GetAllowedAddPorts()
        {
            if (ViewInfo == null)
                GrabInfoAboutView(rt);
            if (ViewInfo == null)
                yield break;

            foreach (var item in ViewInfo.Members.Values)
            {
                if (item.Type == "void")
                    yield return (typeof(ActionInputPort), item.Name);
            }

            foreach (var item in ViewInfo.Members.Values)
            {
                if (item.Type.IndexOf("Button") >= 0)
                    yield return (typeof(ActionOutputPort), item.Name);
            }

            foreach (var item in ViewInfo.Members.Values)
            {
                if (item.Type.StartsWith("System.Action"))
                    yield return (typeof(EventPort), item.Name);
            }

            yield return (typeof(BindOutputPort), "Bind");
            yield return (typeof(BindInputPort), "This");

            foreach (Gesture gest in Enum.GetValues(typeof(Gesture)))
                if (gest != Gesture.None)
                    yield return (typeof(GesturePort), gest.ToString());
        }

        public static event Action<ViewNode> OnCreateUserScript;
        public static event Action<ViewNode> OnShowAutoScript;

        [ContextMenu("Show or Create User Script", false, 1)]
        void CreateUserViewFile()
        {
            if (rt == null)
            {
                UnityEditor.EditorUtility.DisplayDialog("No GameObject", "GameObject for the node is not found!", "OK");
                return;
            }

            OnCreateUserScript(this);
        }

        [ContextMenu("Show Autogenerated Script", false, 1)]
        void ShowAutogeneratedScript()
        {
            OnShowAutoScript?.Invoke(this);
        }

        [ContextMenu("Rename")]
        void RenameView()
        {
            if (rt == null)
            {
                UnityEditor.EditorUtility.DisplayDialog("No GameObject", "GameObject for the node is not found!", "OK");
                return;
            }

            RenameRequest?.Invoke(this);
        }

        [ContextMenu("Remove Node and Scripts", false, 1)]
        public void DeleteNodeAndScripts()
        {
            if (EditorUtility.DisplayDialog("Node and Scripts removing",
                $"Do you want to delete Node and ALL Scripts '{name}.cs'?",
                "Delete", "Cancel"))
            {
                if (rt != null)
                    DestroyImmediate(rt.GetComponent<BaseView>());

                var files = Directory.GetFiles(Application.dataPath, $"{name}.cs", SearchOption.AllDirectories);
                foreach (var file in files)
                    File.Delete(file);

                NodeEditorWindow.current.SelectNode(this, false);
                NodeEditorWindow.current.RemoveSelectedNodes();

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        public ViewInfo ViewInfo;

        public void GrabInfoAboutView(RectTransform rt)
        {
            if (rt == null)
                return;

            ViewInfo = new ViewInfo();
            ViewInfo.Members.Clear();

            void GrabMethods(Type type, bool checkAttr)
            {
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var mi in methods)
                {
                    if (mi.Name.StartsWith("get_") ||
                        mi.Name.StartsWith("<"))
                        continue;//property getter

                    var attr = mi.GetCustomAttribute<VisibleInGraphAttribute>();
                    if (attr == null && checkAttr)
                        continue;
                    if (attr != null && !attr.Visible)
                        continue;

                    var parameters = mi.GetParameters();
                    var count = parameters.Count(p => !p.HasDefaultValue);
                    if (count != 0)
                    {
                        if (attr == null || !attr.Visible)
                            continue;
                    }

                    ViewInfo.Members[mi.Name] = new ViewInfoItem { Name = mi.Name, MethodInfo = mi };
                }
            }

            void GrabEvents(Type type, bool checkAttr)
            {
                var events = type.GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var ei in events)
                {
                    var attr = ei.GetCustomAttribute<VisibleInGraphAttribute>();
                    if (attr == null && checkAttr)
                        continue;
                    if (attr != null && !attr.Visible)
                        continue;

                    ViewInfo.Members[ei.Name] = new ViewInfoItem { Name = ei.Name, EventInfo = ei };
                }
            }

            //grab components from scene
            foreach (var info in SceneInfoGrabber<BaseView>.GrabInfo(rt, true))
                ViewInfo.Members[info.Key] = new ViewInfoItem { Name = info.Key, Component = info.Value };

            //grab exists fileds
            Type viewType = null;
            var view = rt.GetComponent<BaseView>();
            if (view != null)
                viewType = view.GetType();
            else
                viewType = UIGraphEditor.GetTypeByName(rt.name, rt.gameObject.scene);

            if (viewType != null)
            {
                var fields = viewType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var fi in fields)
                {
                    if (SceneInfoGrabber<BaseView>.WellKnownTypes.Contains(fi.FieldType) || typeof(BaseView).IsAssignableFrom(fi.FieldType))
                    {
                        var isAutogenerated = fi.GetCustomAttribute<AutoGeneratedAttribute>();
                        if (isAutogenerated != null)
                        {
                            //skip isAutogenerated fields => they will recretaed
                            continue;
                        }

                        ViewInfo.Members[fi.Name] = new ViewInfoItem { Name = fi.Name, FieldInfo = fi};
                    }
                }

                //grab argless public methods
                GrabMethods(viewType, false);

                //grab public events
                GrabEvents(viewType, false);
            }

            //grab argless public methods of base class
            GrabMethods(typeof(BaseView), true);

            //add linked Views
            foreach (var port in Outputs)
            {
                foreach (var linked in port.GetConnections())
                {
                    if (linked.node is ViewNode otherNode)
                    {
                        if (!ViewInfo.Members.ContainsKey(otherNode.name))
                            ViewInfo.Members[otherNode.name] = new ViewInfoItem() { Binded = otherNode.name, Name = otherNode.name, IsAutogenerated = true, IsAutogeneratedReallyExists = true };
                    }
                }
            }

            //remove Ports w/o members
            foreach (var port in Ports.ToArray())
                if (port.ValueType == typeof(ActionInputPort) || port.ValueType == typeof(ActionOutputPort) || port.ValueType == typeof(EventPort))
                {
                    if (!ViewInfo.Members.ContainsKey(port.fieldName))
                    {
                        //remove port
                        RemoveDynamicPort(port);
                    }
                }
        }
    }

    public class ViewInfo
    {
        public Dictionary<string, ViewInfoItem> Members = new Dictionary<string, ViewInfoItem>();
    }

    public class ViewInfoItem
    {
        public string Name;
        public Component Component;
        public FieldInfo FieldInfo;
        public MethodInfo MethodInfo;
        public EventInfo EventInfo;
        public string Binded;
        public bool IsAutogenerated;
        public bool IsAutogeneratedReallyExists;

        public string Type => 
            Component != null ? Component.GetType().FullName 
            : (FieldInfo != null ? FieldInfo.FieldType.FullName
            : (EventInfo != null ? EventInfo.EventHandlerType.FullName
            : (Binded != null ? Binded : "void")));
    }
}