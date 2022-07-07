using Events;
using Script.Mgr;
using UI.Character;
using UI.TrainMode;

namespace Script.Main
{
    public class CharacterUIMgr : UUIMgr
    {
        // public static SelectSexView SelectSexView;
        public static DressUpView DressUpView;
        public override string Name => "CharacterUIMgr";
        public override string sortingLayerName => "CommonView";
        
        public override void DoCreat()
        {
            base.DoCreat();
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            Add<DressUpView>("DressUpView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("DressUpView", "UIView" ));
            // Add<SelectSexView>("SelectSexView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            // gameFlowTaskGroup.Add(GetHandle("SelectSexView", "UIView" ));
            gameFlowTaskGroup.Attach(GameFlowMgr.LoadPrepareData80Per);
        }
        public override void DoDestroy()
        {
            base.DoDestroy();
        }
    }
}