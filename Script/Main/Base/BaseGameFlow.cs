using System;
using System.Collections.Generic;
using System.Threading;
using Events;
using Script.Mgr;
using UnityEngine;

namespace Script.Main.Base
{
    public class BaseGameFlow : MonoBehaviour
    {
        protected bool IsEnable = false;
        private void Awake()
        {
            DoAwake();
        }

        private void OnEnable()
        {
            if(IsEnable)
                return;
            AddListener();
            IsEnable = true;
            DoEnable();
        }

        private void OnDisable()
        {
            if(!IsEnable)
                return;
            RemoveListener();
            IsEnable = false;
            DoDisable();
            //DoDestroy();
        }

        /// <summary>
        /// 添加生命周期监听器
        /// </summary>
        void AddListener()
        {
            string key = this.GetType().FullName;
            if (GameFlowMgr.Dictionary.ContainsKey(key))
            {
                GameFlowMgr.AddRemoveList(key);
            }
            GameFlowMgr.AddToAddList(key ,this);
        }
        
        /// <summary>
        /// 移除生命周期监听器
        /// </summary>
        void RemoveListener()
        {
            string key = this.GetType().FullName;
            if(!GameFlowMgr.Dictionary.ContainsKey(key))
                return;
            GameFlowMgr.AddRemoveList(key);
        }

        /// <summary>
        /// 由GameFlowEvent调用
        /// </summary>
        public void InvokeEvent(string EventName)
        {
            if(gameObject == null || !gameObject.activeInHierarchy)
                return;
            switch (EventName)
            {
                case "EnterGame":
                    EnterGame();
                    break;
                case "LoadedInitData":
                    LoadedInitData();
                    break;
                case "StartNewScene":
                    StartNewScene();
                    break;
                case "UnLoadScene":
                    UnLoadScene();
                    break;
                case "UnLoadData":
                    UnLoadData();
                    break;
                case "UnLoadedScene":
                    UnLoadedScene();
                    break;
                case "LoadScene":
                    LoadScene();
                    break;
                case "LoadPrepareData70Per":
                    LoadPrepareData70Per();
                    break;
                case "LoadPrepareData80Per":
                    LoadPrepareData80Per();
                    break;
                case "LoadPrepareData90Per":
                    LoadPrepareData90Per();
                    break;
                case "LoadedScene":
                    LoadedScene();
                    break;
                case "EnterNewScene":
                    EnterNewScene();
                    break;
            }
        }

        private void OnDestroy()
        {
            RemoveListener();
            DoDestroy();
        }

        /*private void OnEnable()
        {
            DoEnable();
        }

        private void OnDisable()
        {
            DoDisable();
        }*/

        private void Update()
        {
            DoUpdata();
        }

        protected virtual void DoAwake()
        {
            
        }

        protected virtual void DoEnable()
        {
            
        }
        
        protected virtual void DoDisable()
        {
            ;
        }
        
        protected virtual void DoDestroy()
        {
            ;
        }
        
        protected virtual void DoUpdata()
        {
            ;
        }
        
        //进入游戏,准备加载初始数据
        protected virtual void EnterGame()
        {
            
        }
        //初始数据加载完成
        protected virtual void LoadedInitData()
        {
            
        }
        
        
        //准备开始新的场景，播放loding开始动画
        protected virtual void StartNewScene()
        {
            
        }
        //准备卸载场景
        protected virtual void UnLoadScene()
        {
            
        }
        //准备卸载场景数据
        protected virtual void UnLoadData()
        {
            
        }
        //卸载完场景
        protected virtual void UnLoadedScene()
        {
            
        }
        //准备加载场景
        protected virtual void LoadScene()
        {
            
        }
        //准备加载场景数据
        protected virtual void LoadPrepareData70Per()
        {
            
        }
        //准备加载场景数据
        protected virtual void LoadPrepareData80Per()
        {
            
        }
        //准备加载场景数据
        protected virtual void LoadPrepareData90Per()
        {
            
        }
        //加载完场景，播放Loding完成动画
        protected virtual void LoadedScene()
        {
            
        }
        //准备开始游戏
        protected virtual void EnterNewScene()
        {
            
        }
    }
}