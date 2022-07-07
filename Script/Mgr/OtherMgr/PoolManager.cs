using System;
using Script.Tool.PoolManager;
using UnityEngine;

namespace OtherMgr
{
    public class PoolManager : BasePoolManager
    {
        public static ObjPool GameObjPool;
        public static ObjPool EffectPool;
        public static ObjPool CharacterPool;
        public static ObjPool CommonPool;
        public static ObjPool ItemPool;
        public static void CreatPoolManager()
        {
            GameObject go = new GameObject("PoolManager");
            go.AddComponent<PoolManager>();
            DontDestroyOnLoad(go);
        }

        private void Start()
        {
            GameObjPool = CreatPool("GameObjPool");
            EffectPool = CreatPool("EffectPool");
            CharacterPool = CreatPool("CharacterPool");
            CommonPool = CreatPool("CommonPool");
            ItemPool = CreatPool("ItemPool");
        }
    }
}