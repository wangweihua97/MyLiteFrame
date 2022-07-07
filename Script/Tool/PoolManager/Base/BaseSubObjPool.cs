using System;
using System.Collections.Generic;
using Script.Tool.PoolManager.Model;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Script.Tool.PoolManager.Base
{
    public class BaseSubObjPool
    {
        protected string key;
        protected int TotalNumber;
        protected ObjPool parent;
        public List<GameObject> InPoolGo = new List<GameObject>();
        public List<GameObject> OutPoolGo = new List<GameObject>();
        
        protected Func<GameObject> Instantiate;
        protected Func<Vector3> InstantiateLocalPosition;
        protected Func<Quaternion> InstantiateLocalRotation;
        /// <summary>
        /// 创建对象池
        /// </summary>
        public BaseSubObjPool(ObjPool parent,string key)
        {
            this.key = key;
            this.parent = parent;
            TotalNumber = 0;
        }

        /// <summary>
        /// 得到对象
        /// </summary>
        public GameObject Spawn()
        {
            GameObject go;
            if (InPoolGo.Count <= 0)
            {
                go = Instantiate();
            }
            else
            {
                go = InPoolGo[0];
                go.SetActive(true);
                InPoolGo.RemoveAt(0);
            }
            go.transform.SetParent(null);
            go.transform.localPosition = InstantiateLocalPosition();
            go.transform.localRotation = InstantiateLocalRotation();
            AddOutPool(go);
            return go;
        }
        
        /// <summary>
        /// 得到对象
        /// </summary>
        public GameObject Spawn(Transform parent)
        {
            GameObject go;
            if (InPoolGo.Count <= 0)
            {
                go = Instantiate();
            }
            else
            {
                go = InPoolGo[0];
                go.SetActive(true);
                InPoolGo.RemoveAt(0);
            }
            go.transform.SetParent(parent);
            go.transform.localPosition = InstantiateLocalPosition();
            go.transform.localRotation = InstantiateLocalRotation();
            go.transform.localScale = Vector3.one;
            AddOutPool(go);
            return go;
        }
        
        /// <summary>
        /// 得到对象
        /// </summary>
        public GameObject Spawn(Transform parent, Vector3 position, Vector3 rotation)
        {
            GameObject go;
            if (InPoolGo.Count <= 0)
            {
                go = Instantiate();
            }
            else
            {
                go = InPoolGo[0];
                go.SetActive(true);
                InPoolGo.RemoveAt(0);
            }
            go.transform.SetParent(parent);
            go.transform.localPosition = position;
            go.transform.localRotation = Quaternion.Euler(rotation);
            go.transform.localScale = Vector3.one;
            AddOutPool(go);
            return go;
        }
        
        /// <summary>
        /// 判断这个游戏对象是否属于这个对象池
        /// </summary>
        /// <returns>判断结果</returns>
        public bool Contains(GameObject gameObject)
        {
            return gameObject.name.Contains(key);
        }

        /// <summary>
        /// 将游戏对象放回对象池
        /// </summary>
        public void DesSpawn(GameObject gameObject)
        {
            gameObject.transform.SetParent(parent.transform);
            AddInPool(gameObject);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 添加到没有被使用的对象池
        /// </summary>
        void AddInPool(GameObject gameObject)
        {
            IPool[] iPools = gameObject.GetComponents<IPool>();
            foreach (var iPool in iPools)
            {
                iPool.InPool();
            }
            InPoolGo.Add(gameObject);
        }
        
        /// <summary>
        /// 添加到被使用的对象池
        /// </summary>
        void AddOutPool(GameObject gameObject)
        {
            IPool[] iPools = gameObject.GetComponents<IPool>();
            foreach (var iPool in iPools)
            {
                iPool.OutPool();
            }
            OutPoolGo.Add(gameObject);
        }

        /// <summary>
        /// 创建游戏对象
        /// </summary>
        
        /// <summary>
        /// 释放游戏对象
        /// </summary>
        public void ReleaseInPool()
        {
            foreach (var go in InPoolGo)
            {
                Release(go);
            }
            InPoolGo = new List<GameObject>();
        }
        /// <summary>
        /// 释放游戏对象
        /// </summary>
        public void ReleaseOutPool()
        {
            foreach (var go in OutPoolGo)
            {
                Release(go);
            }
            OutPoolGo = new List<GameObject>();
        }
        /// <summary>
        /// 释放游戏对象
        /// </summary>
        public virtual void ReleaseAll()
        {
            ReleaseInPool();
            ReleaseOutPool();
        }
        
        /// <summary>
        /// 释放游戏对象
        /// </summary>
        protected virtual void  Release(GameObject go)
        {
        }
    }
}