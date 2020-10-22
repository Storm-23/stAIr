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
    partial class WeaponButton : BaseView //Autogenerated
    {
        /// <summary>Static instance of the view</summary>
        public static WeaponButton Instance { get; private set; }
        // Controls
        #pragma warning disable 0414
        //[Header("Controls (auto capture)")]
        [Header("Custom")]
        [AutoGenerated, SerializeField, HideInInspector] OutlineButton bt = default;
        [AutoGenerated, SerializeField, HideInInspector] TMPro.TextMeshProUGUI txPrice = default;
        #pragma warning restore 0414
        
        public override void AutoSubscribe()
        {
            SubscribeOnChanged(bt);
            SubscribeOnChanged(txPrice);
        }
        ///<summary>Data</summary>
        public Model.IItem item{ get; private set; }
        public bool isActive{ get; private set; }
        
        [VisibleInGraph(false)]
        public void Build(Model.IItem item, bool isActive)
        {
            this.item = item;
            this.isActive = isActive;
            OnBuildSafe(true);
        }
        
        public override BaseView Clone()
        {
            var clone = (WeaponButton)base.Clone();
            clone.item = item;
            clone.isActive = isActive;
            return clone;
        }
    }
}