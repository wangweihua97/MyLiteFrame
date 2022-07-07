using System;
using System.Collections.Generic;
using Script.Tool.PoolManager.Base;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Script.Tool.PoolManager
{
    public class ObjPool : MonoBehaviour
    {
        public BasePoolManager parent;

        public PoolCreatMultiGoHelper poolCreatMultiGoHelper;
        //储存各个对象接口
        private Dictionary<string, BaseSubObjPool> _pools = new Dictionary<string, BaseSubObjPool>();

        /// <summary>
        /// 初始化对象池
        /// </summary>
        public void Init(BasePoolManager parent)
        {
            this.parent = parent;
            poolCreatMultiGoHelper = new PoolCreatMultiGoHelper(this);
        }
        
        /// <summary>
        /// 摧毁对象池
        /// </summary>
        public void DoDestroy()
        {
            Destroy(gameObject);
        }
        
        /// <summary>
        /// 创建对象池
        /// </summary>
        public void CreatPool(string key, string label = "", Action<AsyncOperationHandle<IList<GameObject>>> completed = null)
        {
            if (_pools.ContainsKey(key))
            {
                SubObjPool sub = _pools[key] as SubObjPool;
                completed.Invoke(sub.Handle);
                return;
            }
            
            if (completed == null)
            {
                completed = obj =>
                {
                    if (obj.Status != AsyncOperationStatus.Succeeded)
                    {
                        Debug.LogError("不存在" + key + "资源");
                        AddressablesHelper.instance.Release(obj);
                        _pools.Remove(key);
                    }
                };
            }
            SubObjPool subObjPool = new SubObjPool(this ,key ,label ,completed);
            _pools.Add(key ,subObjPool);
        }
        
        /// <summary>
        /// 是否包含对象池
        /// </summary>
        public bool ContainsPool(string key)
        {
            return _pools.ContainsKey(key);
        }
        
        /// <summary>
        /// 删除对象池
        /// </summary>
        public void RemovePool(string key)
        {
            _pools[key].ReleaseAll();
            _pools.Remove(key);
        }
        
        /// <summary>
        /// 得到对象池
        /// </summary>
        public BaseSubObjPool GetPool(string key)
        {
            return _pools[key];
        }
        
        /// <summary>
        /// 生成对象
        /// </summary>
        public GameObject Spawn(string key)
        {
            return _pools[key].Spawn();
        }
        
        /// <summary>
        /// 生成对象
        /// </summary>
        public GameObject Spawn(string key ,Transform parent)
        {
            return _pools[key].Spawn(parent);
        }
        
        /// <summary>
        /// 生成对象
        /// </summary>
        public GameObject Spawn(string key ,Transform parent, Vector3 position, Vector3 rotation)
        {
            return _pools[key].Spawn(parent ,position ,rotation);
        }
        
        /// <summary>
        /// 放回对象
        /// </summary>
        public void DesSpawn(string key ,GameObject go)
        {
            _pools[key].DesSpawn(go);
        }
        
        /// <summary>
        /// 放回对象
        /// </summary>
        public void DesSpawn(GameObject go)
        {
            foreach (var KVP in _pools)
            {
                if (KVP.Value.Contains(go))
                {
                    KVP.Value.DesSpawn(go);
                    return;
                }
            }
            Destroy(go);
        }

        /// <summary>
        /// 清除池中所有对象和bunld
        /// </summary>
        public void Clear()
        {
            foreach (var KVP in _pools)
            {
                KVP.Value.ReleaseAll();
            }
            _pools.Clear();
        }


        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        private void OnDestroy()
        {
            Clear();
        }
    }
}