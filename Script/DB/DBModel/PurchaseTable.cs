using System;
using Player;
using Script.DB.DBBase;
using SQLite4Unity3d;

namespace Script.DB.DBModel
{
    [DBTable(false)]
    public class PurchaseTable
    {
        [PrimaryKey]
        public long UIdItemId
        {
            get;
            set;
        }
        
        public long UId
        {
            get;
            set;
        }
        public string ItemId
        {
            get;
            set;
        }

        public int Count
        {
            get;
            set;
        }
        
        public PurchaseTable()
        {
            UId = PlayerInfo.Instance.UId;
        }
    }
}