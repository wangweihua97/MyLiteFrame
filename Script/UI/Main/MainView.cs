using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Events;
using OtherMgr;
using Player;
using Script.Main;
using Script.Mgr;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main
{
    public class MainView : UView
    {
        public override bool DefaultShow => false;

        public List<SelectItemComponent> SelectItemComponents;
        public Animator aniInOut;
        public static int SelectIndex
        {
            get { return _selectIndex; }
        }
        private static int _selectIndex;
        [Component] public PlayerInfoComponent PlayerInfoComponent;

        public override void DoCreat()
        {
            base.DoCreat();
            HallViewMgr.MainView = this;
            EventCenter.ins.AddEventListener<KeyCode>("KeyDown",KeyDown);
            InitModeDescribe();
            _selectIndex = 0;
            SelectIndexMove(0);
            PlayerInfoComponent.ReadDbData();
        }

        protected override void LoadedScene()
        {
            base.LoadedScene();
        }

        /**true=正常移动,false=瞬间移动*/
        public bool CameraMoveBTypeOnce = false;
        public override void DoOpen()
        {
            base.DoOpen();
            
            GraduallyShow();
            
            aniInOut.Play("MainView_in");
            //场景环境设置
            EventCenter.ins.EventTrigger("bcfMgr_createPlayer");

            if (CameraMoveBTypeOnce)
            {
                EventCenter.ins.EventTrigger("CameraMove" ,ExcelMgr.TDCameraLocation.Get("MainView").location);
            }
            else
            {
                EventCenter.ins.EventTrigger("CameraMoveStraightaway" ,ExcelMgr.TDCameraLocation.Get("MainView").location);
            }
            StartCoroutine(openDelay());
        }
        
        IEnumerator openDelay()
        {
            yield return new WaitForSeconds(0.6f);
            SelectItemComponents[_selectIndex].BeSelected();
        }

        void InitModeDescribe()
        {
            int i = 0;
            foreach (var kvp in ExcelMgr.TDPlayMode.GetDictionary())
            {
                if(i > SelectItemComponents.Count)
                    break;
                SelectItemComponents[i].SetData(kvp.Value.desc1);
                i++;
            }

            while (i < SelectItemComponents.Count)
            {
                SelectItemComponents[i].SetData("在PlayMode中配置描述");
                i++;
            }
        }

        public override void DoClose()
        {
            base.DoClose();
            SelectItemComponents[_selectIndex].Leave();
        }

        public override void DoDestory()
        {
            EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
            base.DoDestory();
        }

        void KeyDown(KeyCode keyCode)
        {
            if(!IsTop() || !IsActive())
                return;
            switch (keyCode)
            {
                case KeyCode.W:
                    SelectIndexMove(-1);
                    AudioManager.PlayAudioEffectA("选中框移动");
                    break;
                case KeyCode.A:
                    break;
                case KeyCode.S:
                    SelectIndexMove(1);
                    AudioManager.PlayAudioEffectA("选中框移动");
                    break;
                case KeyCode.D:
                    break;
                case KeyCode.J:
                    if (0 ==_selectIndex)
                    {
                        AudioManager.PlayAudioEffectA("选中确认");
                        aniInOut.Play("MainView_out 0");
                        StartCoroutine(OpenLevelSelectUIView());
                    }
                    else if (2 ==_selectIndex)
                    {
                        AudioManager.PlayAudioEffectA("选中确认");
                        aniInOut.Play("MainView_out 0");
                        StartCoroutine(OpenTrainSelectView());
                    }
                    else if (_selectIndex == 3)
                    {
                        AudioManager.PlayAudioEffectA("选中确认");
                        aniInOut.Play("MainView_out 0");
                        StartCoroutine(OpenCharacterView());
                    }
                    break;
                case KeyCode.K:
                    break;
            }
            
        }

        IEnumerator OpenLevelSelectUIView()
        {
            yield return new WaitForSeconds(1f);
            DoClose();
            // yield return 0;
            HallViewMgr.LevelSelectUIViewV1.DoOpen();
            // HallViewMgr.LevelSelectUIViewV1.DoOpen();
        }
        
        IEnumerator OpenTrainSelectView()
        {
            yield return new WaitForSeconds(1f);
            // yield return 0;
            DoClose();
            TrainViewMgr.TrainSelectView.DoOpen();
        }
        
        IEnumerator OpenCharacterView()
        {
            yield return new WaitForSeconds(1f);
            // yield return 0;
            DoClose();
            CharacterUIMgr.DressUpView.DoOpen();
            // CharacterUIMgr.SelectSexView.DoOpen();
        }

        void SelectIndexMove(int move)
        {
            int newIndex = _selectIndex + move;
            if(newIndex < 0 || newIndex >= SelectItemComponents.Count)
                return;
            SelectItemComponents[_selectIndex].Leave();
            SelectItemComponents[newIndex].BeSelected();
            _selectIndex = newIndex;
        }
    }
}