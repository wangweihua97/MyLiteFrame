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
        
        protected override void DoEnable()
        {
            base.DoEnable();
        }

        public void InitTDActionFlow(string[] levelId ,bool addPrefix = true)
        {
           
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