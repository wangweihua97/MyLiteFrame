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