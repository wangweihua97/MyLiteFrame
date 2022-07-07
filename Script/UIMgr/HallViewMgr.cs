using Events;
using Script.Mgr;
using UI.LevelSelect;
using UI.Main;

namespace Script.Main
{
    public class HallViewMgr : UUIMgr
    {
        public static LoginView LoginView;
        public static MainView MainView;
        // public static LevelSelectUIView LevelSelectUIView;
        public static LevelSelectUIViewV1 LevelSelectUIViewV1;
        public override string Name => "HallViewMgr";
        public override string sortingLayerName => "CommonView";
        
        public override void DoCreat()
        {
            base.DoCreat();
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            Add<MainView>("MainView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("MainView", "UIView" ));
            // Add<LevelSelectUIView>("LevelSelectUIView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            // gameFlowTaskGroup.Add(GetHandle("LevelSelectUIView", "UIView"));
            Add<LevelSelectUIViewV1>("LevelSelectUIViewV1", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("LevelSelectUIViewV1", "UIView"));
            gameFlowTaskGroup.Attach(GameFlowMgr.LoadPrepareData80Per);
        }
        public override void DoDestroy()
        {
            base.DoDestroy();
        }
    }
}