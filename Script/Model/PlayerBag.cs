using System.Collections.Generic;
using Script.Mgr;
using Script.Tool;

namespace Script.Model
{
    public static class PlayerBag
    {
        /**玩家背包数据*/
        public static PlayerBagModel PlayerBagModel;
        /**装饰url*/
        public const string CharacterIconUrl = "CharacterIcon";
        /**关卡icon url*/
        public const string LvIconUrl = "LvIcon";

        public static void initBagData()
        {
            PlayerBagModel = NativeStoreTool.Get<PlayerBagModel>("PlayerBagModel");
            if (null == PlayerBagModel)
            {
                PlayerBagModel = new PlayerBagModel();
                PlayerBagModel.gold = 5000;
                PlayerBagModel.DressUpBagData = new List<DressUpBagData>();
                PlayerBagModel.DressUpBagDataDic = new Dictionary<string, DressUpBagData>();
                NativeStoreTool.Set("PlayerBagModel" ,PlayerBagModel);
            }
            //
            if(PlayerBagModel.gold<=1000)PlayerBagModel.gold = 5000;
            if (null == PlayerBagModel.DressUpBagDataDic)
            {
                PlayerBagModel.DressUpBagDataDic = new Dictionary<string, DressUpBagData>();
            }
        }
        
        public static bool ContainsItem(string itemId, int tgValue)
        {
            return PlayerBagModel.ItemBagDataDic.ContainsKey(itemId)&&PlayerBagModel.ItemBagDataDic[itemId].count >= tgValue;
        }

        /**
         * 添加道具
         */
        public static void AddItem(string itemId, int count, bool autoSave=true)
        {
            //增加数量
            if (PlayerBagModel.ItemBagDataDic.ContainsKey(itemId))
            {
                ItemBagData data = PlayerBagModel.ItemBagDataDic[itemId];
                data.count += count;
            }
            else
            {
                //添加新项
                ItemBagData data = new ItemBagData();
                data.TDItemID = itemId;
                data.count = count;
                PlayerBagModel.ItemBagDataDic[data.TDItemID] = data;
            }
            //自动保存
            if (autoSave)
            {
                saveBagData();
            }
        }
        /**
         * 减少道具
         */
        public static void delItem(string itemId, int count, bool autoSave=true)
        {
            
            if (PlayerBagModel.ItemBagDataDic.ContainsKey(itemId))
            {
                ItemBagData data = PlayerBagModel.ItemBagDataDic[itemId];
                if (data.count > count)
                {
                    //减少数量
                    data.count -= count;
                }
                else
                {
                    //删除项
                    PlayerBagModel.ItemBagDataDic.Remove(itemId);
                }
                //自动保存
                if (autoSave)
                {
                    saveBagData();
                }
            }
        }

        public static bool ContainsMoney(int money)
        {
            return PlayerBagModel.gold >= money;
        }

        public static bool ContainsCharacterID(string id)
        {
            return PlayerBagModel.DressUpBagDataDic.ContainsKey(id);
        }
        
        public static void BuyCharacter(string id)
        {
            //增加数据
            DressUpBagData data = new DressUpBagData();
            data.TDCharacterID = id;
            data.count = 1;
            PlayerBagModel.DressUpBagData.Add(data);
            PlayerBagModel.DressUpBagDataDic[data.TDCharacterID] = data;
            //扣钱
            PlayerBagModel.gold -= ExcelMgr.TDCharacter.Get(id).price;
            NativeStoreTool.Set("PlayerBagModel" , PlayerBagModel);
            //发送
            // NativeStoreTool.Set("DressUp_buy_item" , id);
        }
        
        public static void saveBagData()
        {
            NativeStoreTool.Set("PlayerBagModel" , PlayerBagModel);
        }
    }
}