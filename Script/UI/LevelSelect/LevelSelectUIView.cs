﻿using System;
 using System.Collections;
using DG.Tweening;
 using Events;
 using OtherMgr;
 using Player;
 using Script.DB;
 using Script.DB.DBModel;
 using Script.Excel.Table;
 using Script.Main;
 using Script.Mgr;
 using Script.Model;
 using UI.Base;
 using UnityEngine;
 using UnityEngine.UI;

 namespace UI.LevelSelect
{
    public class LevelSelectUIView: UView
    {
        public override bool DefaultShow => false;
        public Animator enterAni;
        /**关卡信息区*/
        public Image[] lv_info_bg;
        public Text lv_info_name;
        public Text lv_info_des;
        public Text lv_info_count;
        public Text lv_info_sss;
        public Text lv_info_passNum;
        private string[] noToW = new string[4]{"一","二","三","四"};
        /**map区域*/
        public RectTransform[] buoy_list;
        public RectTransform selectBuoy;
        public LvItemCtr[] item_list;
        private int itemIdx=0;
        private TDLevel levelData;
        private bool isAddEvent = false;
        /**------*/
        /**-1=初始化,0=动画,1=有选项,2=开始加载战斗*/
        public int viewType = -1;

        private bool isJump = false;
        public override void DoCreat()
        {
            base.DoCreat();
            gameObject.SetActive(isJump);
            // HallViewMgr.LevelSelectUIView = this;
            EventCenter.ins.AddEventListener<KeyCode>("KeyDown",KeyDown);
        }
        
        public override void DoOpen()
        {
            base.DoOpen();
            AudioManager.PlayAudioEffectA("打开弹窗");
        }

        public void JumpShow()
        {
            isJump = true;
            Show();
        }
        
        public override void Show(bool b = true)
        {
            base.Show(b);
            if (b)
            {
                if (isJump)
                {
                    enterAni.Play("LevelSelectUI", 0, 1f);
                }
                else
                {
                    enterAni.Play("LevelSelectUI", 0, 0);
                }
                isJump = false;
                int len = item_list.Length;
                for (int i = 0; i < len; i++)
                {
                    item_list[i].SetSelect(0==i);
                }
                Global.instance.StartCoroutine(delayViewType(0.5f));
            
                levelData = ExcelMgr.TDLevel.Get("1001");
                itemIdx = 0;
                updateMapInfo();
            }
            else
            {
                isAddEvent = false;
            }
        }
        
        private IEnumerator delayViewType(float time)
        {
            yield return new WaitForSeconds(time);
            viewType=1;
            isAddEvent = true;
        }
        
        void KeyDown(KeyCode keyCode)
        {
            if(!IsTop() || !IsActive() ||!isAddEvent)
                return;
            if (1==viewType)
            {
                switch (keyCode)
                {
                    case KeyCode.W:
                        
                        break;
                    case KeyCode.A:
                        item_list[itemIdx].SetSelect(false);
                        itemIdx = itemIdx>0?itemIdx-1:item_list.Length-1;
                        item_list[itemIdx].SetSelect(true);
                        //
                        AudioManager.PlayAudioEffectA("选中框移动");
                        //
                        updateBuoy();
                        updateMapInfo();
                        break;
                    case KeyCode.S:
                        
                        break;
                    case KeyCode.D:
                        item_list[itemIdx].SetSelect(false);
                        itemIdx = itemIdx<item_list.Length-1?itemIdx+1:0;
                        item_list[itemIdx].SetSelect(true);
                        //
                        AudioManager.PlayAudioEffectA("选中框移动");
                        //
                        updateBuoy();
                        updateMapInfo();
                        break;
                    case KeyCode.J:
                        //GameVariable.Init((itemIdx+1001)+"");
                        viewType = 2;
                        AudioManager.PlayAudioEffectA("选中确认");
                        string sId = itemIdx + 1001 + "";
                        GameVariable.InitLevel(sId);
                        Global.SceneMgr.EnterGameScene();
                        break;
                    case KeyCode.K:
                        //LoginUIMgr.LoginView.Show(true);
                        AudioManager.PlayAudioEffectA("返回");
                        Show(false);
                        if(!HallViewMgr.MainView.IsActive())
                            HallViewMgr.MainView.DoOpen();
                        break;
                }
            }
            
        }

        private void updateMapInfo()
        {
            string levelId = (itemIdx + 1001).ToString();
            levelData = ExcelMgr.TDLevel.Get(levelId);
            for (int i = 0; i < 4; i++)
            {
                lv_info_bg[i].gameObject.SetActive(i==itemIdx);
            }
            lv_info_des.text = levelData.desc;
            lv_info_name.text = "第"+noToW[itemIdx]+"关 "+levelData.name ;
            LevelTable levelTable = DBManager.Instance.Find<LevelTable>(PlayerInfo.Instance.UId + Int32.Parse(levelId));
            if (levelTable == null)
            {
                lv_info_count.text = "--";
                lv_info_passNum.text = "--";
                lv_info_sss.text = "--";
            }
            else
            {
                lv_info_count.text = levelTable.Score.ToString();
                lv_info_passNum.text = levelTable.ClearanceTime.ToString();
                lv_info_sss.text = ((SType)levelTable.Grade).ToString();
            }
            
        }

        private void updateBuoy()
        {
            Vector3 v3 = buoy_list[itemIdx].localPosition;
            selectBuoy.localPosition = new Vector3(v3.x,v3.y-10,0);
        }
        
        protected override void DoDestroy()
        {
            isAddEvent = false;
            EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
            base.DoDestroy();
        }
        
    }
}