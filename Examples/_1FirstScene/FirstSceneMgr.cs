using Script.Excel.Table;
using Script.Main;
using Script.Main.Base;
using Script.Mgr;
using Script.Model;
using Script.Scene;
using UnityEngine;

namespace Examples.FirstScene
{
    public class FirstSceneMgr : BaseSceneMgr
    {
            protected override void DoEnable()
            {
                base.DoEnable();
                AddScene<FirstScene>(new FirstScene(),"FirstScene");
            }
    }
}