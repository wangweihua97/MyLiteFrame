using System;
using Player;
using Script.DB.DBBase;
using SQLite4Unity3d;

namespace Script.DB.DBModel
{
    [DBTable(false)]
    public class MusicTable
    {
        [PrimaryKey]
        public long UIdMusicId
        {
            get;
            set;
        }
        
        public long UId
        {
            get;
            set;
        }
        public string MusicId
        {
            get;
            set;
        }

        public int PlayTime
        {
            get;
            set;
        }
        
        public MusicTable()
        {
            UId = PlayerInfo.Instance.UId;
        }
    }
}