using System;
using System.Collections;
using Events;
using Script.Main;
using Script.Main.Base;
using Script.Mgr;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Loading
{
    public class LoadingView : BaseLoadingUI
    {
        public LoadingAnimView LoadingAnimView;
        public GameObject MainLoadingUI;
        public GameObject tips_root;
        public Text progressText;
        public Text tips_txt;

        private int cfgIdx = 1;
        private int cfgLen = 0;

        protected override void StartNewScene()
        {
            base.StartNewScene();
            MainLoadingUI.SetActive(false);
            tips_root.SetActive(true);
            
            GameFlowTask gameFlowTask = FlowTaskFactory.CreatTask();
            LoadingAnimView.PlayEnter(() =>
            {
                gameFlowTask.Completed.Invoke();
                MainLoadingUI.SetActive(true);
            });
            GameFlowMgr.StartNewScene.AddTask(gameFlowTask);
            //
            EventCenter.ins.AddEventListener<KeyCode>("KeyDown",KeyDown);
            cfgIdx = UnityEngine.Random.Range(1, cfgLen);
        }

        protected override void UnLoadScene()
        {
            base.UnLoadScene();
        }

        protected override void LoadedScene()
        {
            base.LoadedScene();
            MainLoadingUI.SetActive(false);
            tips_root.SetActive(false);
            
            GameFlowTask gameFlowTask = FlowTaskFactory.CreatTask();
            LoadingAnimView.PlayOut(() =>
            {
                gameFlowTask.Completed.Invoke();
            });
            GameFlowMgr.LoadedScene.AddTask(gameFlowTask);
            EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
        }

        public override void DoCreat()
        {
            base.DoCreat();
            LoadingViewMgr.LoadingView = this;
        }

        protected override void DoUpdata()
        {
            base.DoUpdata();
            progressText.text = "" + (int)curProgressValue;
        }
        void KeyDown(KeyCode keyCode)
        {
            if(!IsActive())
                return;
            switch (keyCode)
            {
                case KeyCode.W:
                    break;
                case KeyCode.A:
                    break;
                case KeyCode.S:
                    break;
                case KeyCode.D:
                    break;
                case KeyCode.J:
                    cfgIdx++;
                    if (cfgIdx>=cfgLen)
                        cfgIdx = 1;
                    break;
                case KeyCode.K:
                    
                    break;
            }
            
        }
    }
}