using Events;
using Script.Mgr;
using UI.SettlementView;

namespace Script.Main
{
    /// <summary> 结算界面管理 </summary>
    public class SettlementUIMgr : UUIMgr
    {
        #region prop

        public static SettlementView SettlementView;
        public static ResultView ResultView;
        #endregion

        #region life
        public override string Name => "SettlementUIMgr";
        public override string sortingLayerName => "CommonView";
        public override void DoCreat()
        {
            base.DoCreat();
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            Add<SettlementView>("SettlementView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("SettlementView", "UIView"));
            Add<ResultView>("ResultView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("ResultView", "UIView"));
            gameFlowTaskGroup.Attach(GameFlowMgr.LoadPrepareData90Per);
        }
        #endregion

        #region Callback
        protected override void UnLoadedScene()
        {
            base.UnLoadedScene();
            DoDestroy();
        }
        #endregion
    }
}