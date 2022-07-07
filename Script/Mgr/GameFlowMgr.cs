using System;
using System.Collections.Generic;
using Events;
using Script.Main;
using Script.Main.Base;
using UnityEngine;
using UnityEngine.Events;

namespace Script.Mgr
{
    public class GameFlowMgr : BaseGameFlowMgr
    {
        //进入游戏,准备加载初始数据
        public static GameFlowEvent EnterGame = FlowEventFactory.CreatEvent("EnterGame" ,() =>
        {
            LoadedInitData.Invoke();
        },50);
        //初始数据加载完成
        public static GameFlowEvent LoadedInitData= FlowEventFactory.CreatEvent("LoadedInitData" ,() =>
        {
            //Global.instance.LoadFirstScene();
        },50);
        
        
        //准备开始新的场景，播放loding开始动画
        public static GameFlowEvent StartNewScene= FlowEventFactory.CreatEvent("StartNewScene" ,() =>
        {
            UnLoadScene.Invoke();
        }, 0);
        //准备卸载场景
        public static GameFlowEvent UnLoadScene= FlowEventFactory.CreatEvent("UnLoadScene" ,() =>
        {
            UnLoadData.Invoke();
        }, 20);
        //卸载完场景
        public static GameFlowEvent UnLoadData= FlowEventFactory.CreatEvent("UnLoadData" ,() =>
        {
            UnLoadedScene.Invoke();
        }, 5);
        //卸载完场景
        public static GameFlowEvent UnLoadedScene= FlowEventFactory.CreatEvent("UnLoadedScene" ,() =>
        {
            GC.Collect();
            LoadScene.Invoke();
        }, 5);
        //准备加载场景
        public static GameFlowEvent LoadScene= FlowEventFactory.CreatEvent("LoadScene" ,() =>
        {
            LoadPrepareData70Per.Invoke();
        }, 40);
        //准备加载场景数据
        public static GameFlowEvent LoadPrepareData70Per= FlowEventFactory.CreatEvent("LoadPrepareData70Per" ,() =>
        {
            LoadPrepareData80Per.Invoke();
        }, 10);
        //准备加载场景数据
        public static GameFlowEvent LoadPrepareData80Per= FlowEventFactory.CreatEvent("LoadPrepareData80Per" ,() =>
        {
            LoadPrepareData90Per.Invoke();
        }, 10);
        //准备加载场景数据
        public static GameFlowEvent LoadPrepareData90Per= FlowEventFactory.CreatEvent("LoadPrepareData90Per" ,() =>
        {
            LoadedScene.Invoke();
        }, 10);
        //加载完场景，播放Loding完成动画
        public static GameFlowEvent LoadedScene= FlowEventFactory.CreatEvent("LoadedScene" ,() =>
        {
            EnterNewScene.Invoke();
        }, 0);
        //准备开始游戏
        public static GameFlowEvent EnterNewScene= FlowEventFactory.CreatEvent("EnterNewScene" ,() =>
        {
        }, 0);
    }
}