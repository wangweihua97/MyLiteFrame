using Player;
using Script.DB.DBBase;
using SQLite4Unity3d;

namespace Script.DB.DBModel
{
    [DBTable(false)]
    public class BattleTable
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id
        {
            get;
            set;
        }
        
        public long PlayerUId { get; set; } 
        public int CurScore { get; set; } //当前得分
        public int TotalScore { get; set; }  //总共得分
        public float CurCalorie{ get; set; }  //当前卡路里
        
        
        public int VeryGoodCount{ get; set; }  //很好数量
        public int PerfectCount{ get; set; }  //完美数量
        public int MissCount{ get; set; } //错过数量
        public int MaxPerfectTime{ get; set; }  //最大完美连击的次数
        public float GameTime{ get; set; } 
        
        public int ActNumber{ get; set; }  //出拳数
        public int GradeType{ get; set; }  //玩家评级
        public int BodyAge{ get; set; } 
        public int ComboCount{ get; set; }

        public BattleTable()
        {
            PlayerUId = PlayerInfo.Instance.UId;
        }
    }
}