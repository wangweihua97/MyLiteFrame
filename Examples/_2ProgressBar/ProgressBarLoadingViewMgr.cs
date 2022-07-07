using Events;
using Script.Mgr;

namespace Examples.ProgressBar
{
    public class ProgressBarLoadingViewMgr : UUIMgr
    {
        
        public static ProgressBarLoadingView ProgressBarLoadingView;
        public override string Name => "LoadingViewMgr";
        public override string sortingLayerName => "LoadingView";
        
        
        public override void DoCreat()
        {
            base.DoCreat();
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            Add<ProgressBarLoadingView>("ProgressBarLoadingView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("ProgressBarLoadingView", "UIView"));
            gameFlowTaskGroup.Attach(GameFlowMgr.LoadedInitData);
        }
        public override void DoDestroy()
        {
            base.DoDestroy();
        }
    }
}