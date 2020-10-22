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
    partial class BuyItemPanel : BaseView //Autogenerated
    {
        /// <summary>Static instance of the view</summary>
        public static BuyItemPanel Instance { get; private set; }
        // Controls
        #pragma warning disable 0414
        //[Header("Controls (auto capture)")]
        [Header("Custom")]
        [AutoGenerated, SerializeField, HideInInspector] OutlineButton btBuy = default;
        [AutoGenerated, SerializeField, HideInInspector] ItemInfo ItemInfo = default;
        #pragma warning restore 0414
        
        public override void AutoSubscribe()
        {
            SubscribeOnChanged(btBuy);
            SubscribeOnChanged(ItemInfo);
        }
        ///<summary>Data</summary>
        public Model.IItem item{ get; private set; }
        
        [VisibleInGraph(false)]
        public void Build(Model.IItem item)
        {
            this.item = item;
            OnBuildSafe(true);
        }
        
        public override BaseView Clone()
        {
            var clone = (BuyItemPanel)base.Clone();
            clone.item = item;
            return clone;
        }
    }
}