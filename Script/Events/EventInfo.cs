using System;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem;
using UnityEngine.Events;

namespace Events
{
    
    public interface IEventInfo { }
    // 实现两个个参数
    public class EventInfo2<T0,T1> : IEventInfo
    {
        public UnityAction<T0,T1> actions;
        public EventInfo2(UnityAction<T0 ,T1> action)
        {
            actions += action;
        }
    }
    // 实现一个参数
    public class EventInfo<T> : IEventInfo
    {
        public UnityAction<T> actions;
        public EventInfo(UnityAction<T> action)
        {
            actions += action;
        }
    }
    // 实现无参
    public class EventInfo : IEventInfo
    {
        public UnityAction actions;
        public EventInfo(UnityAction action)
        {
            actions += action;
        }
    }
}
