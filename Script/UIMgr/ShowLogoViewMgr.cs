using Events;
using Script.Mgr;
using UI.LevelSelect;
using UI.Main;

namespace Script.Main
{
    public class ShowLogoViewMgr : UUIMgr
    {
        public static LoginView LoginView;
        // public static MainView MainView;
        // public static LevelSelectUIView LevelSelectUIView;
        public override string Name => "ShowLogoViewMgr";
        public override string sortingLayerName => "CommonView";
        
        public override void DoCreat()
        {
            base.DoCreat();
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            Add<LoginView>("LoginView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("LoginView", "UIView" ));
            gameFlowTaskGroup.Attach(GameFlowMgr.LoadPrepareData80Per);
        }
        public override void DoDestroy()
        {
            base.DoDestroy();
        }
    }
}