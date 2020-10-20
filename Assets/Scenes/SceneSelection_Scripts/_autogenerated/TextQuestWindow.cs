/////////////////////////////////////////
//     THIS IS AUTOGENERATED CODE      //
//       do not change directly        //
/////////////////////////////////////////
using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using CometUI;

namespace SceneSelection_UI
{
    partial class TextQuestWindow : BaseView //Autogenerated
    {
        /// <summary>Static instance of the view</summary>
        public static TextQuestWindow Instance { get; private set; }
        // Controls
        #pragma warning disable 0414
        //[Header("Controls (auto capture)")]
        [Header("Custom")]
        [AutoGenerated, SerializeField, HideInInspector] TMPro.TextMeshProUGUI txName = default;
        [AutoGenerated, SerializeField, HideInInspector] TMPro.TextMeshProUGUI txDesc = default;
        [AutoGenerated, SerializeField, HideInInspector] UnityEngine.UI.Image imImage = default;
        [AutoGenerated, SerializeField, HideInInspector] HexMapScene_UI.TextQuestVariant pnTextQuestVariant = default;
        [AutoGenerated, SerializeField, HideInInspector] OutlineButton btClose = default;
        #pragma warning restore 0414
        
        public override void AutoSubscribe()
        {
            SubscribeOnChanged(txName);
            SubscribeOnChanged(txDesc);
            SubscribeOnChanged(imImage);
            SubscribeOnChanged(pnTextQuestVariant);
            SubscribeOnChanged(btClose);
        }
        ///<summary>Data</summary>
        public Model.Quest quest{ get; private set; }
        
        [VisibleInGraph(false)]
        public void Build(Model.Quest quest)
        {
            this.quest = quest;
            OnBuildSafe(true);
        }
        
        public override BaseView Clone()
        {
            var clone = (TextQuestWindow)base.Clone();
            clone.quest = quest;
            return clone;
        }
    }
}