using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Player;
using Script.DB;
using Script.DB.DBModel;
using Script.Excel.Table;
using Script.Main;
using Script.Mgr;
using Script.Model;

namespace Script.Game
{
    public class RecordHelper
    {
        private GameState _state;
        public RecordHelper()
        {
        }

        public void RecordTheBattle(GameState state)
        {
            _state = state;
            Global.instance.StartCoroutine(StartRecord());
        }

        IEnumerator StartRecord()
        {
            RecordBattleInfo();
            yield return 0;
            RecordDaysInfo();
            yield return 0;
            switch (GameVariable.GameMode)
            {
                case GameMode.LevelMode:
                    RecordLevelInfo();
                    break;
                case GameMode.TrainMode:
                    RecordTrainInfo();
                    break;
            }
        }
        
        void RecordBattleInfo()
        {
            BattleTable battleTable = new BattleTable();
            battleTable.CurScore = MainGameCtr.instance.GradeHelper.curScore;
            battleTable.TotalScore = MainGameCtr.instance.GradeHelper.totalScore;
            battleTable.CurCalorie = MainGameCtr.instance.GradeHelper.curCalorie;
            battleTable.VeryGoodCount = MainGameCtr.instance.GradeHelper.veryGoodCount;
            battleTable.PerfectCount = MainGameCtr.instance.GradeHelper.perfectCount;
            battleTable.MissCount = MainGameCtr.instance.GradeHelper.missCount;
            battleTable.MaxPerfectTime = MainGameCtr.instance.GradeHelper.maxPerfectTime;
            battleTable.GameTime = MainGameCtr.instance.GradeHelper.gameTime;
            battleTable.ActNumber = MainGameCtr.instance.GradeHelper.actNumber;
            battleTable.GradeType = (int)MainGameCtr.instance.GradeHelper.gradeType;
            battleTable.BodyAge = MainGameCtr.instance.GradeHelper.bodyAge;
            battleTable.ComboCount = MainGameCtr.instance.ComboHelper.MaxComboCount;
            DBManager.Instance.Insert(battleTable ,false);
        }

        void RecordDaysInfo()
        {
            IEnumerable<DaysRecordTable> daysRecordTables = DBManager.Instance.Table<DaysRecordTable>();
            DaysRecordTable daysRecordTable = daysRecordTables.First();
            daysRecordTable.Calorie += Convert.ToInt32(MainGameCtr.instance.GradeHelper.curCalorie);
            daysRecordTable.PunchTime += MainGameCtr.instance.GradeHelper.actNumber;
            daysRecordTable.Time += Convert.ToInt32(MainGameCtr.instance.GradeHelper.gameTime);
            if (!daysRecordTable.CurDate.Year.Equals(DateTime.Now.Year) ||
                !daysRecordTable.CurDate.Month.Equals(DateTime.Now.Month) ||
                !daysRecordTable.CurDate.Day.Equals(DateTime.Now.Day))
            {
                daysRecordTable.Days += 1;
                daysRecordTable.CurDayCalorie = Convert.ToInt32(MainGameCtr.instance.GradeHelper.curCalorie);
                daysRecordTable.CurDayPunchTime = MainGameCtr.instance.GradeHelper.actNumber;
                daysRecordTable.CurDayTime = Convert.ToInt32(MainGameCtr.instance.GradeHelper.gameTime);
            }
            else
            {
                daysRecordTable.CurDayCalorie += Convert.ToInt32(MainGameCtr.instance.GradeHelper.curCalorie);
                daysRecordTable.CurDayPunchTime += MainGameCtr.instance.GradeHelper.actNumber;
                daysRecordTable.CurDayTime += Convert.ToInt32(MainGameCtr.instance.GradeHelper.gameTime);
            }
                
            daysRecordTable.CurDate = DateTime.Now;
            DBManager.Instance.Insert(daysRecordTable ,true);
        }

