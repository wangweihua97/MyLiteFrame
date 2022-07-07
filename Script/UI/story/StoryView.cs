﻿using System;
 using System.Collections;
 using System.Collections.Generic;
 using DG.Tweening;
 using Enemy;
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
 using SimpleJSON;
 using UI.Base;
 using UI.CreatePlayer.vo;
 using UnityEngine;
 using UnityEngine.AddressableAssets;
 using UnityEngine.ResourceManagement.AsyncOperations;
 using UnityEngine.UI;
 using Ease = EasingCore.Ease;

 namespace UI.story
{
    public class StoryView: InOutUView
    {
        // [Header("列表组件")]
        // public ScrollView list;
        // public LvInfoCtr lvInfoCtr;
        public RectTransform tipsPanel;
        public RectTransform tp_bg;
        public Text tp_txt;
        public RectTransform tp_A;
        public RectTransform tp_go;
        public GameObject bottomPanel;
        public Text bp_name;
        public Text bp_txt;
        public GameObject voicePanel;
        public Text vp_name;
        public Text vp_txt;

        public RawImage head;

        private GameObject headHan;
        private string headKey;

        private JSONNode json;
        private int dialogIndex;
        private int dialogLen;

        private bool _isDelayHand = false;

        private Vector2 arrowOffset = new Vector2();

        public override bool DefaultShow => false;

        public override void DoCreat()
        {
            base.DoCreat();
            StoryMgr.StoryView = this;
            // initData();
            // headArea.SetActive(false);
        }
        
        void initData()
        {
            
        }
        
        public override void DoOpen()
        {
            base.DoOpen();
        }

        public void SetDataAndOpen(string jsonID)
        {
            dialogIndex = 0;
            string url = "CsvSD/" + jsonID + ".json";
            //
            if (AddressablesHelper.instance.ContainsKey(url) && AddressablesHelper.instance.GetHandle(url).IsValid())
            {
                AsyncOperationHandle<IList<TextAsset>> handle = AddressablesHelper.instance.GetHandle(url).Convert<IList<TextAsset>>();
                analyzeJSON(handle.Result[0]);
                DoOpen();
            }
            else
            {
                AsyncOperationHandle<IList<TextAsset>> handle = AddressablesHelper.instance.LoadAssetsAsync<TextAsset>(url, "");
                handle.Completed += obj =>
                {
                    if (obj.Status == AsyncOperationStatus.Succeeded)
                    {
                        // TextAsset textAsset = obj.Result[0];
                        analyzeJSON(obj.Result[0]);
                        DoOpen();
                    }
                };
            }
        }

        void analyzeJSON(TextAsset ta)
        {
            json = JSON.Parse(ta.text);
            dialogLen = json.Count;
        }

        void updateDialog()
        {
            //{"panelType":1, "model":"","modelX":"","name":"角色C", "txt":"啊是的发给和就和3" }
            JSONNode dialog = json[dialogIndex];
            //
            tipsPanel.gameObject.SetActive(dialog["type"] == 2);//气泡框
            bottomPanel.gameObject.SetActive(dialog["type"] == 1);//底大框
            voicePanel.gameObject.SetActive(dialog["type"] == 3);//旁白底大框
            //
            if (dialog["type"] == 1)
            {
                //1=头像左边,2=头像右边
                int mx = dialog["modelX"];
                //头像
                loadHeadAct(dialog["npcId"]);
                bp_name.text = dialog["name"];
                bp_txt.text = dialog["text"];
            }else if (dialog["type"] == 2)
            {
                Vector3 A = new Vector3();
                Vector3 Go = new Vector3();
                Vector3 bgXY = new Vector3();
                Vector3 bgSize = new Vector2();
                Vector3 txtLp = new Vector3();
                Vector3 tpSize = new Vector2();
                tp_txt.text = dialog["text"];
                // _desc.text = StringTool.Formatting(_data.description);
                LayoutRebuilder.ForceRebuildLayoutImmediate(tp_txt.rectTransform);
                float txtH = tp_txt.rectTransform.sizeDelta.y;
                float bgH = txtH + 20;
                if (1 == dialog["modelX"])//箭头向左
                {
                    //tp_bg.SetLocalScaleX(1);
                    txtLp = new Vector3(0f,bgH*0.5f-10,0f);
                    bgXY = new Vector3(0f,bgH*0.5f,0f);
                    bgSize = new Vector2(460f,bgH);
                    A = new Vector3(120f, -(bgH * 0.5f + 35),0f);
                    Go = new Vector3(183f, -(bgH * 0.5f + 40),0f);
                    arrowOffset.x = -bgSize.x*0.5f;
                    arrowOffset.y = -bgH*0.5f+25;
                }
                else//箭头向右
                {
                    //tp_bg.SetLocalScaleX(-1);
                    txtLp = new Vector3(-10f,bgH*0.5f-10,0f);
                    bgXY = new Vector3(0f,bgH*0.5f,0f);
                    bgSize = new Vector2(460f,bgH);
                    A = new Vector3(-183f, -(bgH * 0.5f + 35),0f);
                    Go = new Vector3(-130f, -(bgH * 0.5f + 40),0f);
                    arrowOffset.x = bgSize.x*0.5f;
                    arrowOffset.y = -bgH*0.5f+25;
                }
                //
                tipsPanel.sizeDelta = bgSize;
                //
                tp_A.localPosition = A;
                tp_go.localPosition = Go;
                tp_bg.localPosition = bgXY;
                tp_bg.sizeDelta = bgSize;
                tp_txt.transform.localPosition = txtLp;
                //1=敌人,2=自己
                int mx = dialog["modelX"];
                //获取目标世界坐标
                Vector3 worldV3 = 1==mx?EnemyMgr.instance.transform.position:PlayerMgr.instance.transform.position;// = new Vector3();
                Vector3 localV3 = tipsPanel.parent.transform.InverseTransformPoint(worldV3);
                localV3.x += arrowOffset.x;//气泡框指向偏移
                localV3.y += arrowOffset.y;
                tipsPanel.localPosition = localV3;
            }else if (dialog["type"] == 3)
            {
                // bp_name.text = dialog["name"];--
                bp_txt.text = dialog["text"];
            }else if (dialog["type"] == 4)
            {
                if (!string.IsNullOrEmpty(dialog["soundBg"]))
                {
                    AudioManager.PlayBackground(dialog["soundBg"]);
                }
                if (!string.IsNullOrEmpty(dialog["sound"]))
                {
                    AudioManager.PlayAudioEffectA(dialog["sound"]);
                }
                if (null != dialog["delay"])
                {
                    dialogIndex++;
                    Global.instance.StartCoroutine(storyDelay(float.Parse(dialog["delay"])));
                    _isDelayHand = true;
                }
            }
        }
        
