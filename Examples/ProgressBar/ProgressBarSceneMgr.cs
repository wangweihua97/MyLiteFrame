using Script.Excel.Table;
using Script.Main;
using Script.Main.Base;
using Script.Mgr;
using Script.Model;
using Script.Scene;
using UnityEngine;

namespace Examples.ProgressBar
{
    public class ProgressBarSceneMgr : BaseSceneMgr
    {
            protected override void DoEnable()
            {
                base.DoEnable();
                AddScene<ProgressBarScene>(new ProgressBarScene(),"ProgressBarScene");
            }
    }
}