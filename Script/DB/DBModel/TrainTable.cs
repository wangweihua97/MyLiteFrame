using System;
using Player;
using Script.DB.DBBase;
using SQLite4Unity3d;

namespace Script.DB.DBModel
{
    [DBTable(false)]
    public class TrainTable
    {
        [PrimaryKey]
        public long UIdTrainId
        {
            get;
            set;
        }
        public long UId
        {
            get;
            set;
        }
        public string TrainId
        {
            get;
            set;
        }

        public int PlayTime
        {
            get;
            set;
        }

        public int ClearanceTime
        {
            get;
            set;
        }
        
        public int Star
        {
            get;
            set;
        }
        
        public int Score
        {
            get;
            set;
        }
        public TrainTable()
        {
            UId = PlayerInfo.Instance.UId;
        }
    }
}