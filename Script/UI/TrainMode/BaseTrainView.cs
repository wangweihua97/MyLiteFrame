using System.Collections;
using Events;
using OtherMgr;
using Player;
using Script.Main;
using Script.Mgr;
using UI.Base;
using UI.TrainMode;
using UnityEngine;

namespace UI.TrainMode
{
    public class BaseTrainView : UView
    {
        enum ViewState
        {
            Common,
            OpenTipsComponent,
        }
        public override bool DefaultShow => false;

        [Component]
        [Header("动作Group")]
        public ActGroupCpmponent ActGroupCpmponent;
        
        [Component]
        [Header("详情弹出框")]
        public ActTipsCpmponent ActTipsCpmponent;
        

        [Header("站姿")]
        [SerializeField] private PostureItem PostureItem;

        private bool isLeft = true;
        private ViewState _viewState = ViewState.Common;
        
        public override void DoCreat()
        {
            base.DoCreat();
            TrainViewMgr.BaseTrainView = this;
            EventCenter.ins.AddEventListener<KeyCode>("KeyDown",KeyDown);
        }

        public override void DoOpen()
        {
            base.DoOpen();
            AudioManager.PlayAudioEffectA("打开弹窗");
            _viewState = ViewState.Common;
            RefreshView();
            GraduallyShow(0.3f);
            EventCenter.ins.EventTrigger("CameraMove" ,ExcelMgr.TDCameraLocation.Get("BaseTrainView").location);
            PlayerMgr.instance.playerAnimationMgr.ChangeToFIdle(isLeft);
        }

        public void OpenTips()
        {
            AudioManager.PlayAudioEffectA("选中确认");
            _viewState = ViewState.OpenTipsComponent;
            RefreshView();
            
            ActTipsCpmponent.SetData(ActGroupCpmponent.GetSelectData());
            updateTipsXY();
        }

        void updateTipsXY()
        {
            ActTipsCpmponent.SetXY(ActGroupCpmponent.GetItem());
        }

        public void ChangePosture()
        {
            if (PlayerMgr.instance.playerAnimationMgr.BodyIsLeft)
            {
                isLeft = false;
            }
            else
            {
                isLeft = true;
            }
            PlayerMgr.instance.playerAnimationMgr.ChangeBodyPosture(isLeft);
            RefreshPosture();
        }

        void RefreshView()
        {
            RefreshPosture();
            ActTipsCpmponent.SetVisible(_viewState == ViewState.OpenTipsComponent);
        }

        void CloseTipComponent()
        {
            if (_viewState == ViewState.OpenTipsComponent)
            {
                _viewState = ViewState.Common;
                RefreshView();
            }
        }

        void RefreshPosture()
        {
            ActGroupCpmponent.SetPosture(isLeft);
            PostureItem.SetBodyPosture(isLeft);
        }

        void UpdateTips()
        {
            if (_viewState == ViewState.OpenTipsComponent)
            {
                ActTipsCpmponent.SetData(ActGroupCpmponent.GetSelectData());
                updateTipsXY();
            }
        }

        void KeyDown(KeyCode keyCode)
        {
            if(!IsTop() || !IsActive())
                return;
            switch (keyCode)
            {
                case KeyCode.W:
                    // CloseTipComponent();
                    ActGroupCpmponent.MoveUp();
                    UpdateTips();
                    break;
                case KeyCode.A:
                    // CloseTipComponent();
                    ActGroupCpmponent.MoveLeft();
                    UpdateTips();
                    break;
                case KeyCode.S:
                    // CloseTipComponent();
                    ActGroupCpmponent.MoveDown();
                    UpdateTips();
                    break;
                case KeyCode.D:
                    // CloseTipComponent();
                    ActGroupCpmponent.MoveRight();
                    UpdateTips();
                    break;
                case KeyCode.J:
                    if (_viewState == ViewState.Common)
                        OpenTips();
                    break;
                case KeyCode.H:
                    // CloseTipComponent();
                    ChangePosture();
                    break;
                case KeyCode.K:
                    if (_viewState == ViewState.OpenTipsComponent)
                    {
                        CloseTipComponent();
                    }
                    else
                    {
                        StartCoroutine(OpenTrainSelectView());
                    }
                    break;
            }
            
        }
        
        IEnumerator OpenTrainSelectView()
        {
            yield return 0;
            DoClose();
            TrainViewMgr.TrainSelectView.DoOpen();
        }

        public override void DoClose()
        {
            base.DoClose();
            PlayerMgr.instance.playerAnimationMgr.ChangeToIdle();
        }

        public override void DoDestory()
        {
            base.DoDestory();
            EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
        }
    }
}