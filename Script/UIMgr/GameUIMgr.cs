using Events;
using OtherMgr;
using Script.Mgr;
using UI;
using UI.LevelSelect;
using UI.Main;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Script.Main
{
    public class GameUIMgr : UUIMgr
    {
        #region prop

        // public static MainMenuView MainMenuView { get; private set; }
        public static GameMainView GameMainView;
        public static StopGameView StopGameView;

        public static StartView StartView;
        
        //当前玩家选择的按钮
        public int curIndex;
        //按钮数量为2，第一个为”继续游戏“，第二个为”结束游戏“
        public int maxIndex = 2;
        //public static NationSelectionView NationSelectionView { get; private set; }

        #endregion

        #region life

        public override string Name => "GameViewMgr";
        public override string sortingLayerName => "CommonView";

        public override void DoCreat()
        {
            base.DoCreat();
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            Add<StopGameView>("StopGameView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("StopGameView", "UIView"));
            Add<GameMainView>("GameMainView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("GameMainView", "UIView"));
            Add<StartView>("StartView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("StartView", "UIView"));
            gameFlowTaskGroup.Attach(GameFlowMgr.LoadPrepareData80Per);
            curIndex = 0;
            //NationSelectionView = CreateView<NationSelectionView>("NationSelectionView");
        }

        protected override void DoUpdata()
        {
            base.DoUpdata();
        }

        #endregion

        #region 公共方法

        public void CreatView()
        {
            DoCreat();
        }

        public void DistoryView()
        {
            DoDestroy();
        }

        public GameMainView GetGameMainView()
        {
            return GameMainView;
        }
        
        public void StopGameUI()
        {
            AudioManager.PlayAudioEffectA("打开弹窗");
            StopGameView.Show(true);
        }
        
        public void BackGame()
        {
            StopGameView.Show(false);
        }

        public void SelectIconMove()
        {
            StopGameView.MoveTo(curIndex);
        }
        

        #endregion
    }

}