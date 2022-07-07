using Events;
using Script.Mgr;

namespace Examples.UI
{
    public class UILoadingViewMgr : UUIMgr
    {
        
        public static UILoadingView UILoadingView;
        public override string Name => "LoadingViewMgr";
        public override string sortingLayerName => "LoadingView";
        
        
        public override void DoCreat()
        {
            base.DoCreat();
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            Add<UILoadingView>("UILoadingView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("UILoadingView", "UIView"));
            gameFlowTaskGroup.Attach(GameFlowMgr.LoadedInitData);
        }
        public override void DoDestroy()
        {
            base.DoDestroy();
        }
    }
}