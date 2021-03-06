/////////////////////////////////////////
//     THIS IS AUTOGENERATED CODE      //
//       do not change directly        //
/////////////////////////////////////////
using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using CometUI;

namespace Hub_UI
{
    partial class StatLine : BaseView //Autogenerated
    {
        /// <summary>Static instance of the view</summary>
        public static StatLine Instance { get; private set; }
        // Controls
        #pragma warning disable 0414
        //[Header("Controls (auto capture)")]
        [Header("Custom")]
        [AutoGenerated, SerializeField, HideInInspector] TMPro.TextMeshProUGUI txName = default;
        [AutoGenerated, SerializeField, HideInInspector] TMPro.TextMeshProUGUI txValue = default;
        #pragma warning restore 0414
        
        public override void AutoSubscribe()
        {
            SubscribeOnChanged(txName);
            SubscribeOnChanged(txValue);
        }
        ///<summary>Data</summary>
        public string text{ get; private set; }
        public string value{ get; private set; }
        public string tooltip{ get; private set; }
        
        [VisibleInGraph(false)]
        public void Build(string text, string value, string tooltip)
        {
            this.text = text;
            this.value = value;
            this.tooltip = tooltip;
            OnBuildSafe(true);
        }
        
        public override BaseView Clone()
        {
            var clone = (StatLine)base.Clone();
            clone.text = text;
            clone.value = value;
            clone.tooltip = tooltip;
            return clone;
        }
    }
}