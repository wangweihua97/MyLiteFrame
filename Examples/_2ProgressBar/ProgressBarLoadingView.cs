using System;
using System.Collections;
using Events;
using Script.Main;
using Script.Main.Base;
using Script.Mgr;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.ProgressBar
{
    public class ProgressBarLoadingView : BaseLoadingUI
    {
        public Text progressText;

        protected override void StartNewScene()
        {
            base.StartNewScene();
        }

        protected override void UnLoadScene()
        {
            base.UnLoadScene();
        }

        protected override void LoadedScene()
        {
            base.LoadedScene();
            
        }

        public override void DoCreat()
        {
            base.DoCreat();
            ProgressBarLoadingViewMgr.ProgressBarLoadingView = this;
        }

        protected override void DoUpdata()
        {
            base.DoUpdata();
            progressText.text = "" + (int)curProgressValue;
        }
    }
}