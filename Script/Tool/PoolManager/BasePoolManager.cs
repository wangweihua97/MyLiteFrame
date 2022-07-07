using System;
using UnityEngine;

namespace Script.Tool.PoolManager
{
    public class BasePoolManager : MonoBehaviour
    {
        public static SpritesPool SpritesPool;
        /// <summary>
        /// 创建对象池
        /// </summary>
        /// <param name="name">对象池的名字</param>
        /// <returns>null</returns>
        public ObjPool CreatPool(string name)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(transform);
            return InitPool(go);
        }

        /// <summary>
        /// 初始化对象池
        /// </summary>
        ObjPool InitPool(GameObject go)
        {
            ObjPool objPool = go.AddComponent<ObjPool>();
            objPool.Init(this);
            return objPool;
        }
    }
}