using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Events;
using Script.Excel;
using Script.Excel.Table;
using Script.Mgr;
using Script.Model;
using Script.Scene.Base;

namespace Examples.ProgressBar
{
    public class ProgressBarSceneA: IScene
    {
            public string Name => "FirstScene";
            public void OnUpdate()
            {
            }

            public void OnBattleUnLoad()
            {
            }

            public void OnBattleUnLoaded()
            {
            }

            public void OnBattleLoad()
            {
            }

            public void LoadPrepareData70Per()
            {
                GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup(10);
                gameFlowTaskGroup.Attach(GameFlowMgr.LoadPrepareData80Per);
                GameFlowTaskGroup(gameFlowTaskGroup);
            }

            async void GameFlowTaskGroup(GameFlowTaskGroup gameFlowTaskGroup)
            {
                for (int i = 0; i < 10; i++)
                {
                    await Task.Delay(TimeSpan.FromSeconds(0.2f));
                    gameFlowTaskGroup.CompleteATask();
                }
            }

            public void LoadPrepareData80Per()
            {
            }

            public void LoadPrepareData90Per()
            {
            }

            public void OnBattleLoaded()
            {
            }

            public void OnUnLoadData()
            {
            
            }
            public void EnterNewScene()
            {
            }
        
    }
}