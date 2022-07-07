using Events;
using Script.Mgr;
using UI.Loading;

namespace Script.Main
{
    public class LoadingViewMgr : UUIMgr
    {
        
        public static LoadingView LoadingView;
        public override string Name => "LoadingViewMgr";
        public override string sortingLayerName => "LoadingView";
        
        
        public override void DoCreat()
        {
            base.DoCreat();
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            Add<LoadingView>("LoadingView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("LoadingView", "UIView"));
            gameFlowTaskGroup.Attach(GameFlowMgr.LoadPrepareData90Per);
        }
        public override void DoDestroy()
        {
            base.DoDestroy();
        }
    }
}