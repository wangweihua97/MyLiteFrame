using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enemy;
using OtherMgr;
using Player;
using Script.Item;
using Script.Mgr;
using Script.Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Game
{
    public class ItemDropHelper
    {
        public async Task DropRewards()
        {
            return;
            int index = GameVariable.CurBattleIndex - 1;
            string rewardId = GameVariable.Reward[index];
            List<string> Items = new List<string>();
            foreach (var kvp in ExcelMgr.TDReward.Get(rewardId).rewardMap)
            {
                for(int i = 0; i< kvp.Value; i++)
                    Items.Add(kvp.Key);
            }
            await DoDropRewardTask(Items.ToArray());
        }
        
        public async Task DropOneCoin()
        {
            await DoDropRewardTask(new string[]{"1"});
        }

        async Task DoDropRewardTask(string[] itemNames)
        {
            if(itemNames.Length <= 0)
                return;
            foreach (var item in itemNames)
            {
                await DropItem(item);
            }
            await Task.Delay(TimeSpan.FromSeconds(0.5f));
        }
        
         Task DropItem(string itemId)
         {
             string itemName = ExcelMgr.TDItem.Get(itemId).model;
             GameObject item = ItemLoadMgr.Instance.Get(itemName);
             ItemShowPrefab itemShowPrefab =
                 PoolManager.CommonPool.Spawn("Item/ItemShow").GetComponent<ItemShowPrefab>();
             ItemShowData itemShowData;
             itemShowData.Item = item;
             itemShowPrefab.SetData(itemShowData);
             itemShowPrefab.transform.position = EnemyMgr.instance.transform.position + new Vector3(0, 0.3f, 0f);
             itemShowPrefab.transform.forward = PlayerMgr.instance.GetTransform().position - EnemyMgr.instance.transform.position;
             
             Vector3 position = RandomDrop(EnemyMgr.instance.gameObject, 45, 10, 5);
             
             itemShowPrefab.PlayShow(position + new Vector3(0,0.1f,0));
             return Task.Delay(TimeSpan.FromSeconds(0.2f));
         }

        /// <summary>
        /// 得到go面前随机掉落点
        /// </summary>
        /// <param name="go"></param>
        /// <param name="angle">掉落的角度</param>
        /// <param name="maxDistance">掉落的最大距离</param>
        /// <param name="minDistance">掉落的最小距离</param>
        /// <returns> 掉落的位置</returns>
        private Vector3 RandomDrop(GameObject go, float angle, float maxDistance, float minDistance)
        {
            float randomAngle = Random.Range(-angle ,angle);
            float distance = Random.Range(minDistance ,maxDistance);
            Vector3 direction = Quaternion.AngleAxis(randomAngle, EnemyMgr.instance.transform.up) *
                                go.transform.forward;
            Vector3 position = Vector3.Normalize(direction) * distance;
            return position + go.transform.position;
        }
    }
}