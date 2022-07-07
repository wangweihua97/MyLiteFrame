using System;
using System.Collections.Generic;
using OtherMgr;
using Script.Excel.Table;
using UI.Base;
using UIModel.TrainMode;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace UI.TrainMode
{
    public class DetailsComponent : UComponent
    {
        [Header("训练时间")]
        [SerializeField] Text Time;
        
        [Header("卡路里")]
        [SerializeField] Text Calorie;
        
        [Header("分数")]
        [SerializeField] Text Grade;
        
        [Header("训练部位")]
        [SerializeField] Transform TrainPart;
        
        [Header("包含动作")]
        [SerializeField] Transform ContentsAct;

        private List<IconItem> trainPartItems;
        private List<IconItem> ContentsActItems;

        private void Awake()
        {
            trainPartItems = new List<IconItem>();
            ContentsActItems = new List<IconItem>();
        }

        public void SetData(TDTraining data ,TrainScrollItemData trainScrollItemData)
        {
            Clear();
            Time.text = data.time/60 + ":";
            int s = data.time % 60;
            if (s < 10)
            {
                Time.text += "0" + s;
            }
            else
            {
                Time.text += s;
            }
            Calorie.text = data.calorie.ToString();
            Grade.text = trainScrollItemData.Score.ToString();
            for (int i = 0; i < data.bodyparts.Count; i++)
            {
                GameObject go = PoolManager.CommonPool.Spawn("Prefab/IconItem", TrainPart);
                IconItem iconItem = go.GetComponent<IconItem>();
                iconItem.SetImage(SpritesMgr.Get(data.bodyparts[i])?.Sprite ,82f);
                trainPartItems.Add(iconItem);
            }
            
            for (int i = 0; i < data.basicmovement.Count; i++)
            {
                GameObject go = PoolManager.CommonPool.Spawn("Prefab/IconItem", ContentsAct);
                IconItem iconItem = go.GetComponent<IconItem>();
                iconItem.SetImage(SpritesMgr.Get(data.basicmovement[i])?.Sprite ,100f);
                ContentsActItems.Add(iconItem);
            }
        }

        public void Clear()
        {
            Grade.text = "0";
            foreach (var IconItem in trainPartItems)
            {
                PoolManager.CommonPool.DesSpawn(IconItem.gameObject);
            }
            trainPartItems.Clear();
            
            foreach (var IconItem in ContentsActItems)
            {
                PoolManager.CommonPool.DesSpawn(IconItem.gameObject);
            }
            ContentsActItems.Clear();
        }
    }
}