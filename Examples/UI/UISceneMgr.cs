using Script.Excel.Table;
using Script.Main;
using Script.Main.Base;
using Script.Mgr;
using Script.Model;
using Script.Scene;
using UnityEngine;

namespace Examples.UI
{
    public class UISceneMgr : BaseSceneMgr
    {
            protected override void DoEnable()
            {
                base.DoEnable();
                AddScene<UIScene>(new UIScene(),"UIScene");
            }
    }
}