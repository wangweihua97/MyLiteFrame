using System;
using Player;
using Script.DB.DBBase;
using SQLite4Unity3d;

namespace Script.DB.DBModel
{
    [DBTable(true)]
    public class DaysRecordTable
    {
        [PrimaryKey]
        public long UId
        {
            get;
            set;
        }
        public DateTime CurDate { get; set; }
        public int Days { get; set; }
        public int PunchTime { get; set; }
        public int Calorie { get; set; }
        public int Time { get; set; }
        
        public int CurDayPunchTime { get; set; }
        public int CurDayCalorie { get; set; }
        public int CurDayTime { get; set; }

        public DaysRecordTable()
        {
            UId = PlayerInfo.Instance.UId;
            CurDate = DateTime.Now.AddDays(-1);
            Days = 0;
            PunchTime = 0;
            Calorie = 0;
            Time = 0;
            CurDayPunchTime = 0;
            CurDayCalorie = 0;
            CurDayTime = 0;
        }
    }
}