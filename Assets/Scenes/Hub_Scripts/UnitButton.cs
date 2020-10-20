using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using CometUI;
using Model;

namespace Hub_UI
{
    partial class UnitButton : BaseView
    {
        public event Action<Unit> Clicked;

        private void Start()
        {
            //subscribe buttons or events here
            Subscribe(bt, OnClick);
        }

        private void OnClick()
        {
            Clicked?.Invoke(unit);
        }

        protected override void OnBuild(bool isFirstBuild)
        {
            //Data: Unit unit
            //copy data to UI controls here
            Set(txName, unit.Name);
            Set(bt, GameSettings.Instance.GetFace(unit.IconIndex));
            SetInteractable(bt, isActive);

            //GetComponent<Tooltip>().TextLeft = unit.Name;
        }
    }
}