using System;
using Player;
using Script.DB.DBBase;
using SQLite4Unity3d;

namespace Script.DB.DBModel
{
    [DBTable(false)]
    public class LevelTable
    {
        [PrimaryKey]
        public long UIdLevelId
        {
            get;
            set;
        }
        public long UId
        {
            get;
            set;
        }
        public string LevelId
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
        public int Score
        {
            get;
            set;
        }
        
        public int Grade
        {
            get;
            set;
        }
        
        public LevelTable()
        {
            UId = PlayerInfo.Instance.UId;
        }
        
    }
}