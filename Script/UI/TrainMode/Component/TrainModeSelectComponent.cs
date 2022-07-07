using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using OtherMgr;
using Script.DB;
using Script.DB.DBModel;
using Script.Excel.Table;
using Script.Main;
using Script.Mgr;
using UI.Base;
using UIModel.TrainMode;
using UnityEngine;

namespace UI.TrainMode
{
    public class TrainModeSelectComponent : UComponent
    {
        public List<TrainScrollItemData>[] Datas = new List<TrainScrollItemData>[3];
        public List<ScrollViewComponent> ScrollViewComponents;
        public List<DegreeItem> DegreeItems;
        public List<GameObject> TrainList;
        public RectTransform ScrollContent;

        public static int degreeIndex;
        public static int selectIndex;

        public override void DoOpen()
        {
            base.DoOpen();
        }

        public void InitData()
        {
            for (int i = 0; i < Datas.Length; i++)
            {
                Datas[i] = new List<TrainScrollItemData>();
            }
            var m = from a in ExcelMgr.TDTraining.GetDictionary().Values
                join j in DBManager.Instance.Table<TrainTable>() on a.Id equals j.TrainId
                into b
                select (a,b);
            foreach (var ab in m)
            {
                TDTraining training = ab.a;
                int starNum = ab.b.Count() == 0 ? 0 : ab.b.First().Star;
                int score = ab.b.Count() == 0 ? 0 : ab.b.First().Score;
                bool isLocked;
                GetIslocked(training.Id,out isLocked);
                TrainScrollItemData trainScrollItem = new TrainScrollItemData(training.Id ,isLocked ,training.name ,starNum ,score);
                switch (training.degree)
                {
                    case "轻松":
                        trainScrollItem.Degree = 0;
                        Datas[0].Add(trainScrollItem);
                        break;
                    case "适中":
                        trainScrollItem.Degree = 1;
                        Datas[1].Add(trainScrollItem);
                        break;
                    default:
                        trainScrollItem.Degree = 2;
                        Datas[2].Add(trainScrollItem);
                        break;
                }
            }
            for (int i = 0; i < ScrollViewComponents.Count; i++)
            {
                ScrollViewComponents[i].UpdateData(Datas[i]);
                ScrollViewComponents[i].ScrollTo(0);
            }

            degreeIndex = 0;
            selectIndex = 0;
            Refresh();
        }

        public void MoveUp()
        {
            if(selectIndex <= 0)
                return;
            ChangeSelectIndex(-1);
        }
        
        public void MoveLeft()
        {
            if(degreeIndex == 0)
                return;
            ChangeDegree(-1);
        }
        
        public void MoveDown()
        {
            if(selectIndex+1 >= Datas[degreeIndex].Count)
                return;
            ChangeSelectIndex(1);
        }
        
        public void MoveRight()
        {
            if(degreeIndex == 2)
                return;
            ChangeDegree(1);
            
        }

        public TrainScrollItemData GetSelectedData()
        {
            return Datas[degreeIndex][selectIndex];
        }

        void ChangeSelectIndex(int changeValue)
        {
            AudioManager.PlayAudioEffectA("选中框移动");
            int oldValue = selectIndex;
            selectIndex += changeValue;
            Refresh();
        }

        void ChangeDegree(int changeValue)
        {
            degreeIndex += changeValue;
            ScrollViewComponents[degreeIndex].JumpTo(selectIndex);
            ScrollContent.DOLocalMoveX(-405 - degreeIndex * 810f,0.3f ,true);
            WaitTimeMgr.WaitTime(0.3f, () =>
            {
                Refresh();
            });
            
            int i = 0;
            foreach (var item in DegreeItems)
            {
                if (i == degreeIndex)
                    item.IsSelected = true;
                else
                    item.IsSelected = false;
                i++;
            }
        }

        public void Refresh()
        {
            TDTraining data = ExcelMgr.TDTraining.Get(Datas[degreeIndex][selectIndex].ExcelId);
            TrainViewMgr.TrainSelectView.SetData(data);
            ScrollViewComponents[degreeIndex].ScrollTo(selectIndex);
            DegreeItems[degreeIndex].IsSelected = true;
        }

        void GetIslocked(string id ,out bool isLocked)
        {
            int test = Int32.Parse(id);
            if (test <= 2000)
                isLocked = false;
            else
                isLocked = true;
        }
    }
}