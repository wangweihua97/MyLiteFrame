using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using OtherMgr;
using Script.Excel.Table;
using Script.Main;
using Script.Mgr;
using UI.Base;
using UI.TrainMode;
using UIModel.TrainMode;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace UI.TrainMode
{
    public class TrainSelectView : UView
    {
        [Header("选择训练管卡的组件")]
        public TrainModeSelectComponent TrainModeSelectComponent;
        [Component]
        [Header("难易度的组件")]
        public GradeComponent GradeComponent;
        [Header("细节的组件")]
        public DetailsComponent DetailsComponent;
        public override bool DefaultShow => false;

        public TDTraining CurData;

        public TrainScrollItemData TrainScrollItemData
        {
            get
            {
                return TrainModeSelectComponent.GetSelectedData();
            }
        } 
        
        public override void DoCreat()
        {
            base.DoCreat();
            TrainViewMgr.TrainSelectView = this;
            TrainModeSelectComponent.InitData();
            EventCenter.ins.AddEventListener<KeyCode>("KeyDown",KeyDown);
        }

        public void SetData(TDTraining data)
        {
            CurData = data;
            GradeComponent.SetDegree(data.difficulty);
            DetailsComponent.SetData(data ,TrainScrollItemData);
        }
        
        void KeyDown(KeyCode keyCode)
        {
            if(!IsTop() || !IsActive())
                return;
            switch (keyCode)
            {
                case KeyCode.W:
                    TrainModeSelectComponent.MoveUp();
                    break;
                case KeyCode.A:
                    TrainModeSelectComponent.MoveLeft();
                    break;
                case KeyCode.S:
                    TrainModeSelectComponent.MoveDown();
                    break;
                case KeyCode.D:
                    TrainModeSelectComponent.MoveRight();
                    break;
                case KeyCode.J:
                    var data =TrainModeSelectComponent.GetSelectedData();
                    if(data.IsLocked)
                        CommonUIMgr.PopupFrame.ShowTips("  关卡未解锁，完成上一关解锁  ");
                    else
                    {
                        TrainViewMgr.SelectMusicView.ActName = data.Describe;
                        StartCoroutine(OpenSelectMusicView());
                    }
                    break;
                case KeyCode.H:
                    StartCoroutine(OpenBaseTrainView());
                    break;
                case KeyCode.K:
                    DoClose();
                    HallViewMgr.MainView.CameraMoveBTypeOnce = true;
                    HallViewMgr.MainView.DoOpen();
                    break;
            }
            
        }
        
        IEnumerator OpenBaseTrainView()
        {
            yield return 0;
            DoClose();
            TrainViewMgr.BaseTrainView.DoOpen();
        }
        
        IEnumerator OpenSelectMusicView()
        {
            yield return 0;
            DoClose();
            TrainViewMgr.SelectMusicView.DoOpen();
        }

        public override void DoOpen()
        {
            base.DoOpen();
            GraduallyShow(0.3f);
            AudioManager.PlayAudioEffectA("打开弹窗");
            EventCenter.ins.EventTrigger("CameraMove" ,ExcelMgr.TDCameraLocation.Get("TrainSelectView").location);
        }
        
        public override void DoClose()
        {
            base.DoClose();
            AudioManager.PlayAudioEffectA("返回");
        }
        
        public override void DoDestory()
        {
            base.DoDestory();
            EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
        }
    }
}