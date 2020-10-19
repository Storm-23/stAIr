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
    partial class UnitInfoPanel : BaseView //Autogenerated
    {
        /// <summary>Static instance of the view</summary>
        public static UnitInfoPanel Instance { get; private set; }
        // Controls
        #pragma warning disable 0414
        //[Header("Controls (auto capture)")]
        [Header("Custom")]
        [AutoGenerated, SerializeField, HideInInspector] Hub_UI.UnitInfo pnUnitInfo = default;
        #pragma warning restore 0414
        
        public override void AutoSubscribe()
        {
            SubscribeOnChanged(pnUnitInfo);
            Subscribe(Gesture.Tap, () => this.Close());
        }
        ///<summary>Data</summary>
        public Model.Unit unit{ get; private set; }
        
        [VisibleInGraph(false)]
        public void Build(Model.Unit unit)
        {
            this.unit = unit;
            OnBuildSafe(true);
        }
        
        public override BaseView Clone()
        {
            var clone = (UnitInfoPanel)base.Clone();
            clone.unit = unit;
            return clone;
        }
    }
}