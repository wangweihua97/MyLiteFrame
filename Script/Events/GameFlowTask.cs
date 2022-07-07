using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Events
{
    
    //单个任务
    public class GameFlowTask
    {
        public Action Completed;
        public CoroutineDelegate CoroutineDelegate;
        
        /// <summary>
        /// 任务的进度
        /// </summary>
        /// <param name="index">要得到数据的位置</param>
        public float Percent
        {
            get
            {
                return _percentAction.Invoke();
            }
        }

        //自定义string类型数据
        public string StrVariable;
        //自定义int类型数据
        public int IntVariable;
        //自定义float类型数据
        public float FloatVariable;
        //自定义回调
        public Action CallBack;
        private TaskPercentFunc _percentAction;
        public GameFlowTask(CoroutineDelegate coroutineDelegate ,TaskPercentFunc percentageAction)
        {
            this.CoroutineDelegate = coroutineDelegate;
            _percentAction = percentageAction;
        }
        
        public GameFlowTask(CoroutineDelegate coroutineDelegate)
        {
            this.CoroutineDelegate = coroutineDelegate;
            _percentAction = () =>
            {
                return 1;
            };
        }
        
        public GameFlowTask(TaskPercentFunc percentageAction)
        {
            this.CoroutineDelegate = null;
            _percentAction = percentageAction;
        }
        
        public GameFlowTask()
        {
            this.CoroutineDelegate = null;
            _percentAction = () =>
            {
                return 1;
            };
        }
    }
    
}