using System.Collections.Generic;
using Events;
using OtherMgr;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Script.Item
{
    public class ItemLoadMgr
    {
        public static ItemLoadMgr Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new ItemLoadMgr();
                return _instance;
            }
        }
        static ItemLoadMgr _instance;

        private List<string> _paths;

        private ItemLoadMgr()
        {
            _paths = new List<string>();
        }

        //载入bundle中的道具模型
        public void LoadModels(string[] paths , GameFlowEvent attach)
        {
            _paths = new List<string>(paths);
            PoolManager.ItemPool.poolCreatMultiGoHelper.AddGameObject(_paths,attach, () =>
            {
                ;
            });
        }

        //根据道具名称得到GameObject
        public GameObject Get(string itemName)
        {
            return PoolManager.ItemPool.Spawn("Item/" + itemName);
        }
        
        //将道具回收
        public void Recycle(GameObject go)
        {
            PoolManager.ItemPool.DesSpawn(go);
        }
        
        
        //清空所有道具
        public void ClearModels()
        {
            foreach (var path in _paths)
            {
                PoolManager.ItemPool.RemovePool(path);
            }
        }
    }
}