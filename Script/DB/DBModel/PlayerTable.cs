using Player;
using Script.DB.DBBase;
using SQLite4Unity3d;

namespace Script.DB.DBModel
{
    [DBTable(false)]
    public class PlayerTable
    {
        [PrimaryKey]
        public long UId{get; set;} 
        public bool Sex{get; set;} //true为男，false为女
        public string Name { get; set; }
        public int PassMaxLevel{ get; set; }
        
        public PlayerTable()
        {
            UId = PlayerInfo.Instance.UId;
            Name = "";
            PassMaxLevel = 0;
        }
    }
}