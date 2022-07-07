﻿using System;
using System.Collections;
using DG.Tweening;
 using Events;
 using Ghost;
 using OtherMgr;
 using Script.Excel.Table;
 using Script.Game;
 using Script.Main;
 using Script.Mgr;
 using Script.Model;
 using UIModel;
 using UnitMgr;
 using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ActionItem : MonoBehaviour
    {
        public Image icon;
        public RectTransform rectTransform;
        public BeltItem parent;
        public ActionModel actionModel;
        public GameObject perfectPrefab;
        public GameObject veryGoodPrefab;
        public GameObject goodPrefab;
        public GameObject correctPrefab;
        //public GameObject missPrefab;
        private bool isLife;
        private bool isPlayMiss;
        private bool isShow;
        private bool isEliminateError;
        private bool isArrived;
        
        
        private float time;
        private float moveTime;
        private float totalTime;

        public GameObject DefenseMissEffect;
        public Material Material;
        private Coroutine _coroutine;
        
        
        public GameObject LStraightMissEffect;
        public GameObject RStraightMissEffect;
        public GameObject LSwingMissEffect;
        public GameObject RSwingMissEffect;
        public GameObject LHookMissEffect;
        public GameObject RHookMissEffect;

        private void Awake()
        {
            rectTransform = transform as RectTransform;
        }

        public void Init(ActionModel actionModel ,BeltItem parent)
        {
            isLife = true;
            isPlayMiss = false;
            isShow = false;
            isEliminateError = false;
            isArrived = false;
            
            this.parent = parent;
            this.actionModel = actionModel;
            time = 0;
            totalTime = 0;
            moveTime = GameVariable.beatTime - GameVariable.StopTime;
            perfectPrefab.GetComponent<Animator>().Rebind();
            icon.gameObject.SetActive(true);
            InitIcon();
            /*if (actionModel.acitonLocat == AcitonLocat.Right)
            {
                icon.transform.localRotation = Quaternion.Euler(new Vector3(0,180,0));
            }
            else
            {
                icon.transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));
            }*/
            OffeMove();
            WaitTimeMgr.CancelWait(ref _coroutine);
            _coroutine = WaitTimeMgr.WaitTime(GameVariable.BirthToClickTime + actionModel.offeTime, DoArrived);
        }

        void InitIcon()
        {
            SpriteModel spriteModel = SpritesMgr.Get(actionModel.Icon);
            icon.sprite = spriteModel.Sprite;
            Vector3 scale = spriteModel.IsOverturn ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
            icon.rectTransform.localScale = scale;
        }

        void OffeMove()
        {
            float a = (GameVariable.BirthToClickTime + actionModel.offeTime) / GameVariable.beatTime;
            float b = a - (int) a;
            while (b < 0)
            {
                b += GameVariable.beatTime;
            }
            time = -b * GameVariable.beatTime;
            rectTransform.DOLocalMove(
                new Vector3(rectTransform.localPosition.x,
                    rectTransform.localPosition.y + actionModel.speed * b * GameVariable.beatTime, rectTransform.localPosition.z),
                b * GameVariable.beatTime);
        }

        void Move()
        {
            time -= GameVariable.beatTime;
            float x = rectTransform.localPosition.y + actionModel.speed * GameVariable.beatTime - GameVariable.CenterY;
            if (Mathf.Abs(x) < GameVariable.Radius / 2)
            {
                rectTransform.DOLocalMove(
                    new Vector3(rectTransform.localPosition.x,
                        GameVariable.CenterY, rectTransform.localPosition.z),
                    moveTime);
                return;
            }
            rectTransform.DOLocalMove(
                new Vector3(rectTransform.localPosition.x,
                    rectTransform.localPosition.y + actionModel.speed * GameVariable.beatTime, rectTransform.localPosition.z),
                moveTime);
        }

        public void SetLife(bool isLife)
        {
            this.isLife = isLife;
        }

        IEnumerator Play(GradeType gradeType ,GameObject missGO)
        {
            yield return new WaitForSeconds(0.2f);
            missGO.SetActive(false);
            switch (gradeType)
            {
                case GradeType.Perfect:
                    perfectPrefab.SetActive(true);
                    //perfectPrefab.GetComponent<Animator>().Rebind();
                    break;
                case GradeType.VeryGood:
                    veryGoodPrefab.SetActive(true);
                    //veryGoodPrefab.GetComponent<Animator>().Rebind();
                    break;
                case GradeType.Good:
                    goodPrefab.SetActive(true);
                    //goodPrefab.GetComponent<Animator>().Rebind();
                    break;
                case GradeType.Correct:
                    correctPrefab.SetActive(true);
                    //correctPrefab.GetComponent<Animator>().Rebind();
                    break;
                case GradeType.Miss:
                    //missPrefab.SetActive(true);
                    //missPrefab.GetComponent<Animator>().Rebind();
                    break;
            }
        }

        //得到消散特效
        /*GameObject GetMissEffect()
        {
            switch (actionModel.actionType)
            {
                case ActionType.Defense:
                    return DefenseMissEffect;
                case ActionType.Straight:
                    if (actionModel.isLeft)
                        return LStraightMissEffect;
                    else
                        return RStraightMissEffect;
                case ActionType.Hook:
                    if (actionModel.isLeft)
                        return LHookMissEffect;
                    else
                        return RHookMissEffect;
                case ActionType.Swing:
                    if (actionModel.isLeft)
                        return LSwingMissEffect;
                    else
                        return RSwingMissEffect;
                default:
                    return DefenseMissEffect;
            }
        }*/

        public void PlayGrade(GradeType gradeType)
        {
            if(gradeType == GradeType.Miss)
                return;
            Shock(gradeType);
            //GameObject missGO = GetMissEffect();
            GameObject missGO = DefenseMissEffect;
            missGO.SetActive(true);
            Material.mainTexture = icon.sprite.texture;
            missGO.transform.localScale = new Vector3(icon.transform.localScale.x ,1);
            missGO.GetComponent<ParticleSystem>().Play();
            icon.gameObject.SetActive(false);
            Global.instance.StartCoroutine(Play(gradeType ,missGO));
        }

        Texture2D SpriteToTexture2D(Sprite sprite)
        {
            var targetTex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            var pixels = sprite.texture.GetPixels(
                (int)sprite.textureRect.x,
                (int)sprite.textureRect.y,
                (int)sprite.textureRect.width,
                (int)sprite.textureRect.height);
            targetTex.SetPixels(pixels);
            targetTex.Apply();
            return targetTex;
        }

        void Shock(GradeType gradeType)
        {
            float low_freq = 100;
            float high_freq = 200;
            float amp = 0.6f;
            int time = 0;
            switch (gradeType)
            {
                case GradeType.Perfect:
                    low_freq = 180;
                    high_freq = 360;
                    time = 200;
                    break;
                case GradeType.VeryGood:
                    low_freq = 160;
                    high_freq = 320;
                    time = 180;
                    break;
                case GradeType.Good:
                    low_freq = 120;
                    high_freq = 240;
                    time = 160;
                    break;
                case GradeType.Correct:
                    low_freq = 80;
                    high_freq = 160;
                    time = 140;
                    break;
            }
            /*if (parent.isLeft)
                LeftJoyconMgr.instance.SetRumble(low_freq, high_freq, amp, time);
            else
                RightJoyconMgr.instance.SetRumble(low_freq, high_freq, amp, time);*/
        }

        /// <summary>
        /// 执行移动
        /// </summary>
        public void DoMove()
        {
            if(!isLife)
                return;
            time += Time.deltaTime;
            totalTime += Time.deltaTime;
            EliminateError();
            if(time > GameVariable.StopTime)
                Move();
            if (rectTransform.localPosition.y > GameVariable.CenterY - 10 + GameVariable.Radius && isPlayMiss == false)
            {
                isPlayMiss = true;
                parent.PlayMiss();
                EventCenter.ins.EventTrigger("ActionMiss",this);
            }
            //打印误差
            /*if (rectTransform.localPosition.y > GameVariable.CenterY - 10 && isShow == false)
            {
                isShow = true;
                Debug.Log(totalTime);
                int b = (int) (MainGameCtr.instance.gameTime * 1000 -
                               (rectTransform.localPosition.y - GameVariable.CenterY) / GameVariable.speed);
                //Debug.Log(b);
                TDActionFlow c = ExcelMgr.TDActionFlow.Get(0);

                Debug.Log(c.delay + (actionModel.step-1)*c.beatTime - b);
            }*/
            if (rectTransform.localPosition.y > 700)
            {
                OnMoveOut();
            }
        }

        /// <summary>
        /// 初始化下一帧执行，消除误差
        /// </summary>
        void EliminateError()
        {
            if(isEliminateError)
                return;
            /*float gameTime = MainGameCtr.instance.gameTime * 1000;
            TDActionFlow data = ExcelMgr.TDActionFlow.Get(0);
            float selfTime = data.delay + (actionModel.step-1)*data.beatTime - GameVariable.BirthToClickTime* 1000 + totalTime*1000;
            float errorValue = (gameTime - selfTime)/1000;
            time += errorValue;
            totalTime += errorValue;*/
            isEliminateError = true;
        }

        /// <summary>
        /// 当移出屏幕时
        /// </summary>
        void OnMoveOut()
        {
            rectTransform.DOKill();
            parent.ActionItemMoveOut(this);
        }

        /// <summary>
        /// 当图标到达判定点时
        /// </summary>
        void DoArrived()
        {
            isArrived = true;
            if (GameVariable.Auto)
            {
                KeyCode keyCode = parent.isLeft ? KeyCode.Q : KeyCode.E;
                EventCenter.ins.EventTrigger("KeyDown", keyCode);
            }
        }

        public void OnDie()
        {
             perfectPrefab.SetActive(false);
             veryGoodPrefab.SetActive(false);
             goodPrefab.SetActive(false);
             correctPrefab.SetActive(false);
        }
    }
}