﻿using System.Collections;
using System.Collections.Generic;
 using Events;
 using Ghost;
 using OtherMgr;
 using Script.Game;
 using Script.Main;
 using Script.Model;
 using UI.Base;
 using UnityEngine;

namespace UI
{
    public class BeltItem : UComponent
    {
        public MovingBelt parent; 
        List<ActionItem> queue = new List<ActionItem>();
        private int delectNum;
        public int index;
        public bool isLife;
        public GameObject rhythmEffectPrefab;
        public Transform ItemBrithLocation;
        public bool isLeft;
        public GameObject MissGO;
        public GameObject ComBoEffect;
        public GameObject ComBoRhythmEffect;
        private bool _isCombo;

        public void Init(MovingBelt movingBelt)
        {
            index = 0;
            parent = movingBelt;
            isLife = true;
            _isCombo = false;
            EventCenter.ins.AddEventListener("PlayBeltItemEffect",PlayEffect);
            EventCenter.ins.AddEventListener("EnterComBo",EnterComBo);
            EventCenter.ins.AddEventListener("ExitComBo",ExitComBo);
        }

        void PlayEffect()
        {
            if (MainGameCtr.instance.IsComBo)
            {
                _isCombo = true;
                ComBoRhythmEffect.SetActive(true);
            }
            else
                rhythmEffectPrefab.SetActive(true);
            Global.instance.StartCoroutine(EffectMiss());
        }

        void EnterComBo()
        {
            ComBoEffect.SetActive(true);
        }
        
        void ExitComBo()
        {
            ComBoEffect.SetActive(false);
        }

        public void PlayMiss()
        {
            MissGO.SetActive(true);
            MissGO.GetComponent<Animator>().Rebind();
        }

        IEnumerator EffectMiss()
        {
            float missTime = GameVariable.beatTime < 0.3f ? GameVariable.beatTime : 0.3f;
            yield return new WaitForSeconds(missTime);
            if (_isCombo)
            {
                _isCombo = false;
                ComBoRhythmEffect.SetActive(false);
            }
            rhythmEffectPrefab.SetActive(false);
        }

        public void UpdateMove()
        {
            if(!isLife)
                return;
            if(queue.Count == 0)
                return;
            foreach (var item in queue)
            {
                item.DoMove();
            }
            if(delectNum > 0)
                DoDelect();
        }

        public void DoDelect()
        {
            for (int i = 0; i < delectNum; i++)
            {
                queue.RemoveAt(0);
                index = --index < 0 ? 0 : index;
            }
            delectNum = 0;
        }
        
        public void AddIndex()
        {
            index++;
        }

        public void AddDelectNum()
        {
            delectNum++;
        }

        public void ClickMissing()
        {
            queue.RemoveAt(index);
        }
        
        public ActionItem GetCanClickItem()
        {
            if (queue.Count <= index)
                return null;
            return queue[index];
        }

        public void DoEnQueue(ActionModel actionModel)
        {
            ActionItem actionItem = PoolManager.GameObjPool.Spawn("Prefab/ActionItem").GetComponent<ActionItem>();
            actionItem.rectTransform.SetParent(ItemBrithLocation);
            actionItem.transform.localScale = new Vector3(1,1,1);
            float brithY = GameVariable.CenterY - (GameVariable.BirthToClickTime + actionModel.offeTime)* GameVariable.speed;
            actionItem.rectTransform.localPosition = new Vector3(0,brithY,-5);
            actionItem.Init(actionModel ,this);
            queue.Add(actionItem);
        }

        public ActionItem DoPeek()
        {
            if(queue.Count == 0)
                return null;
            return queue[0];
        }

        public void Close()
        {
            MissGO.SetActive(false);
            foreach (var item in queue)
            {
                item.OnDie();
                PoolManager.GameObjPool.DesSpawn(item.gameObject);
            }
            queue.Clear();
        }
        
        public void ActionItemMoveOut(ActionItem actionItem)
        {
            actionItem.OnDie();
            PoolManager.GameObjPool.DesSpawn(actionItem.gameObject);
            actionItem.parent.AddDelectNum();
        }

        public void OnDestory()
        {
            EventCenter.ins.RemoveEventListener("PlayBeltItemEffect",PlayEffect);
            EventCenter.ins.RemoveEventListener("EnterComBo",EnterComBo);
            EventCenter.ins.RemoveEventListener("ExitComBo",ExitComBo);
        }
    }
}