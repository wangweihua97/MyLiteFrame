using System;
using System.Collections;
using Script.Main;
using Script.Model;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StartView: UView
    {
        public override bool DefaultShow => true;
        public Text levelName;
        public override void DoCreat()
        {
            base.DoCreat();
            GameUIMgr.StartView = this;
            levelName.text = GameVariable.LevelName;
            StartCoroutine(StarGame());
        }

        IEnumerator StarGame()
        {
            yield return new WaitForSeconds(2f);
            Show(false);
            GameUIMgr.GameMainView.Show(true);
            
        }

        public override void DoDestory()
        {
            base.DoDestory();
        }
    }
}