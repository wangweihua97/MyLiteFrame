using System.Collections;
using System.Collections.Generic;
using Events;
using OtherMgr;
using Script.Excel.Table;
using Script.Main;
using Script.Mgr;
using Script.Model;
using UI.Base;
using UIModel.TrainMode;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TrainMode
{
    public class SelectSceneView : UView
    {
        [Header("选择音乐的滚动条")]
        public SceneSelectScrollViewComponent SceneSelectScrollViewComponent;

        [SerializeField] private Text _titleText;


        public TDScene CurData;
        private List<SceneItemData> Datas;
        public string title;

        private int _selectIndex;
        private int _count;
        private bool _canClick;
        
        public override void DoCreat()
        {
            base.DoCreat();
            TrainViewMgr.SelectSceneView = this;
            EventCenter.ins.AddEventListener<KeyCode>("KeyDown",KeyDown);
            InitDatas();
            SceneSelectScrollViewComponent.UpdateData(Datas);
            SceneSelectScrollViewComponent.ScrollTo(0 ,0);
        }

        void InitDatas()
        {
            Datas = new List<SceneItemData>();
            _count = 0;
            for (int i = 0; i < 5; i++)
            {
                _count = 0;
                Datas.Add(new SceneItemData("0",false,true,"随机场景" ,""));
                _count++;
                foreach (var kvp in ExcelMgr.TDScene.GetDictionary())
                {
                    SceneItemData sceneItemData = new SceneItemData(kvp.Value.Id ,false ,false ,kvp.Value.Id ,kvp.Value.icon);
                    Datas.Add(sceneItemData);
                    _count++;
                }
            }
        }

        public string GetSelectedDataID()
        {
            return Datas[_selectIndex].SceneId;
        }

        void KeyDown(KeyCode keyCode)
        {
            if (!IsTop() || !IsActive() || !_canClick)
                return;
            switch (keyCode)
            {
                case KeyCode.W:
                    break;
                case KeyCode.A:
                    MoveLeft();
                    break;
                case KeyCode.S:
                    break;
                case KeyCode.D:
                    MoveRight();
                    break;
                case KeyCode.J:
                    Confirm();
                    break;
                case KeyCode.H:
                    break;
                case KeyCode.K:
                    StartCoroutine(OpenSelectMusicView());
                    break;
            }
        }
        
        void Confirm()
        {
            AudioManager.PlayAudioEffectA("选中确认");
            SceneItemData data = Datas[_selectIndex];
            if (data.IsLocked)
            {
                CommonUIMgr.PopupFrame.ShowTips("  场景没有解锁  ");
                return;
            }
            if (data.IsRandom)
            {
                int[] UnLockedIndexs = new int[_count - 1];
                int count = 0;
                for (int i = 1; i < _count; i++)
                {
                    if (!Datas[i].IsLocked)
                    {
                        UnLockedIndexs[count] = i;
                        count++;
                    }
                }
                int random = Random.Range(0, count - 1);
                data = Datas[UnLockedIndexs[random]];
            }

            GameVariable.InitTrainMode(TrainViewMgr.TrainSelectView.CurData, TrainViewMgr.SelectMusicView.CurData.Id,
                data.SceneId);
            Global.SceneMgr.EnterGameScene();
        }
        
        void MoveLeft()
        {
            if(_selectIndex <= 0)
                return;
            ChangeSelectIndex(-1);
        }
        
        void MoveRight()
        {
            if(_selectIndex + 1 >= Datas.Count)
                return;
            ChangeSelectIndex(1);
        }

        void ChangeSelectIndex(int value)
        {
            AudioManager.PlayAudioEffectA("选中框移动");
            _selectIndex += value;
            Refresh();
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
            AudioManager.PlayAudioEffectA("打开弹窗");
            _titleText.text = "- " + title;
            _selectIndex = Datas.Count*2/5;
            SceneSelectScrollViewComponent.ScrollTo(_selectIndex ,0);
            RefreshData();
            GraduallyShow(0.3f);
            _canClick = true;
            
            EventCenter.ins.EventTrigger("CameraMove" ,ExcelMgr.TDCameraLocation.Get("SelectSceneView").location);
        }
        
        public void Refresh()
        {
            if (_selectIndex < _count)
            {
                _canClick = false;
                SceneSelectScrollViewComponent.ScrollTo(_selectIndex ,0.3f ,()=>
                {
                    StartCoroutine(ScrpllCenter(_count));
                });
            }
            else if (_selectIndex > Datas.Count - _count)
            {
                _canClick = false;
                SceneSelectScrollViewComponent.ScrollTo(_selectIndex ,0.3f ,()=>
                {
                    StartCoroutine(ScrpllCenter(-_count));
                });
            }
            else
            {
                SceneSelectScrollViewComponent.ScrollTo(_selectIndex);
            }

            RefreshData();
        }

        void RefreshData()
        {
            CurData = ExcelMgr.TDScene.Get(GetSelectedDataID());
        }

        IEnumerator ScrpllCenter(int value)
        {
            yield return 0;
            _canClick = true;
            _selectIndex += value;
            SceneSelectScrollViewComponent.ScrollTo(_selectIndex ,0);
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