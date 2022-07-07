using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events;
using OtherMgr;
using Player;
using Script.Main;
using Script.Mgr;
using Script.Model;
using Script.Scene.Base;
using Script.Tool.PoolManager;
using UI.Main;
using UnityEngine;
using UnityEngine.U2D;

namespace Script.Scene
{
    public class HallScene : IScene
    {
        public string Name => "HallScene";
        public void OnUpdate()
        {
        }
        public void LoadPrepareData70Per()
        {
            EventCenter.ins.AddEventListener("createPlayer_complete",OnCreatePlayerComp);
            //Cams = GameObject.Find("Cams").GetComponent<HallSceneHelper>();
            EventCenter.ins.AddEventListener("start_move_login_camera",OnStartMoveCamera);
            EventCenter.ins.AddEventListener("set_camera_act",OnSetCameraAct);
            EventCenter.ins.AddEventListener("set_camera_act_to_complete",OnToComplete);
            
            EventCenter.ins.EventTrigger("CameraInitLocations");

            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            
            SpritesMgr.AddSprites("PunchIcon" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            SpritesMgr.AddSprites("CharacterIcon" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            SpritesMgr.AddSpriteAtlas("TrainIcon" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            SpritesMgr.AddSpriteAtlas("SceneIcon" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            SpritesMgr.AddSprites("LvIcon" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            gameFlowTaskGroup.Attach(GameFlowMgr.LoadPrepareData70Per);
            LoadPoolPrefab();
        }
        
        public void LoadPrepareData80Per()
        {
            RootUIMgr.instance.CreatUIMgr<HallViewMgr>(true);
            RootUIMgr.instance.CreatUIMgr<TrainViewMgr>();
            RootUIMgr.instance.CreatUIMgr<CharacterUIMgr>();
            if(!RootUIMgr.instance.ContainsUIMgr<CommonUIMgr>())
                RootUIMgr.instance.CreatUIMgr<CommonUIMgr>();
            
        }

        public void LoadPrepareData90Per()
        {
            //有无创建角色-加载创建块
            if (!PlayerInfo.Instance.HavePlayerInfo())//test
            {
                RootUIMgr.instance.CreatUIMgr<CreatePlayerViewMgr>(true);
            }

            /*//对话提前加载.需要加载模型
            StoryMgr.initStoryData();
            //
            LoadDialogAct();*/
        }

        void LoadDialogAct()
        {
            // List<string> charactersPath = 
            StoryMgr.ModelUrlList(urlList =>
            {
                if (urlList.Count>0)
                {
                    PoolManager.CharacterPool.poolCreatMultiGoHelper.AddGameObject(urlList,GameFlowMgr.LoadPrepareData90Per);
                }
                else
                {
                    // GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
                    // gameFlowTaskGroup.Attach(GameFlowMgr.LoadPrepareData90Per);
                    FlowTaskFactory.CreatTaskGroup().Attach(GameFlowMgr.LoadPrepareData90Per);
                }
            });
        }

        public void OnBattleLoaded()
        {
        }

        /**加载完成时*/
        public void EnterNewScene()
        {
            //有无创建角色-是否打开
            if (PlayerInfo.Instance.HavePlayerInfo())//
            {
                LoadData();
                //镜头走主界面
                HallViewMgr.MainView.CameraMoveBTypeOnce = false;
                HallViewMgr.MainView.DoOpen();
            }
            else
            {
                //场景环境设置
                EventCenter.ins.EventTrigger("bcfMgr_createPlayer");
                //镜头走创建界面
                EventCenter.ins.EventTrigger("CameraMoveStraightaway" ,ExcelMgr.TDCameraLocation.Get("SelectSexView").location);
                //延迟显示创角界面
                Global.instance.StartCoroutine(delayShowCreatePlayerView(0.5f));
            }
            
        }

        /**加载数据*/
        void LoadData()
        {
            //背包
            PlayerBag.initBagData();
            
        }
        
        private IEnumerator delayShowCreatePlayerView(float time)
        {
            yield return new WaitForSeconds(time);
            EventCenter.ins.EventTrigger("create_player_step0");
        }
        
        
        public void OnBattleUnLoad()
        {
        }

        public void OnUnLoadData()
        {
            RootUIMgr.instance.DestroyUIMgr<HallViewMgr>();
            RootUIMgr.instance.DestroyUIMgr<TrainViewMgr>();
            RootUIMgr.instance.DestroyUIMgr<CharacterUIMgr>();
            SpritesMgr.RemoveSpriteAtlas("SceneIcon");
            // SpritesMgr.RemoveSpriteAtlas("CharacterIcon");
            if(RootUIMgr.instance.ContainsUIMgr<CreatePlayerViewMgr>())
                RootUIMgr.instance.DestroyUIMgr<CreatePlayerViewMgr>();
        }

        public void OnBattleUnLoaded()
        {
            EventCenter.ins.RemoveEventListener("start_move_login_camera",OnStartMoveCamera);
            EventCenter.ins.RemoveEventListener("set_camera_act",OnSetCameraAct);
            EventCenter.ins.RemoveEventListener("createPlayer_complete",OnCreatePlayerComp);
        }

        void OnCreatePlayerComp()
        {
            // EventCenter.ins.AddEventListener("bcfMgr_playAni_complete",OnBcfMgr_playAni_complete);
            // EventCenter.ins.EventTrigger("bcfMgr_playAni");
            OnBcfMgr_playAni_complete();
        }

        void OnBcfMgr_playAni_complete()
        {
            LoadData();
            // EventCenter.ins.RemoveEventListener("bcfMgr_playAni_complete",OnBcfMgr_playAni_complete);
            //镜头
            HallViewMgr.MainView.CameraMoveBTypeOnce = true;
            //界面
            HallViewMgr.MainView.DoOpen();
        }
        
        
        private void OnToComplete()
        {
            //Cams.ToComplete();
        }

        private void OnSetCameraAct()
        {
            //Cams.SetCameraAct(true);
        }
        
        public void OnStartMoveCamera()
        {
            //Cams.Play();
            Global.instance.StartCoroutine(EnterStep1(1f));
        }
        /**logoTxt出现*/
        private IEnumerator EnterStep1(float time)
        {
            yield return new WaitForSeconds(time);
            EventCenter.ins.EventTrigger("move_camera_complete");//发送完成
        }
        
        void LoadPoolPrefab()
        {
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            PoolManager.CommonPool.CreatPool("Prefab/IconItem","", obj =>
            {
                gameFlowTaskGroup.CompleteATask();
            });
            SubObjPool iconItemPool = PoolManager.CommonPool.GetPool("Prefab/IconItem") as SubObjPool;
            gameFlowTaskGroup.Add(iconItemPool.Handle);
            PoolManager.CommonPool.CreatPool("Prefab/MapLine","", obj =>
            {
                gameFlowTaskGroup.CompleteATask();
            });
            SubObjPool mapLinePool = PoolManager.CommonPool.GetPool("Prefab/MapLine") as SubObjPool;
            gameFlowTaskGroup.Add(mapLinePool.Handle);
            gameFlowTaskGroup.Attach(GameFlowMgr.LoadPrepareData70Per);
        }
    }
}