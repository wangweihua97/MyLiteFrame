using System.Collections;
using System.Collections.Generic;
using Events;
using Script.Excel.Table;
using Script.Main;
using Script.Mgr;
using Script.Model;
using UI.Base;
using UI.Common.VO;
using UI.SettlementView.item;
using UnityEngine;

namespace UI.SettlementView
{
    public class ResultView : InOutUView
    {
        public Item[] itemList;
        private ItemVo[] voList;
        public override void DoCreat()
        {
            base.DoCreat();
            SettlementUIMgr.ResultView = this;
            // initListData();
            voList = new ItemVo[3];
            for (int i = 0; i < 3; i++)
            {
                voList[i] = new ItemVo();
                voList[i].cfg = null;
                voList[i].count = 0;
            }
        }
        public override void DoOpen()
        {
            // AudioManager.PlayAudioEffectA("打开弹窗");
            // EventCenter.ins.AddEventListener<KeyCode>("KeyDown",KeyDown);
            // EventCenter.ins.EventTrigger("bcfMgr_playAni");
            // GraduallyShow(gameObject, 0.3f);
            // Global.instance.StartCoroutine(delayFun(0.5f));
            //
            // TDLevel levelData = ExcelMgr.TDLevel.Get(GameVariable.CurLevelId);
            // id2cfg(levelData._bouns1ID,0,levelData._Amount1);
            // id2cfg(levelData._bouns2ID,1,levelData._Amount2);
            // id2cfg(levelData._bouns3ID,2,levelData._Amount3);
            //
            base.DoOpen();
        }

        void id2cfg(string id,int idx,int count)
        {
            if (!string.IsNullOrEmpty(id))
            {
                voList[idx].cfg = ExcelMgr.TDItem.Get(id);
                voList[idx].count = count;
                itemList[idx].SetData(voList[idx]);
                itemList[idx].gameObject.SetActive(true);
            }
            else
            {
                itemList[idx].gameObject.SetActive(false);
            }
        }
        protected override void enterStageComplete()
        {
            base.enterStageComplete();
            EventCenter.ins.AddEventListener<KeyCode>("KeyDown",KeyDown);
        }
        void KeyDown(KeyCode keyCode)
        {
            switch (keyCode)
            {
                case KeyCode.W:
                    break;
                case KeyCode.A:
                    break;
                case KeyCode.S:
                    break;
                case KeyCode.D:
                    break;
                case KeyCode.J:
                    // AudioManager.PlayAudioEffectA("选中确认");
                    // DoClose();
                    // string sId = listIdx + 1001 + "";
                    // GameVariable.InitLevel(sId);
                    // Global.SceneMgr.EnterGameScene();
                    DoClose();
                    Global.SceneMgr.StartScene("HallScene" ,LoadingViewMgr.LoadingView);
                    EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown", KeyDown);
                    break;
                case KeyCode.K:
                    // AudioManager.PlayAudioEffectA("返回");
                    // DoClose();
                    // // EventCenter.ins.EventTrigger("bcfMgr_createPlayer");
                    // HallViewMgr.MainView.CameraMoveBTypeOnce = true;
                    // if(!HallViewMgr.MainView.IsActive())
                    //     HallViewMgr.MainView.DoOpen();
                    break;
            }
        }
        public override void DoClose()
        {
            EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
            base.DoClose();
        }

        protected override void DoDestroy()
        {
            // EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
            base.DoDestroy();
        }
        // Start is called before the first frame update
    }
}


