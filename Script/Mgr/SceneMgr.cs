using Script.Excel.Table;
using Script.Main;
using Script.Main.Base;
using Script.Model;
using Script.Scene;
using UnityEngine;

namespace Script.Mgr
{
    public class SceneMgr : BaseSceneMgr
    {
        protected override void DoEnable()
        {
            base.DoEnable();
            AddScene<HallScene>(new HallScene(),"HallScene");
            AddScene<TestScene>(new TestScene(),"TestScene");
            AddScene<GameScene>(new GameScene(),"GameScene");
            AddScene<LogoScene>(new LogoScene(),"LogoScene");
        }

        public void EnterGameScene()
        {
            if (!ExcelMgr.TDBattleScene.ContainsKey(GameVariable.CurScene))
            {
                Debug.LogError("Battle表中没有找到Id");
                return;
            }
            TDBattleScene tdBattle = ExcelMgr.TDBattleScene.Get(GameVariable.CurScene);
            string sceneKey = tdBattle.SceneName;
            if (!ContainsKey(sceneKey))
            {
                AddScene(GetScene("GameScene"),sceneKey);
            }
            StartScene(sceneKey, LoadingViewMgr.LoadingView); 
        }
    }
}