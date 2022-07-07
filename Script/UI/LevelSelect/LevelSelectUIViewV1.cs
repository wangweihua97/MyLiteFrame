﻿using System;
 using System.Collections;
 using System.Collections.Generic;
 using DG.Tweening;
 using Events;
 using FancyScrollView;
 using OtherMgr;
 using Player;
 using Script.DB;
 using Script.DB.DBModel;
 using Script.Excel.Table;
 using Script.Main;
 using Script.Mgr;
 using Script.Model;
 using Script.Scene.Hall;
 using UI.Base;
 using UI.CreatePlayer.vo;
 using UnityEngine;
 using UnityEngine.UI;
 using Ease = EasingCore.Ease;

 namespace UI.LevelSelect
{
    public class LevelSelectUIViewV1: InOutUView
    {
        [Header("列表组件")]
        public ScrollView list;
        public LvInfoCtr lvInfoCtr;
        public GameObject bg;
        public GameObject btn_tips;

        public override bool DefaultShow => false;
        private int listIdx = 0;
        private int listLen = 0;
        private List<ItemData> dataList;
        
        public override void DoCreat()
        {
            base.DoCreat();
            HallViewMgr.LevelSelectUIViewV1 = this;
            initListData();
        }
        
        void initListData()
        {
            dataList = new List<ItemData>();
            LvItemVo vo;
            //LevelTable levelTable = null;
            //前占位
            for (int i = 0; i < 4; i++)
            {
                vo = new LvItemVo();
                vo.type = -1;
                dataList.Add(vo);
            }
            //
            int index = 0;
            foreach (var kvp in ExcelMgr.TDLevel.GetDictionary())
            {
                /*levelTable = DBManager.Instance.Find<LevelTable>(PlayerInfo.Instance.UId + Int32.Parse(kvp.Value.Id));*/
                //
                vo = new LvItemVo();
                vo.cfg = kvp.Value;
                // vo.type = 0;
                //vo.type = levelTable != null && levelTable.ClearanceTime > 0 ? 0 : 2 ;
                vo.type = index <= PlayerInfo.Instance.PlayerData.PassMaxLevel ? 1 : 2;
                dataList.Add(vo);
                index++;
            }
            //后占位
            for (int i = 0; i < 4; i++)
            {
                vo = new LvItemVo();
                vo.type = -1;
                dataList.Add(vo);
            }
            //
            listIdx = 4;
            listLen = dataList.Count;
            list.UpdateData(dataList);
            updateListSelect();
            updateLevelInfo();
        }
        void updateListSelect()
        {
            list.ScrollTo(listIdx,0.2f, Ease.InOutQuint, Alignment.Middle);
            EarthMap.Instance.SelectLevel(listIdx - 4);
            // list.UpdateSelection(listIdx);
        }

        void updateLevelInfo()
        {
            lvInfoCtr.SetData((LvItemVo)dataList[listIdx]);
        }
        
        public override void DoOpen()
        {
            base.DoOpen();
            AudioManager.PlayAudioEffectA("打开弹窗");
            EarthMap.Instance.Init();
            listIdx = PlayerInfo.Instance.PlayerData.PassMaxLevel + 4;
            listIdx = listIdx >= ExcelMgr.TDLevel.Count + 4 ? ExcelMgr.TDLevel.Count + 3 : listIdx;
            list.ScrollTo(listIdx ,0 ,Ease.InOutQuint, Alignment.Middle);
            // EventCenter.ins.AddEventListener<KeyCode>("KeyDown",KeyDown);
            // Global.instance.StartCoroutine(delayViewType(0.5f));
            EventCenter.ins.EventTrigger("CameraMove" ,ExcelMgr.TDCameraLocation.Get("SelectLevelView").location);
            EventCenter.ins.EventTrigger("bcfMgr_playAni");
            
            GraduallyShow(gameObject, 0.3f);
            // Global.instance.StartCoroutine(delayFun(0.5f));
        }
        protected override void enterStageComplete()
        {
            EventCenter.ins.AddEventListener<KeyCode>("KeyDown",KeyDown);
            WaitTimeMgr.WaitTime(0.5f, () =>
            {
                EarthMap.Instance.SelectLevel(listIdx - 4);
            });
        }
        
        private IEnumerator delayFun(float time)
        {
            yield return new WaitForSeconds(time);
            // enterAni.enabled = false;
        }

        /*private IEnumerator delayViewType(float time)
        {
            yield return new WaitForSeconds(time);
            viewType=1;
            isAddEvent = true;
        }*/
        
        void KeyDown(KeyCode keyCode)
        {
            switch (keyCode)
            {
                case KeyCode.W:
                    break;
                case KeyCode.A:
                    AudioManager.PlayAudioEffectA("选中框移动");
                    listIdx--;
                    if (listIdx<4)
                    {
                        // listIdx += listLen-4;
                        listIdx = 4;
                        return;
                    }
                    updateListSelect();
                    updateLevelInfo();
                    break;
                case KeyCode.S:
                    break;
                case KeyCode.D:
                    AudioManager.PlayAudioEffectA("选中框移动");
                    listIdx++;
                    if (listIdx>=listLen-4)
                    {
                        listIdx = listLen-4;
                        return;
                    }

                    if (listIdx - 4 > PlayerInfo.Instance.PlayerData.PassMaxLevel)
                    {
                        listIdx--;
                        return;
                    }
                    updateListSelect();
                    updateLevelInfo();
                    break;
                case KeyCode.J:
                    AudioManager.PlayAudioEffectA("选中确认");
                    EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
                    list.gameObject.SetActive(false);
                    bg.gameObject.SetActive(false);
                    btn_tips.gameObject.SetActive(false);
                    StoryMgr.CheckStory(CheckStoryStep.SelectLevel, (haveStory) =>
                    {
                        if (haveStory)
                        {
                            
                        }
                        else
                        {
                            
                        }
                        list.gameObject.SetActive(true);
                        bg.gameObject.SetActive(true);
                        btn_tips.gameObject.SetActive(true);
                        DoClose();
                        string sId = listIdx - 4 + 1 + "";
                        // string sId = listIdx - 4 + 1001 + "";
                        GameVariable.InitLevel(sId);
                        Global.SceneMgr.EnterGameScene();
                    });
                    break;
                case KeyCode.K:
                    AudioManager.PlayAudioEffectA("返回");
                    DoClose();
                    EventCenter.ins.EventTrigger("bcfMgr_playAni_back");
                    EventCenter.ins.AddEventListener("bcfMgr_playAni_complete",bcfMgr_playAni_complete);
                    break;
            }
        }

        void bcfMgr_playAni_complete()
        {
            EventCenter.ins.RemoveEventListener("bcfMgr_playAni_complete",bcfMgr_playAni_complete);
            // EventCenter.ins.EventTrigger("bcfMgr_createPlayer");
            HallViewMgr.MainView.CameraMoveBTypeOnce = true;
            if(!HallViewMgr.MainView.IsActive())
                HallViewMgr.MainView.DoOpen();
        }
        
        public override void DoClose()
        {
            EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
            EarthMap.Instance.Exit();
            base.DoClose();
        }

        protected override void DoDestroy()
        {
            EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
            base.DoDestroy();
        }
        
    }
}