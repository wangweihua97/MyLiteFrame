using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace OtherMgr
{
    public class WaitTimeMgr
    {
        private static TaskBehaviour m_Task;

        static WaitTimeMgr()
        {
            GameObject go = new GameObject("WaitTimeMgr");
            GameObject.DontDestroyOnLoad(go);
            m_Task = go.AddComponent<TaskBehaviour>();
        }


        public static void CancelWait(ref Coroutine coroutine)
        {
            if (coroutine!=null)
            {
                m_Task.StopCoroutine(coroutine);
            }
        }

        public static Coroutine Schedule(float time, float interval, UnityAction callback)
        {
            return m_Task.StartCoroutine(StartSchedule(time, interval, callback));
        }

        public static Coroutine WaitTime(float time, UnityAction callback)
        {
            return m_Task.StartCoroutine(Coroutine(time, callback));
        }

        private static IEnumerator Coroutine(float time, UnityAction callback)
        {
            yield return new WaitForSeconds(time);
            if (callback!=null)
            {
                callback();
            }
        }
        
        private static IEnumerator StartSchedule(float time, float interval, UnityAction callback)
        {
            yield return new CustomWait(time, interval, callback);
            
        }

        //内部类
        class TaskBehaviour : MonoBehaviour
        {
        }
        
        class CustomWait:CustomYieldInstruction
        {
            public override bool keepWaiting
            {
                get
                {
                    //此方法通过返回false表示协程结束
                    if (Time.time-m_StatTime>=m_Time)
                    {
                        return false;
                    }else if (Time.time - m_LastTime >= m_Interval)
                    {
                        m_LastTime = Time.time;
                        m_IntervalCallback();
                    }
                    return true;
                }
            }
            private UnityAction m_IntervalCallback;
            private float m_StatTime;
            private float m_LastTime;
            private float m_Interval;
            private float m_Time;

            /// <param name="time">执行的总共时间</param>
            /// <param name="interval">每隔几秒调用一次</param>
            /// <param name="callback"></param>
            public CustomWait(float time, float interval, UnityAction callback)
            {
                m_StatTime = Time.time;
                m_LastTime = Time.time;
       
                m_Time = time;
                m_Interval = interval;
                m_IntervalCallback = callback;
            }
        }
    }
}