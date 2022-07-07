using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events;
using OtherMgr;
using Script.DB;
using Script.DB.DBModel;
using Script.Excel.Table;
using Script.Main;
using Script.Mgr;
using UI.Base;
using UIModel.TrainMode;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TrainMode
{
    public class SelectMusicView : UView
    {
        [Header("选择音乐的滚动条")]
        public MusicSelectScrollViewComponent MusicSelectScrollViewComponent;

        [SerializeField] private Text _title;


        public TDMusic CurData;
        private List<MusicItemData> Datas;
        public string ActName;

        private int _selectIndex;
        
        public override void DoCreat()
        {
            base.DoCreat();
            TrainViewMgr.SelectMusicView = this;
            EventCenter.ins.AddEventListener<KeyCode>("KeyDown",KeyDown);
            InitDatas();
            MusicSelectScrollViewComponent.UpdateData(Datas);
            MusicSelectScrollViewComponent.ScrollTo(0 ,0);
        }

        void InitDatas()
        {
            Datas = new List<MusicItemData>();
            Datas.Add(new MusicItemData("0",false,true,"随机歌曲" ,0,0));
            int i = 0;
            var m = from a in ExcelMgr.TDMusic.GetDictionary().Values
                join j in DBManager.Instance.Table<MusicTable>() on a.Id equals j.MusicId
                into b
                select (a, b);

            foreach (var ab in m)
            {
                if(ab.a.Hide)
                    continue;
                MusicItemData data;
                float speed = (float) 135 / ab.a.Bpm;
                int time = ab.b.Count() == 0 ? 0 : ab.b.First().PlayTime;
                data = new MusicItemData(ab.a.Id ,false ,false ,ab.a.Name,time ,(int)(speed * TrainViewMgr.TrainSelectView.CurData.time) / 60);
                /*if(i < 5)
                    data = new MusicItemData(kvp.Value.Id ,false ,false ,kvp.Value.Id,0 ,(int)(speed * TrainViewMgr.TrainSelectView.CurData.time) / 60);
                else
                {
                    data = new MusicItemData(kvp.Value.Id ,true ,false ,kvp.Value.Id,0 ,(int)(speed * TrainViewMgr.TrainSelectView.CurData.time) / 60);
                }*/
                Datas.Add(data);
                i++;
            }
        }

        public string GetSelectedDataID()
        {
            return Datas[_selectIndex].MusicId;
        }

        void KeyDown(KeyCode keyCode)
        {
            if (!IsTop() || !IsActive())
                return;
            switch (keyCode)
            {
                case KeyCode.W:
                    MoveUp();
                    break;
                case KeyCode.A:
                    break;
                case KeyCode.S:
                    MoveDown();
                    break;
                case KeyCode.D:
                    break;
                case KeyCode.J:
                    Confirm();
                    break;
                case KeyCode.H:
                    break;
                case KeyCode.K:
                    StartCoroutine(OpenTrainSelectView());
                    break;
            }
        }
        
        void MoveUp()
        {
            if(_selectIndex <= 0)
                return;
            ChangeSelectIndex(-1);
        }
        
        void MoveDown()
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

        void Confirm()
        {
            AudioManager.PlayAudioEffectA("选中确认");
            MusicItemData data = Datas[_selectIndex];
            if (data.IsLocked)
            {
                CommonUIMgr.PopupFrame.ShowTips("  音乐没有解锁  ");
                return;
            }
            if (data.IsRandom)
            {
                var UnLockedIndexs = (from element in Datas
                    where !element.IsLocked && !element.IsRandom
                    select element).ToArray();
                int random = Random.Range(0, UnLockedIndexs.Count() - 1);
                data = UnLockedIndexs[random];
                CurData = ExcelMgr.TDMusic.Get(data.MusicId);;
            }

            TrainViewMgr.SelectSceneView.title = ActName + " - " + data.Name;
            StartCoroutine(OpenSelectSceneView());
        }
        
        IEnumerator OpenTrainSelectView()
        {
            yield return 0;
            DoClose();
            TrainViewMgr.TrainSelectView.DoOpen();
        }
        
        IEnumerator OpenSelectSceneView()
        {
            yield return 0;
            DoClose();
            TrainViewMgr.SelectSceneView.DoOpen();
        }
        
        public override void DoOpen()
        {
            base.DoOpen();
            AudioManager.PlayAudioEffectA("打开弹窗");
            _title.text = "- " + ActName;
            _selectIndex = 0;
            MusicSelectScrollViewComponent.ScrollTo(_selectIndex ,0);
            RefreshData();
            GraduallyShow(0.3f);
            
            EventCenter.ins.EventTrigger("CameraMove" ,ExcelMgr.TDCameraLocation.Get("SelectMusicView").location);
        }
        
        public void Refresh()
        {
            MusicSelectScrollViewComponent.ScrollTo(_selectIndex);
            RefreshData();
        }

        void RefreshData()
        {
            CurData = ExcelMgr.TDMusic.Get(GetSelectedDataID());
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