        void RecordLevelInfo()
        {
            LevelTable levelTable = new LevelTable();
            levelTable.UId = PlayerInfo.Instance.UId;
            levelTable.LevelId = GameVariable.CurLevelId;

            LevelTable find = DBManager.Instance.Find<LevelTable>(levelTable.UIdLevelId);
            if (find == null)
            {
                levelTable.Grade = (int)MainGameCtr.instance.GradeHelper.gradeType;
                levelTable.Score = MainGameCtr.instance.GradeHelper.curScore;
                levelTable.PlayTime = 1;
                if(_state == GameState.Succeed)
                {
                    levelTable.ClearanceTime = 1;
                }
                else
                {
                    levelTable.ClearanceTime = 0;
                }
            }
            else
            {
                levelTable.Grade = Math.Min((int)MainGameCtr.instance.GradeHelper.gradeType,find.Grade); ;
                levelTable.Score = Math.Max(MainGameCtr.instance.GradeHelper.curScore, find.Score);
                levelTable.PlayTime = find.PlayTime + 1;
                if (_state == GameState.Succeed)
                    levelTable.ClearanceTime = find.ClearanceTime + 1;
                else
                    levelTable.ClearanceTime = find.ClearanceTime;
            }
            
            levelTable.UIdLevelId = levelTable.UId + Int64.Parse(levelTable.LevelId);
            DBManager.Instance.Insert(levelTable, true);

            PlayerTable playerTable = PlayerInfo.Instance.PlayerData;
            int curLevelId = Int32.Parse(GameVariable.CurLevelId);
            if (playerTable.PassMaxLevel < curLevelId && _state == GameState.Succeed)
            {
                playerTable.PassMaxLevel = curLevelId;
                DBManager.Instance.Insert(playerTable, true);
            }
        }
        
        void RecordTrainInfo()
        {
            RecordMusicInfo();
            TrainTable trainTable = new TrainTable();
            trainTable.UId = PlayerInfo.Instance.UId;
            trainTable.TrainId = GameVariable.CurTrainId;

            TrainTable find = DBManager.Instance.Find<TrainTable>(trainTable.UIdTrainId);
            if (find == null)
            {
                trainTable.PlayTime = 1;
                if(_state == GameState.Succeed)
                {
                    trainTable.ClearanceTime = 1;
                }
                else
                {
                    trainTable.ClearanceTime = 0;
                }
                trainTable.Score = MainGameCtr.instance.GradeHelper.curScore;
                trainTable.Star = GetStar(trainTable.TrainId, trainTable.Score);
            }
            else
            {
                trainTable.PlayTime = find.PlayTime + 1;
                if(_state == GameState.Succeed)
                {
                    trainTable.ClearanceTime = find.ClearanceTime + 1;
                }
                else
                {
                    trainTable.ClearanceTime = find.ClearanceTime;
                }
                trainTable.Score = Math.Max(MainGameCtr.instance.GradeHelper.curScore ,find.Score) ;
                trainTable.Star = Math.Max(GetStar(trainTable.TrainId, trainTable.Score) ,find.Star);
            }
            
            trainTable.UIdTrainId = trainTable.UId + Int64.Parse(trainTable.TrainId);
            DBManager.Instance.Insert(trainTable, true);
        }

        int GetStar(string trainId, int score)
        {
            TDTraining training = ExcelMgr.TDTraining.Get(trainId);
            if(score > training.starpoint5)
                return 5;
            if(score > training.starpoint4)
                return 4;
            if(score > training.starpoint3)
                return 3;
            if(score > training.starpoint2)
                return 2;
            if(score > training.starpoint1)
                return 1;
            return 0;
        }

        void RecordMusicInfo()
        {
            MusicTable musicTable = new MusicTable();
            musicTable.UId = PlayerInfo.Instance.UId;
            musicTable.MusicId = GameVariable.MusicId[GameVariable.CurBattleIndex];

            MusicTable find = DBManager.Instance.Find<MusicTable>(musicTable.UIdMusicId);
            if (find == null)
            {
                musicTable.PlayTime = 1;
            }
            else
            {
                musicTable.PlayTime = find.PlayTime + 1;
            }
            musicTable.UIdMusicId = musicTable.UId + Int64.Parse(musicTable.MusicId);
            DBManager.Instance.Insert(musicTable, true);
        }
    }
}