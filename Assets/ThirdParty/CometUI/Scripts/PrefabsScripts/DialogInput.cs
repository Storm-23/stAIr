using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using CometUI;

namespace CometUI
{
    public partial class DialogInput : BaseView
    {
#pragma warning disable 0414 //never used
        [Header("Controls (auto capture)")]
        [AutoGenerated, SerializeField] UnityEngine.UI.Text txMessage = default;
        [AutoGenerated, SerializeField] UnityEngine.UI.Button btOk = default;
        [AutoGenerated, SerializeField] UnityEngine.UI.Button btCancel = default;
        [AutoGenerated, SerializeField] UnityEngine.UI.InputField ifText = default;
        [AutoGenerated, SerializeField] UnityEngine.UI.Text txPlaceholder = default;
#pragma warning restore 0414

        bool created = false;
        public string Result { get; private set; }

        void Start()
        {
            created = true;
        }

        public override void AutoSubscribe()
        {
            SubscribeOnChanged(btOk);
            SubscribeOnChanged(btCancel);
            Subscribe(btOk, () => { Result = ifText.text; Close(); });
            Subscribe(btCancel, () => { Result = null; Close(); });
        }

        public void Build(string message, string text, string placeHolderText, string okText = "OK", string cancelText = "Cancel")
        {
            Set(txMessage, message);
            Set(btOk, okText);
            Set(btCancel, cancelText);
            Set(ifText, text);
            Set(txPlaceholder, placeHolderText);

            SetActive(btOk, okText != null);
            SetActive(btCancel, cancelText != null);

            Result = null;

            OnBuildSafe(true);
        }

        protected override void OnShown()
        {
            base.OnShown();

            ifText.Select();
            ifText.ActivateInputField();
        }

        protected override void OnDisable()
        {
            if (created)
                Destroy(gameObject);
            base.OnDisable();
        }
    }
}

