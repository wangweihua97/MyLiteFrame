using System.Collections;
using System.Collections.Generic;
using Player;
using Script.DB;
using Script.DB.DBModel;
using Tool.Others;
using UI.Base;
using UnityEngine.UI;

namespace UI.Main
{
    public class PlayerInfoComponent : UComponent
    {
        public Text Days;
        public Text PunchTime;
        public Text Calorie;
        public Text Time;

        public void ReadDbData()
        {
            IEnumerable<DaysRecordTable> a = DBManager.Instance.Table<DaysRecordTable>();
            DaysRecordTable daysRecord = DBManager.Instance.Get<DaysRecordTable>(PlayerInfo.Instance.UId);
            Days.text = daysRecord.Days.ToString();
            PunchTime.text = daysRecord.CurDayPunchTime.ToString();
            Calorie.text = daysRecord.CurDayCalorie.ToString();
            Time.text = daysRecord.CurDayTime.SecondToTime(":");
        }
    }
}