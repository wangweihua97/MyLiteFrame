using Events;
using Script.Mgr;
using UI.Common;
using UI.TrainMode;

namespace Script.Main
{
    public class CommonUIMgr : UUIMgr
    {
        public static PopupFrame PopupFrame;
        public override string Name => "CommonUIMgr";
        public override string sortingLayerName => "PopupView";
        
        public override bool IsMask => false;
        
        public override void DoCreat()
        {
            base.DoCreat();
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            Add<PopupFrame>("PopupFrame", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("PopupFrame", "UIView" ));
            gameFlowTaskGroup.Attach(GameFlowMgr.LoadPrepareData80Per);
        }
        public override void DoDestroy()
        {
            base.DoDestroy();
        }
    }
}