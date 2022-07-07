﻿using System.Collections;
using System.Collections.Generic;
 using Events;
 using Ghost;
 using Player;
 using Script.Excel;
 using Script.Excel.Table;
 using Script.Game;
 using Script.Main;
 using Script.Mgr;
 using UnityEngine;

namespace Script.Model
{
    public class ActionFlowModel
    {
        public float DeltaTime;
        
        private CsvTable<TDActionFlow> curActionFlow;
        private List<string> actionFlowID;
        private List<string> actionID;
        private int curStep;
        private float delay;
        private int preStep;
        private float preDelay;
        private bool isLife;

        private GameTipModel GameTipModel;
        
        public void Init(float deltaTime)
        {
            if(GameTipModel == null)
                GameTipModel = new GameTipModel();
            curActionFlow = ExcelMgr.TDActionFlow[GameVariable.CurBattleIndex];
            actionFlowID = curActionFlow.GetKeys();
            if(actionID == null)
                actionID = ExcelMgr.TDAction.GetKeys();
            curStep = 0;
            preStep = 0;
            delay = GameVariable.delayTime;
            DeltaTime = deltaTime;
            preDelay = -1 * GameVariable.BirthToClickTime + GameVariable.delayTime;
            isLife = true;
            Execute();
            PreExecute();
        }

        public void OnUpdate()
        {
            if(!isLife)
                return;
            delay -= Time.deltaTime;
            preDelay -= Time.deltaTime;
            if (delay <= 0)
                Execute();
            if (preDelay <= 0)
                PreExecute();
            GameTipModel.Check();
        }

        //执行表中数据
        void Execute()
        {
            if (curStep >= actionFlowID.Count)
            {
                EventCenter.ins.EventTrigger("FinalFlow");
                return;
            }
            TDActionFlow data = curActionFlow.Get(actionFlowID[curStep]);
            if (data.body.Equals("左") != MainGameCtr.instance.bodyIsLeft)
            {
                ChangeBody();
            }
            if (data.story != 0)
            {
                IntoStory(data);
            }
            if (data.rhythmEffects == 1)
            {
                PlayRhythmEffects();
            }

            if (data.node != null && data.node.Count > 0)
            {
                IntoNode(data);
            }
            curStep++;
            delay += GameVariable.beatTime;
            if (!data.delay.Equals(Const.NULL_FLOAT))
                delay += data.delay / 1000;
            
            if (delay <= 0)
                Execute();
        }

        void ChangeBody()
        {
            MainGameCtr.instance.bodyIsLeft = !MainGameCtr.instance.bodyIsLeft;
            PlayerMgr.instance.playerAnimationMgr.ChangeBodyPosture(MainGameCtr.instance.bodyIsLeft);
            PlayerMgr.CoachInstance.playerAnimationMgr.ChangeBodyPosture(MainGameCtr.instance.bodyIsLeft);
            EventCenter.ins.EventTrigger("ChangeBody");
        }

        void IntoStory(TDActionFlow data)
        {
            EventCenter.ins.EventTrigger("IntoStory",data.story);
        }

        void PlayRhythmEffects()
        {
            Global.instance.StartCoroutine(PlayBeltItemEffect(delay - (float)GameVariable.preEffect/1000));
        }

        void IntoNode(TDActionFlow data)
        {
            /*if(data.node.ContainsKey("die"))
                EventCenter.ins.EventTrigger("InterEnemyDieNode" ,data.node["die"]);
            if(data.node.ContainsKey("run"))
                EventCenter.ins.EventTrigger("InterPlayerRunNode" ,data.node["run"]);*/
        }
        
        IEnumerator PlayBeltItemEffect(float time)
        {
            yield return new WaitForSeconds(time);
            EventCenter.ins.EventTrigger("PlayBeltItemEffect");
        }

        //预先执行表中数据
        void PreExecute()
        {
            if (preStep >= actionFlowID.Count)
                return;
            TDActionFlow data = curActionFlow.Get(actionFlowID[preStep]);
            if (data.left != Const.NULL_STRING || data.right != Const.NULL_STRING || data.all != Const.NULL_STRING)
            {
                DoAction(data);
            }
            GameTipModel.RefreshGroup(data.group);
            if (!data.text.Equals(Const.NULL_STRING))
            {
                ShowTips(data);
            }

            preStep++;
            preDelay += GameVariable.beatTime;
            if (!data.delay.Equals(Const.NULL_FLOAT))
                preDelay += data.delay / 1000;
            if (preDelay <= 0)
                PreExecute();
        }

        void DoAction(TDActionFlow data)
        {
            ActionModel actionModel = CreatModel(data);
            EventCenter.ins.EventTrigger("CreatActionFlow",actionModel);
        }

        void ShowTips(TDActionFlow data)
        {
            GameTipModel.EnQueue(preDelay - (float)data.textTime/1000 - ((float)GameVariable.preTime/1000) ,data.text ,data.textSoundRes);
        }
        //创建动作
        ActionModel CreatModel(TDActionFlow data)
        {
            TDAction actionData = PlayerActionMgr.GetActionData(data.body, data.left, data.right, data.all);
            if (actionData == null)
            {
                Debug.LogError("------------------没有找到该动作对于的动画-------------------");
                return null;
            }
            AcitonLocat acitonLocat = AcitonLocat.Left;
            if (!data.right.Equals(Const.NULL_STRING))
            {
                acitonLocat = AcitonLocat.Right;
            }
            else if (!data.all.Equals(Const.NULL_STRING))
            {
                acitonLocat = AcitonLocat.All;
            }
            ActionModel actionModel = new ActionModel(GameVariable.speed ,acitonLocat);
            actionModel.SetData(data);
            actionModel.SetData(actionData);
            actionModel.offeTime = preDelay - GameVariable.preTime/1000;
            return actionModel;
        }

        public void Clear()
        {
            isLife = false;
        }
    }
}