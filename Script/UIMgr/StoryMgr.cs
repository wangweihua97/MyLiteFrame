using System;
using System.Collections.Generic;
using Events;
using Player;
using Script.Excel.Table;
using Script.Mgr;
using Script.Model;
using Script.Tool;
using SimpleJSON;
using UI.story;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Script.Main
{
    public class StoryMgr : UUIMgr
    {
        
        public static StoryView StoryView;
        public override string Name => "StoryMgr";
        public override string sortingLayerName => "CommonView";
        
        public override void DoCreat()
        {
            base.DoCreat();
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            Add<StoryView>("StoryView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("StoryView", "UIView"));
            gameFlowTaskGroup.Attach(GameFlowMgr.LoadPrepareData90Per);
        }
        public override void DoDestroy()
        {
            base.DoDestroy();
        }

        public static int StoryCfgID = 1;

        public static void initStoryData()
        {
            StoryCfgID = Mathf.Max(1, NativeStoreTool.GetInt("StoryCfgID"));
            ckRollBack();
        }

        private static Action<List<string>> json_cb;
        static List<string> modelUrlList;
        private static int jsonIdx;
        private static int loadNum;
        /**本次对话需要的模型*/
        public static void ModelUrlList(Action<List<string>> callBack)
        {
            modelUrlList = new List<string>();
            jsonIdx = 0;
            loadNum = 0;
            json_cb = callBack;
            // List<string> list = new List<string>();
            // list.Add("M_Player");
            TDStory cfg = ExcelMgr.TDStory.Get("" + StoryCfgID);
            Json(cfg.story1);
            Json(cfg.story2);
            Json(cfg.story3);
            Json(cfg.story4);
            //没有加载直接回调
            if (jsonIdx>=loadNum)
            {
                Action<List<string>> tmpCb = json_cb;
                tmpCb(modelUrlList);
            }
        }

        static void Json(string jsonID)
        {
            if (!string.IsNullOrEmpty(jsonID))
            {
                loadNum++;
                string url = "CsvSD/" + jsonID + ".json";
                if (AddressablesHelper.instance.ContainsKey(url) && AddressablesHelper.instance.GetHandle(url).IsValid())
                {
                    AsyncOperationHandle<IList<TextAsset>> handle = AddressablesHelper.instance.GetHandle(url).Convert<IList<TextAsset>>();
                    if (null == handle.Result)
                    {
                        handle.Completed += obj =>
                        {
                            if (obj.Status == AsyncOperationStatus.Succeeded)
                            {
                                LoadJsonStep(obj.Result[0]);
                            }
                        };
                    }
                    else
                    {
                        LoadJsonStep(handle.Result[0]);
                    }
                }
                else
                {
                    AsyncOperationHandle<IList<TextAsset>> handle = AddressablesHelper.instance.LoadAssetsAsync<TextAsset>(url, "");
                    handle.Completed += obj =>
                    {
                        if (obj.Status == AsyncOperationStatus.Succeeded)
                        {
                            LoadJsonStep(obj.Result[0]);
                        }
                    };
                }
            }
        }

        static void LoadJsonStep(TextAsset ta)
        {
            jsonIdx++;
            JSONNode json = JSON.Parse(ta.text);
            int len = json.Count;
            string str;
            TDNPC table;
            string _url;
            for (int i = 0; i < len; i++)
            {
                str = json[i]["npcId"];
                if (!string.IsNullOrEmpty(str)&& -1 == modelUrlList.IndexOf(str))
                {
                    table = ExcelMgr.TDNPC.Get(str);
                    _url = table.map;
                    modelUrlList.Add(_url == "M_Player" ? _url : "MonsterCharacter/" + _url + ".prefab");
                }
            }
            //
            if (jsonIdx>=loadNum)
            {
                Action<List<string>> tmpCb = json_cb;
                tmpCb(modelUrlList);
            }
        }

        public static void SetNextLevel()
        {
            StoryCfgID++;
            SaveStoryCfgID();
        }

        public static void SaveStoryCfgID()
        {
            NativeStoreTool.Set("StoryCfgID", StoryCfgID);
        }

        public static bool CheckStoryOnly(CheckStoryStep type)
        {
            return false;
            if (ExcelMgr.TDStory.ContainsKey("" + StoryCfgID))
            {
                string jsonUrl = null;
                TDStory cfg = ExcelMgr.TDStory.Get("" + StoryCfgID);
                switch (type)
                {
                    case CheckStoryStep.SelectLevel:
                        jsonUrl = cfg.story1;
                        break;
                    case CheckStoryStep.BeforeBattle:
                        jsonUrl = cfg.story2;
                        break;
                    case CheckStoryStep.MoveBattle:
                        jsonUrl = cfg.story3;
                        break;
                    case CheckStoryStep.AfterBattle:
                        jsonUrl = cfg.story4;
                        break;
                }
                //
                if (cfg.level - 1 == PlayerInfo.Instance.PlayerData.PassMaxLevel && !string.IsNullOrEmpty(jsonUrl))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private static bool addFlag = false;

        public delegate void ckCB(bool haveStory);
        private static ckCB _cb;
        /**
         * 检测是否有对话
         * CheckStoryStep
         * cb=回调
        */
        public static void CheckStory(CheckStoryStep type, ckCB cb)
        {
            cb?.Invoke(false);
            return;
            addFlag = false;
            _cb = cb;
            if (ExcelMgr.TDStory.ContainsKey("" + StoryCfgID))
            {
                string jsonUrl = null;
                TDStory cfg = ExcelMgr.TDStory.Get("" + StoryCfgID);
                switch (type)
                {
                    case CheckStoryStep.SelectLevel:
                        jsonUrl = cfg.story1;
                        break;
                    case CheckStoryStep.BeforeBattle:
                        jsonUrl = cfg.story2;
                        break;
                    case CheckStoryStep.MoveBattle:
                        jsonUrl = cfg.story3;
                        break;
                    case CheckStoryStep.AfterBattle:
                        jsonUrl = cfg.story4;
                        addFlag = true;
                        break;
                }
                //
                if (cfg.level - 1 == PlayerInfo.Instance.PlayerData.PassMaxLevel && !string.IsNullOrEmpty(jsonUrl))
                {
                    StoryView.SetDataAndOpen(jsonUrl);
                }
                else
                {
                    cbExe();
                }
            }
            else
            {
                cbExe();
            }
            //
        }

        static void cbExe()
        {
            if (null!=_cb)
            {
                StoryCfgIDAdd();
                ckCB tmpCB = _cb;
                tmpCB(false);
            }
        }

        static void StoryCfgIDAdd()
        {
            if (addFlag) StoryCfgID++;
            addFlag = false;
        }

        public static void DialogComplete()
        {
            if (null!=_cb)
            {
                StoryCfgIDAdd();
                ckCB tmpCB = _cb;
                tmpCB(true);
            }
        }

        /**回滚到上一个非0节点*/
        static void ckRollBack()
        {
            TDStory cfg = ExcelMgr.TDStory.Get("" + StoryCfgID);
            while (0 == cfg.level)
            {
                StoryCfgID--;
                cfg = ExcelMgr.TDStory.Get("" + StoryCfgID);
            }
        }
    }
}