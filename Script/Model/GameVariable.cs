﻿using System;
 using System.Collections.Generic;
 using System.Linq;
 using Script.Excel.Table;
 using Script.Main;
 using Script.Mgr;
 using Script.Model;
 using Script.Tool;
 using UnityEngine.PlayerLoop;

namespace Script.Model
{
    public static class GameVariable
    {
        public static GameMode GameMode;
        
        public static float CenterY = 176;
        public static float Radius = 150;
        public static float BirthToClickTime = 4f;
        public static float buffSpeed = 1f;
        public static int height = 716;
        public static float speed = 400;
        public static float AnimationSpeed = 1;
        
        public static int LoginEnterCount = 0;
        
        public static GameState GameState;
        public static string CurScene;
        public static string CurLevelId;
        public static string CurTrainId;
        public static int BattleSum;
        public static string[] BattleId;
        public static string[] MusicId;
        public static string[] EnemyId;
        public static int[] EnemyHp;
        public static string[] SceneBlock;
        public static int[] Born;
        public static string[] Reward;
        
        public static int CurBattleIndex;

        public static int EnemyAttack;
        public static string EnemyName = "";

        public static int PlayerHp = 500;
        public static int PlayerAttack = 500;

        public static float beatTime = 0.444444f;
        public static float StopTime = 0.3f;
        public static float MissCalorie = 0.8f;
        public static int Calorie = 160;
        public static float RunTimePerBlock = 7f;
        public static string LevelName = "";

        public static bool isInGame = false;
        public static float preTime = 0;
        public static float preEffect = 0;
        public static int FullEnergy = 0;
        public static int ComboTime = 0;
        public static float delayTime = 0;

        public static int MaxBlockNumber = 4; //最大同时存在的地块数量
        public static bool Auto = false;
        public static string PlayerRunMusic = "";
        
        public static void InitLevel(string LevelId)
        {
            CurBattleIndex = 0;
            GameMode = GameMode.LevelMode;
            TDLevel levelData = ExcelMgr.TDLevel.Get(LevelId);
            CurLevelId = LevelId;
            CurScene = levelData.map;
            LevelName = levelData.name;
            BattleSum = levelData.battle.Count;
            SceneBlock = levelData.sceneBlock.ToArray();
            Born = levelData.born.ToArray();
            Reward = levelData.reward.ToArray();
            EnemyId = new string[BattleSum];
            MusicId = new string[BattleSum];
            BattleId = new string[BattleSum];
            for (int i = 0; i < BattleSum; i++)
            {
                var battle = ExcelMgr.TDBattle.Get(levelData.battle[i]);
                EnemyId[i] = battle.monsterid;
                MusicId[i] = battle.musicid;
                BattleId[i] = battle.battle;
            }
            Global.ExcelMgr.InitTDActionFlow(BattleId ,false);
            InitCommon();
            InitBattle();
        }

        public static void InitTrainMode(TDTraining tdTraining, string musicId ,string scene)
        {
            CurBattleIndex = 0;
            CurTrainId = tdTraining.Id;
            CurScene = scene;
            LevelName = tdTraining.name;
            GameMode = GameMode.TrainMode;
            BattleSum = 1;
            SceneBlock = new[] {"UniqueBlock"};
            Born = new[] {0};
            EnemyId = new[] {tdTraining.monster};
            MusicId = new []{musicId};
            BattleId = new []{tdTraining.battleId};
            Global.ExcelMgr.InitTDActionFlow(new []{tdTraining.battleId} ,false);
            InitCommon();
            InitBattle();
        }

        public static void InitBattle()
        {
            InitMoster();
            /*InitMusic();*/
        }

        public static void InitMoster()
        {
            TDMonster monsterData = ExcelMgr.TDMonster.Get(EnemyId[CurBattleIndex]);
            EnemyName = monsterData.name;
        }

        public static void InitMusic()
        {
            TDMusic tdMusic = ExcelMgr.TDMusic.Get(MusicId[CurBattleIndex]);
            beatTime = 60f / tdMusic.Bpm;
            AnimationSpeed = 135.0f / tdMusic.Bpm;
            delayTime = tdMusic.FirstBeat / 1000;
        }

        static void InitCommon()
        {
            PlayerHp = Int32.Parse(ExcelMgr.TDGlobal.Get("1007").prop1);
            PlayerAttack = Int32.Parse(ExcelMgr.TDGlobal.Get("1009").prop1);
            MissCalorie = float.Parse(ExcelMgr.TDGlobal.Get("1014").prop1);
            speed = Int32.Parse(ExcelMgr.TDGlobal.Get("1012").prop1);
            Calorie = Int32.Parse(ExcelMgr.TDGlobal.Get("1013").prop1); 
            StopTime = (float)Int32.Parse(ExcelMgr.TDGlobal.Get("1015").prop1)/1000;
            FullEnergy = Int32.Parse(ExcelMgr.TDGlobal.Get("1017").prop1);
            ComboTime = Int32.Parse(ExcelMgr.TDGlobal.Get("1018").prop1);
            RunTimePerBlock = float.Parse(ExcelMgr.TDGlobal.Get("1022").prop1);
            PlayerRunMusic = ExcelMgr.TDGlobal.Get("1023").prop1;
        }

        public static void InitTDActionFlow()
        {
            preTime = ExcelMgr.TDActionFlow[CurBattleIndex].Get(0).sensationTime;
        }

    }
}