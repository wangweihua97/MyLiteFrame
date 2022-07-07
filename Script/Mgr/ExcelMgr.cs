using System.Collections.Generic;
using Events;
using Script.Excel;
using Script.Excel.Table;
using Script.Model;
using UnityEngine;

namespace Script.Mgr
{
    public class ExcelMgr : BaseExcelMgr
    {
        public static CsvTable<TestTable> TestTable;
        public static CsvTable<TDActAnimation> TDActAnimation;
        public static CsvTable<TDAction> TDAction;
        public static List<CsvTable<TDActionFlow>> TDActionFlow;
        public static CsvTable<TDBattleScene> TDBattleScene;
        public static CsvTable<TDGlobal> TDGlobal;
        public static CsvTable<TDGlobalNumber> TDGlobalNumber;
        public static CsvTable<TDLevel> TDLevel;
        public static CsvTable<TDMonster> TDMonster;
        public static CsvTable<TDPlayMode> TDPlayMode;
        public static CsvTable<TDSoundEffects> TDSoundEffects;
        public static CsvTable<TDSkill> TDSkill;
        public static CsvTable<TDVoiceTips> TDVoiceTips;
        public static CsvTable<TDTraining> TDTraining;
        public static CsvTable<TDIcon> TDIcon;
        public static CsvTable<TDScene> TDScene;
        public static CsvTable<TDMusic> TDMusic;
        public static CsvTable<TDCharacter> TDCharacter;
        public static CsvTable<TDCameraLocation> TDCameraLocation;
        public static CsvTable<TDSuit> TDSuit;
        public static CsvTable<TDUITpis> TDUITpis;
        public static CsvTable<TDTips> TDTips;
        public static CsvTable<TDItem> TDItem;
        public static CsvTable<TDStory> TDStory;
        public static CsvTable<TDBattle> TDBattle;
        public static CsvTable<TDNPC> TDNPC;
        public static CsvTable<TDReward> TDReward;
        protected override void DoEnable()
        {
            base.DoEnable();
            TDActionFlow = new List<CsvTable<TDActionFlow>>();
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            TestTable = new CsvTable<TestTable>("TestTable" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDActAnimation = new CsvTable<TDActAnimation>("Csv/ActAnimation" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDAction = new CsvTable<TDAction>("Csv/Action" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDBattleScene = new CsvTable<TDBattleScene>("Csv/TDBScene" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDGlobal = new CsvTable<TDGlobal>("Csv/Global" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDLevel = new CsvTable<TDLevel>("Csv/Level" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDMonster = new CsvTable<TDMonster>("Csv/Monster" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDPlayMode = new CsvTable<TDPlayMode>("Csv/PlayMode" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDSoundEffects = new CsvTable<TDSoundEffects>("Csv/SoundEffects" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDSkill = new CsvTable<TDSkill>("Csv/Skill" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDVoiceTips = new CsvTable<TDVoiceTips>("Csv/VoiceTips" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDTraining = new CsvTable<TDTraining>("Csv/TDTraining" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDIcon = new CsvTable<TDIcon>("Csv/Icon" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDScene = new CsvTable<TDScene>("Csv/TDScene" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDMusic = new CsvTable<TDMusic>("Csv/TDMusic" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDCharacter = new CsvTable<TDCharacter>("Csv/TDCharacter" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDCameraLocation = new CsvTable<TDCameraLocation>("Csv/TDCameraLocation" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDSuit = new CsvTable<TDSuit>("Csv/Suit" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDUITpis = new CsvTable<TDUITpis>("Csv/UITpis" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDTips = new CsvTable<TDTips>("Csv/Tips" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDItem = new CsvTable<TDItem>("Csv/TDItem" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDStory = new CsvTable<TDStory>("Csv/Story" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDBattle = new CsvTable<TDBattle>("Csv/TDBattle" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            TDReward = new CsvTable<TDReward>("Csv/TDReward" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            /*TDNPC = new CsvTable<TDNPC>("Csv/TDNPC" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();*/
            gameFlowTaskGroup.Attach(GameFlowMgr.LoadedInitData);
        }

        public void InitTDActionFlow(string[] levelId ,bool addPrefix = true)
        {
            if (TDActionFlow != null)
            {
                foreach (var td in TDActionFlow)
                {
                    td.Clear();
                }
            }
            TDActionFlow = new List<CsvTable<TDActionFlow>>();

            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            Dictionary<string ,CsvTable<TDActionFlow>> temp = new Dictionary<string, CsvTable<TDActionFlow>>();
            foreach (var ids in levelId)
            {
                string tbName = addPrefix ? "Csv/Battle" + ids + ".csv" : "Csv/" + ids + ".csv";
                if (temp.ContainsKey(tbName))
                {
                    TDActionFlow.Add(temp[tbName]);
                    continue;
                }
                CsvTable<TDActionFlow> table = new CsvTable<TDActionFlow>(tbName,
                    () =>
                    {
                        gameFlowTaskGroup.CompleteATask();
                    });
                TDActionFlow.Add(table);
                temp.Add(tbName ,table);
                gameFlowTaskGroup.Add();
            }

            gameFlowTaskGroup.Completed += () =>
            {
                GameVariable.InitTDActionFlow();
            };
            gameFlowTaskGroup.Attach(GameFlowMgr.LoadPrepareData70Per);
        }

        public void LoadStoryDialogue(string url)
        {
            string tbName = "StoryDialog"+url;
            // GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            // TDActionFlow = new CsvTable<TDActionFlow>(tbName,
            //     ()=>
            //     {
            //         GameVariable.InitTDActionFlow();
            //         gameFlowTaskGroup.CompleteATask();
            //     });
            // gameFlowTaskGroup.Add();
            // gameFlowTaskGroup.Attach(GameFlowMgr.LoadPrepareData70Per);
        }
    }
}