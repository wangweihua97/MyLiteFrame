using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Events
{
    //一个任务组，可以执行多个任务
    public class GameFlowTaskGroup  : CustomYieldInstruction
    {
        public int taskNum;
        public int completedNum;
        private List<TaskPercentFunc> _percentActions = new List<TaskPercentFunc>();
        public Action Completed;

        
        /// <summary>
        /// 初始化
        /// </summary>
        public GameFlowTaskGroup()
        {
            taskNum = 0;
            completedNum = 0;
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="taskNum">子任务的数量</param>
        public GameFlowTaskGroup(int taskNum)
        {
            this.taskNum = taskNum;
            completedNum = 0;
        }
        
        /// <summary>
        /// 添加一个子任务
        /// </summary>
        /// <param name="action">添加时执行</param>
        public void Add()
        {
            taskNum++;
            _percentActions.Add(() =>
            {
                return 1;
            });
        }
        
        /// <summary>
        /// 添加一个子任务
        /// </summary>
        /// <param name="action">添加时执行</param>
        /// <param name="taskPercentFunc">当前子任务完成的百分比函数</param>
        public void Add(TaskPercentFunc taskPercentFunc)
        {
            taskNum++;
            _percentActions.Add(taskPercentFunc);
        }
        
        /// <summary>
        /// 添加一个子任务
        /// </summary>
        /// <param name="action">添加时执行</param>
        /// <param name="handle">AsyncOperationHandle加载进度</param>
        public void Add(AsyncOperationHandle handle)
        {
            taskNum++;
            _percentActions.Add(() =>
            {
                if (handle.IsValid())
                    return handle.PercentComplete;
                else
                    return 1;
            });
        }

        /// <summary>
        /// 所有子任务完成百分比
        /// </summary>
        /// <returns>当前任务组完成的百分比</returns>
        public float PercentageAction()
        {
            if (taskNum == 0)
                return 1;
            float totalProgress = 0;
            foreach (var action in _percentActions)
            {
                totalProgress += action.Invoke();
            }
            return totalProgress / taskNum;
        }

        /// <summary>
        /// 当前任务组绑定到GameFlowEvent中
        /// </summary>
        public void Attach(GameFlowEvent gameFlowEvent)
        {
            GameFlowTask gameFlowTask = FlowTaskFactory.CreatTask(WaitTask ,PercentageAction);
            gameFlowEvent.AddTask(gameFlowTask);
        }
        
        /// <summary>
        /// 等待所有子任务完成
        /// </summary>
        public IEnumerator WaitTask(GameFlowTask gameFlowTask)
        {
            yield return new WaitForSeconds(0.1f);
            yield return this;
            if(Completed != null)
                Completed.Invoke();
            gameFlowTask.Completed.Invoke();
        }
        
        /// <summary>
        /// 等待所有子任务完成
        /// </summary>
        public override bool keepWaiting
        {
            get
            {
                if (completedNum < taskNum)
                    return true;
                else
                {
                    return false;
                }
            }
        }
        
        

        public void CompleteATask()
        {
            completedNum++;
        }
    }
}