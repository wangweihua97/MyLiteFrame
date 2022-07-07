using System;
using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Script.Tool.PoolManager
{
    public class PoolCreatMultiGoHelper
    {
        private ObjPool parent;

        public PoolCreatMultiGoHelper(ObjPool parent)
        {
            this.parent = parent;
        }
        
        public void AddGameObject(List<string> goPaths ,GameFlowEvent attach ,Action callBack = null)
        {
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            int taskNum = goPaths.Count;
            int completedNum = 0;
            foreach (var goPath in goPaths)
            {
                parent.CreatPool(goPath,"", obj =>
                {
                    gameFlowTaskGroup.CompleteATask();
                    completedNum++;
                    if(completedNum >= taskNum)
                        callBack?.Invoke();
                    if (obj.Status == AsyncOperationStatus.Failed)
                    {
                        Debug.LogError("不存在" + goPath + "资源");
                        AddressablesHelper.instance.Release(obj);
                        parent.RemovePool(goPath);
                    }
                });
                SubObjPool subObjPool = parent.GetPool(goPath) as SubObjPool;
                gameFlowTaskGroup.Add(subObjPool.Handle);
            }
            gameFlowTaskGroup.Attach(attach);
        }
    }
}