using Events;
using Script.Mgr;
using UI.LevelSelect;
using UI.Main;

namespace Examples.UI
{
    public class BUIMgr : UUIMgr
    {
        public static B1View B1View;
        public static B2View B2View;
        public static B3View B3View;
        public override string Name => "BUI1";
        public override string sortingLayerName => "CommonView";
        
        public override void DoCreat()
        {
            base.DoCreat();
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            Add<B1View>("B1View", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("B1View", "UIView" ));
            Add<B2View>("B2View", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("B2View", "UIView"));
            Add<B3View>("B3View", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("B3View", "UIView"));
            gameFlowTaskGroup.Attach(GameFlowMgr.LoadPrepareData80Per);
        }
        public override void DoDestroy()
        {
            base.DoDestroy();
        }
    }
}