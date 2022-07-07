using Events;
using Script.Mgr;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Base
{
    public class BaseLoadingUI : UView
    {
        public override bool DefaultShow => false;
        public float curProgressValue = 0;
        public Slider slider;
        private float _loadProgress = 0;

        protected int CurStep;
        protected override void DoEnable()
        {
            base.DoEnable();
            CurStep = -1;
            SetProgress(0);
        }

        protected override void DoDisable()
        {
            base.DoDisable();
        }

        protected override void DoUpdata()
        {
            base.DoUpdata();
            if (CurStep <= 0)
            {
                SetProgress(0);
                return;
            }
            switch (CurStep)
            {
                case 1:
                    GetGameFlowProgress(GameFlowMgr.UnLoadScene);
                    break;
                case 2:
                    GetGameFlowProgress(GameFlowMgr.UnLoadData);
                    break;
                case 3:
                    GetGameFlowProgress(GameFlowMgr.UnLoadedScene);
                    break;
                case 4:
                    GetGameFlowProgress(GameFlowMgr.LoadScene);
                    break;
                case 5:
                    GetGameFlowProgress(GameFlowMgr.LoadPrepareData70Per);
                    break;
                case 6:
                    GetGameFlowProgress(GameFlowMgr.LoadPrepareData80Per);
                    break;
                case 7:
                    GetGameFlowProgress(GameFlowMgr.LoadPrepareData90Per);
                    break;
                case 8:
                    SetProgress(100);
                    break;
                case 9:
                    SetProgress(100);
                    break;
            }
        }

        private void GetGameFlowProgress(GameFlowEvent gameFlowEvent)
        {
            float progress = _loadProgress + gameFlowEvent.Percent * gameFlowEvent.GetLoadingWeight();
            SetProgress(progress);
        }

        private void RefeshLoadingProgress(GameFlowEvent gameFlowEvent)
        {
            _loadProgress += gameFlowEvent.GetLoadingWeight();
        }
        
        //准备开始新的场景，播放loding开始动画
        protected override void StartNewScene()
        {
            base.StartNewScene();
            CurStep = 0;
            _loadProgress = 0;
        }
        //准备卸载场景
        protected override void UnLoadScene()
        {
            base.UnLoadScene();
            CurStep = 1;
            RefeshLoadingProgress(GameFlowMgr.StartNewScene);
        }
        //准备卸载场景数据
        protected override void UnLoadData()
        {
            base.UnLoadData();
            CurStep = 2;
            RefeshLoadingProgress(GameFlowMgr.UnLoadScene);
        }
        //卸载完场景
        protected override void UnLoadedScene()
        {
            base.UnLoadedScene();
            CurStep = 3;
            RefeshLoadingProgress(GameFlowMgr.UnLoadData);
        }
        //准备加载场景
        protected override void LoadScene()
        {
            base.LoadScene();
            CurStep = 4;
            RefeshLoadingProgress(GameFlowMgr.UnLoadedScene);
        }
        //准备加载场景数据
        protected override void LoadPrepareData70Per()
        {
            base.LoadPrepareData70Per();
            CurStep = 5;
            RefeshLoadingProgress(GameFlowMgr.LoadScene);
        }
        //准备加载场景数据
        protected override void LoadPrepareData80Per()
        {
            base.LoadPrepareData80Per();
            CurStep = 6;
            RefeshLoadingProgress(GameFlowMgr.LoadPrepareData70Per);
        }
        //准备加载场景数据
        protected override void LoadPrepareData90Per()
        {
            base.LoadPrepareData90Per();
            CurStep = 7;
            RefeshLoadingProgress(GameFlowMgr.LoadPrepareData80Per);
        }
        //加载完场景，播放Loding完成动画
        protected override void LoadedScene()
        {
            base.LoadedScene();
            CurStep = 8;
            RefeshLoadingProgress(GameFlowMgr.LoadPrepareData90Per);
        }
        //准备开始游戏
        protected override void EnterNewScene()
        {
            base.EnterNewScene();
            CurStep = 9;
            base.Show(false);
            RefeshLoadingProgress(GameFlowMgr.LoadedScene);
        }

        public void SetProgress(float progress)
        {
            if (progress >= 100)
                progress = 100;
            curProgressValue = progress;
            if(slider != null)
                slider.value = progress;
        }
    }
}