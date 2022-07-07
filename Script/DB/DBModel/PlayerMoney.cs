using Player;
using Script.DB.DBBase;
using SQLite4Unity3d;

namespace Script.DB.DBModel
{
    [DBTable(true)]
    public class PlayerMoney
    {
        [PrimaryKey]
        public long UId
        {
            get;
            set;
        }

        public long Money
        {
            get;
            set;
        }

        public PlayerMoney()
        {
            UId = PlayerInfo.Instance.UId;
            Money = 0;
        }
    }
}