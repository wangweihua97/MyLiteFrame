using Events;
using Script.Mgr;
using UI.LevelSelect;
using UI.Main;

namespace Examples.UI
{
    public class AUIMgr : UUIMgr
    {
        public static A1View A1View;
        public static A2View A2View;
        public static A3View A3View;
        public override string Name => "AUI1";
        public override string sortingLayerName => "CommonView";
        
        public override void DoCreat()
        {
            base.DoCreat();
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            Add<A1View>("A1View", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("A1View", "UIView" ));
            Add<A2View>("A2View", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("A2View", "UIView"));
            Add<A3View>("A3View", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("A3View", "UIView"));
            gameFlowTaskGroup.Attach(GameFlowMgr.LoadPrepareData80Per);
        }
        public override void DoDestroy()
        {
            base.DoDestroy();
        }
    }
}