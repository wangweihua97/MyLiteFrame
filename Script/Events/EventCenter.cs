using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Events 
{
    public class EventCenter : MonoBehaviour
    {
        #region 公共静态成员

        public static EventCenter ins;

        #endregion
        
        #region 私有成员
        // key对应事件的名字，value对应的是监听这个事件对应的委托函数【们】
        private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();
        #endregion

        #region 公共方法
        
        // 添加事件监听，两个个参数的
        public void AddEventListener<T0,T1>(string name, UnityAction<T0,T1> action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo2<T0,T1>).actions += action;
            else
                eventDic.Add(name, new EventInfo2<T0,T1>(action));
        }
        
        // 添加事件监听，一个参数的
        public void AddEventListener<T>(string name, UnityAction<T> action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo<T>).actions += action;
            else
                eventDic.Add(name, new EventInfo<T>(action));
        }

        // 添加事件监听，无参数的
        public void AddEventListener(string name, UnityAction action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo).actions += action;
            else
                eventDic.Add(name, new EventInfo(action));
        }

        // 事件触发，无参的
        public void EventTrigger(string name)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo).actions?.Invoke();
        }

        //事件触发，一个参数的
        public void EventTrigger<T>(string name, T info)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo<T>).actions?.Invoke(info);
        }
        
        //事件触发，两个个参数的
        public void EventTrigger<T0,T1>(string name, T0 info ,T1 info2)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo2<T0,T1>).actions?.Invoke(info ,info2);
        }

        //移除监听，无参的
        public void RemoveEventListener(string name, UnityAction action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo).actions -= action;
        }

        //移除监听，一个参数的
        public void RemoveEventListener<T>(string name, UnityAction<T> action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo<T>).actions -= action;
        }
        
        //移除监听，两个个参数的
        public void RemoveEventListener<T0,T1>(string name, UnityAction<T0,T1> action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo2<T0,T1>).actions -= action;
        }

        public bool ContainsKey(string name)
        {
            return eventDic.ContainsKey(name);
        }

        // 清空事件中心，主要用在场景切换时
        public void Clear()
        {
            eventDic.Clear();
        }

        #endregion

        #region life

        void Awake()
        {
            ins = this;
        }
        #endregion

    }
}