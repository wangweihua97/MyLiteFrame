using System;
using System.Collections.Generic;
using Script.Tool.PoolManager.Base;
using Script.Tool.PoolManager.Model;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Script.Tool.PoolManager
{
    public class SubObjPool : BaseSubObjPool
    {
        public AsyncOperationHandle<IList<GameObject>> Handle;

        /// <summary>
        /// 创建对象池
        /// </summary>
        public SubObjPool(ObjPool parent,string key, string label, Action<AsyncOperationHandle<IList<GameObject>>> completed) : base(parent ,key)
        {
            if (AddressablesHelper.instance.ContainsKey(key ,label) && AddressablesHelper.instance.GetHandle(key ,label).IsValid())
            {
                Handle = AddressablesHelper.instance.GetHandle(key ,label).Convert<IList<GameObject>>();
                completed.Invoke(Handle);
            }
            else
            {
                Handle = AddressablesHelper.instance.LoadAssetsAsync<GameObject>(key, label);
                Handle.Completed += completed;
            }

            base.Instantiate = Instantiate;
            base.InstantiateLocalPosition = InstantiateLocalPosition;
            base.InstantiateLocalRotation = InstantiateLocalRotation;
        }
        
        GameObject Instantiate()
        {
            if (!Handle.IsValid())
            {
                Debug.LogError("对象池的物体没有被载入");
                return null;
            }

            TotalNumber++;
            GameObject go = GameObject.Instantiate(Handle.Result[0]);
            go.name = key + "-" + TotalNumber;
            return  go;
        }

        Vector3 InstantiateLocalPosition()
        {
            return Handle.Result[0].transform.localPosition;
        }

        Quaternion InstantiateLocalRotation()
        {
            return Handle.Result[0].transform.localRotation;
        }
        
        
        /// <summary>
        /// 释放游戏对象
        /// </summary>
        protected override void Release(GameObject go)
        {
            base.Release(go);
            if (!Addressables.ReleaseInstance(go)) 
                GameObject.Destroy(go);
        }
    }
}