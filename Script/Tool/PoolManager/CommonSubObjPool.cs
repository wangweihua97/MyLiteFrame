using System;
using System.Collections.Generic;
using Script.Tool.PoolManager.Base;
using Script.Tool.PoolManager.Model;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Script.Tool.PoolManager
{
    public class CommonSubObjPool : BaseSubObjPool
    {
        private GameObject _go;

        /// <summary>
        /// 创建对象池
        /// </summary>
        public CommonSubObjPool(ObjPool parent,string key, GameObject go) : base(parent ,key)
        {
            _go = go;
            if (_go.activeInHierarchy)
            {
                _go.SetActive(false);
                _go.transform.SetParent(parent.transform);
            }
            base.Instantiate = Instantiate;
            base.InstantiateLocalPosition = InstantiateLocalPosition;
            base.InstantiateLocalRotation = InstantiateLocalRotation;
        }
        
        GameObject Instantiate()
        {
            TotalNumber++;
            GameObject go = GameObject.Instantiate(_go);
            go.SetActive(true);
            go.name = key + "-" + TotalNumber;
            return  go;
        }

        Vector3 InstantiateLocalPosition()
        {
            return Vector3.zero;
        }

        Quaternion InstantiateLocalRotation()
        {
            return Quaternion.Euler(Vector3.zero);
        }
        
        
        /// <summary>
        /// 释放游戏对象
        /// </summary>
        protected override void Release(GameObject go)
        {
            base.Release(go);
            GameObject.Destroy(go);
        }
        
        public override void ReleaseAll()
        {
            base.ReleaseAll();
            GameObject.Destroy(_go);
        }
    }
}