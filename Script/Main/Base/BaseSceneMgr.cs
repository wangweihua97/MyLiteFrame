using System.Collections;
using System.Collections.Generic;
using Events;
using Script.Lua.Mgr;
using Script.Mgr;
using Script.Scene.Base;
using UI.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Script.Main.Base
{
    public class BaseSceneMgr : BaseGameFlow
    {
        protected IScene curScene;
        SceneInstance curLoadedScene;
        Dictionary<string,IScene> sceneMap;
        private AsyncOperationHandle sceneHandle;
        protected string sceneKey;
        protected BaseLoadingUI loadingUi;
        
        public bool IsLuaScene = false;
         
        protected override void DoAwake()
        {
            base.DoAwake();
            sceneMap = new Dictionary<string, IScene>();
        }

        void DoStartScene(string sceneKey, BaseLoadingUI loadingUi)
        {
            AsyncOperationHandle<SceneInstance> handle = AddressablesHelper.instance.LoadSceneAsync(sceneKey);
            GameFlowTask gameFlowTask = FlowTaskFactory.CreatTask(() =>
            {
                if (handle.IsValid())
                    return handle.PercentComplete;
                else
                    return 1;
            });
            handle.Completed += obj =>
            {
                sceneHandle = obj;
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    curLoadedScene = obj.Result;
                    SceneManager.SetActiveScene(curLoadedScene.Scene);
                    gameFlowTask.Completed.Invoke();
                }
            };
            GameFlowMgr.LoadScene.AddTask(gameFlowTask);
        }
        
        void DoUnloadScene()
        {
            if (curScene == null)
            {
                return;
            }
            AsyncOperationHandle<SceneInstance> handle = AddressablesHelper.instance.UnloadSceneAsync(curLoadedScene);
            GameFlowTask gameFlowTask = FlowTaskFactory.CreatTask(() =>
            {
                if (handle.IsValid())
                    return handle.PercentComplete;
                else
                    return 1;
            });
            handle.Completed += obj =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    gameFlowTask.Completed.Invoke();
                }
            };
            GameFlowMgr.UnLoadScene.AddTask(gameFlowTask);
        }

        void SetCurScene()
        {
            if(IsLuaScene)
                curScene = LuaSceneMgr.GetScene(sceneKey);
            else
                curScene = sceneMap[sceneKey];
        }

        #region 受保护方法
        protected bool ContainsKey(string key)
        {
            return sceneMap.ContainsKey(key);
        }
        
        protected IScene GetScene(string key)
        {
            return sceneMap[key];
        }
        #endregion
        
        #region 公共方法
        public void StartScene(string sceneKey, BaseLoadingUI loadingUi)
        {
            if (!sceneMap.ContainsKey(sceneKey))
            {
                Debug.LogError("找不到" + sceneKey);
                return;
            }

            IsLuaScene = false;
            this.sceneKey = sceneKey;
            this.loadingUi = loadingUi;
            if(loadingUi != null)
                loadingUi.Show(true);
            GameFlowMgr.StartNewScene.Invoke();
        }

        #region Lua测试
        public void StartLuaScene(string sceneKey, BaseLoadingUI loadingUi)
        {
            IsLuaScene = true;
            this.sceneKey = sceneKey;
            this.loadingUi = loadingUi;
            if(loadingUi != null)
                loadingUi.Show(true);
            GameFlowMgr.StartNewScene.Invoke();
        }
        #endregion
        

        public void AddScene<T>(T scene, string sceneKey) where T : IScene
        {
            sceneMap.Add(sceneKey ,scene);
        }

        public string GetCurSceneName()
        {
            return curScene == null ? "" : curScene.Name;
        }
        #endregion

        #region GameFlow流程
        protected override void DoUpdata()
        {
            base.DoUpdata();
            curScene?.OnUpdate();
        }
        
        //进入游戏,准备加载初始数据
        protected override void EnterGame()
        {
            base.EnterGame();
        }
        //初始数据加载完成
        protected override void LoadedInitData()
        {
            base.LoadedInitData();
        }
        
        
        //准备卸载场景
        protected override void UnLoadScene()
        {
            base.UnLoadScene();
            curScene?.OnBattleUnLoad();
            DoUnloadScene();
        }
        //卸载场景中的数据
        protected override void UnLoadData()
        {
            base.UnLoadData();
            curScene?.OnUnLoadData();
            if (sceneHandle.IsValid())
            {
                AddressablesHelper.instance.Release(sceneHandle);
            }
        }
        //卸载完场景
        protected override void UnLoadedScene()
        {
            base.UnLoadedScene();
            curScene?.OnBattleUnLoaded();
        }
        //准备加载场景数据
        protected override void LoadPrepareData70Per()
        {
            base.LoadPrepareData70Per();
            curScene?.LoadPrepareData70Per();
        }
        //准备加载场景数据
        protected override void LoadPrepareData80Per()
        {
            base.LoadPrepareData80Per();
            curScene?.LoadPrepareData80Per();
        }
        //准备加载场景数据
        protected override void LoadPrepareData90Per()
        {
            base.LoadPrepareData90Per();
            curScene?.LoadPrepareData90Per();
        }
        //加载完场景，播放Loding完成动画
        protected override void LoadedScene()
        {
            base.LoadedScene();
            curScene?.OnBattleLoaded();
        }
        //准备开始游戏
        protected override void EnterNewScene()
        {
            base.EnterNewScene();
            curScene?.EnterNewScene();
        }
        protected override void LoadScene()
        {
            base.LoadScene();
            SetCurScene();
            DoStartScene(sceneKey, loadingUi);
        }


        #endregion
        
    }
}