using OtherMgr;
using Script.Tool.PoolManager;
using UnityEngine;

namespace Examples.GoPool
{
    public class GoPoolMgr: BasePoolManager
    {
        public static ObjPool GoPool;
        public static void CreatPoolManager()
        {
            GameObject go = new GameObject("PoolManager");
            go.AddComponent<GoPoolMgr>();
            DontDestroyOnLoad(go);
        }

        private void Awake()
        {
            GoPool = CreatPool("GoPool");
        }
    }
}