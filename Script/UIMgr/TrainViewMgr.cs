using Events;
using Script.Mgr;
using UI.TrainMode;
using UI.LevelSelect;
using UI.Main;

namespace Script.Main
{
    public class TrainViewMgr : UUIMgr
    {
        public static TrainSelectView TrainSelectView;
        
        public static BaseTrainView BaseTrainView;
        
        public static SelectMusicView SelectMusicView;

        public static SelectSceneView SelectSceneView;
        
        public override string Name => "TrainViewMgr";
        public override string sortingLayerName => "CommonView";
        
        public override void DoCreat()
        {
            base.DoCreat();
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            Add<TrainSelectView>("TrainSelectView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("TrainSelectView", "UIView" ));
            Add<BaseTrainView>("BaseTrainView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("BaseTrainView", "UIView" ));
            Add<SelectMusicView>("SelectMusicView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("SelectMusicView", "UIView" ));
            Add<SelectSceneView>("SelectSceneView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("SelectSceneView", "UIView" ));
            gameFlowTaskGroup.Attach(GameFlowMgr.LoadPrepareData80Per);
        }
        public override void DoDestroy()
        {
            base.DoDestroy();
        }
    }
}