        private IEnumerator storyDelay(float time)
        {
            yield return new WaitForSeconds(time);
            _isDelayHand = false;
            storyNextExe();
        }

        void loadHeadAct(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                TDNPC table = ExcelMgr.TDNPC.Get(url);
                string _url = table.map;
                //回收3D模型
                recoverAct(headKey, headHan);
                //是否为人物
                bool isPlayer = _url == "M_Player";
                //人物+怪物
                string monsterPath = isPlayer? _url: "MonsterCharacter/" + _url + ".prefab";
                // Transform monsterTransform = PoolManager.CharacterPool.Spawn(monsterPath).transform;
                headHan = PoolManager.CharacterPool.Spawn(monsterPath, Global_headAreaMgr.instance.transform);
                if (isPlayer)
                {
                    headHan.transform.localPosition = new Vector3(0, -2.2f, 3);//人
                }
                else
                {
                    headHan.transform.localPosition =  new Vector3(0.1f, -2.1f, 4.6f);//怪物
                    headHan.GetComponentInChildren<EnemyMgr>().EnemyAnimationMgr.Play("FIdle");
                }
                //headHan.transform.SetLocalScaleZ(-1);
                //头像需要刷新texture
                head.texture = Global_headAreaMgr.instance.GetRT();
                // go.transform.SetParent(headArea_camera.transform);
                headKey = monsterPath;
            }
        }
        
        protected override void enterStageComplete()
        {
            EventCenter.ins.AddEventListener<KeyCode>("KeyDown",KeyDown);
            updateDialog();
        }
        
        /*private IEnumerator delayFun(float time)
        {
            yield return new WaitForSeconds(time);
        }*/

        void KeyDown(KeyCode keyCode)
        {
            switch (keyCode)
            {
                case KeyCode.W:
                    break;
                case KeyCode.A:
                    // AudioManager.PlayAudioEffectA("选中框移动");
                    break;
                case KeyCode.S:
                    break;
                case KeyCode.D:
                    // AudioManager.PlayAudioEffectA("选中框移动");
                    break;
                case KeyCode.J:
                    AudioManager.PlayAudioEffectA("选中确认");
                    if (!_isDelayHand)
                    {
                        dialogIndex++;
                        storyNextExe();
                    }
                    break;
                case KeyCode.K:
                    // AudioManager.PlayAudioEffectA("返回");
                    // DoClose();
                    break;
            }
        }

        void storyNextExe()
        {
            if (dialogIndex < dialogLen)
            {
                updateDialog();
            }
            else
            {
                recoverAct(headKey, headHan);
                AudioManager.StopPlayAudioBackGround();
                DoClose();
                StoryMgr.DialogComplete();
            }
        }

        public override void DoClose()
        {
            EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
            base.DoClose();
        }

        protected override void DoDestroy()
        {
            EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
            base.DoDestroy();
        }

        /**回收3D模型*/
        void recoverAct(string key, GameObject go)
        {
            if (null != go && !string.IsNullOrEmpty(key))
            {
                go.transform.SetParent(null);
                PoolManager.CharacterPool.DesSpawn(key, go);
            }
        }
        
    }
}