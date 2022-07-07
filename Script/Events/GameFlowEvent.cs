using System;
using System.Collections;
using System.Collections.Generic;
using Script.Mgr;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace Events
{
    public class GameFlowEvent  : CustomYieldInstruction
    {
        public string EventName;
        public Action Completed;
        private int _index;
        private float _loadingWeight;
        private List<GameFlowTask> _list = new List<GameFlowTask>();
        private int _taskNum;
        private int _completedNum;
        private event FlowEvent m_event;
        /// <summary>
        /// 任务的进度
        /// </summary>
        public float Percent
        {
            get
            {
                if (_taskNum == 0)
                    return 1;
                float totalPercent = 0;
                foreach (var task in _list)
                {
                    totalPercent += task.Percent;
                }
                return totalPercent / _taskNum;
            }
        } 
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="completed">当任务完成时执行</param>
        public GameFlowEvent(string EventName ,Action completed ,int index ,float loadingWeight)
        {
            this.EventName = EventName;
            this.Completed = completed;
            this._index = index;
            this._loadingWeight = loadingWeight;
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="gameFlowTask">要添加的任务</param>
        public void AddTask(GameFlowTask gameFlowTask)
        {
            _taskNum++;
            _list.Add(gameFlowTask);
            gameFlowTask.Completed += TaskCompleted;
            if(gameFlowTask.CoroutineDelegate != null)
                EventCenter.ins.StartCoroutine(gameFlowTask.CoroutineDelegate(gameFlowTask));
        }

        public int GetIndex()
        {
            return _index;
        }
        
        public float GetLoadingWeight()
        {
            return _loadingWeight;
        }

        /// <summary>
        /// 执行当前任务
        /// </summary>
        public void Invoke()
        {
            m_event?.Invoke();
            GameFlowMgr.Clear();
            foreach (var KVP in GameFlowMgr.Dictionary)
            {
                KVP.Value.InvokeEvent(EventName);
            }
            EventCenter.ins.StartCoroutine(WaitTasksCompleted());
        }

        /// <summary>
        /// 添加Listener，当前任务执行时调用
        /// </summary>
        public void AddListener(FlowEvent action)
        {
            m_event += action;
        }

        /// <summary>
        /// 移除Listener
        /// </summary>
        public void RemoveListener(FlowEvent action)
        {
            m_event -= action;
        }
        
        /// <summary>
        /// 有一个子任务完成
        /// </summary>
        public void TaskCompleted()
        {
            _completedNum++;
        }

        /// <summary>
        /// 等待所有子任务完成
        /// </summary>
        public IEnumerator WaitTasksCompleted()
        {
            yield return new WaitForSeconds(0.1f);
            yield return this;
            Clear();
            Completed.Invoke();
        }
        public override bool keepWaiting
        {
            get
            {
                if (_completedNum < _taskNum)
                    return true;
                else
                {
                    return false;
                }
            }
        }
        
        /// <summary>
        /// 清理
        /// </summary>
        void Clear()
        {
            _completedNum = 0;
            _taskNum = 0;
            _list.Clear();
        }

    }
}