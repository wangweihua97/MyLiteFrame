using System;
using System.Collections;
using System.Globalization;
using Events;
using Ghost;
using OtherMgr;
using Script.Game;
using Script.Main;
using Script.Mgr;
using Script.Model;
using UI.Base;
using UI.TrainMode;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameMainView : UView
    {
        public override bool DefaultShow => false;
        public MovingBelt movingBelt;
        public Text ScoreTipText;
        public Text gradeScore;
        public Text time;
        public Text levelName;
        
        
        public EnemyHPProgressBar EnemyHpProgressBar;
        public HPProgressBar HpProgressBar;
        public PowerProgressBar PowerProgressBar;
        public GradeLevelView gradeGO;
        public TipCom tipCom;
        public Text monsterName;
        public ComboCom ComboCom;
        public PostureItem PostureItem;
        [Component] public CoachComponent CoachComponent;

        private int ScoreTipNum;
        public override void DoCreat()
        {
            base.DoCreat();
            InitVariable();
            movingBelt.Init();
            EventCenter.ins.AddEventListener<TipModel>("PlayTip",PlayTip);
            EventCenter.ins.AddEventListener("ChangeBody",ChangeBody);
            EventCenter.ins.AddEventListener("MonsterChange",MonsterChange);
            //EventCenter.ins.AddEventListener<ScoreTipTextModel>("ShowScoreTipText",ShowScoreTipText);
        }
        
        public void MonsterChange()
        {
            monsterName.text = GameVariable.EnemyName;
        }

        public override void Show(bool show)
        {
            base.Show(show);
        }

        void InitVariable()
        {
            GameUIMgr.GameMainView = this;
            movingBelt.GameMainView = this;
            monsterName.text = GameVariable.EnemyName;
            time.text = "0:00";
            levelName.text = GameVariable.LevelName;
            tipCom.showCount = 0;
        }

        void PlayTip(TipModel tipModel)
        {
            if(!gameObject.activeInHierarchy)
                return;
            if (tipModel.text.Equals("VANISH"))
            {
                TipVanish();
                return;
            }
            tipCom.gameObject.SetActive(true);
            tipCom.showCount++;
            tipCom.SetText(tipModel.text ,tipModel.IsContinuous);
            /*if(tipModel.textSoundRes != Const.NULL_STRING)
                AudioManager.PlayAudioEffectB(tipModel.textSoundRes);*/
            //Debug.Log(Global.TDSoundEffects.Get(tipModel.text).animationID);
            StartCoroutine(PlayTip(3));
        }
        
        IEnumerator PlayTip(float time)
        {
            yield return new WaitForSeconds(time);
            tipCom.showCount--;
            if (tipCom.showCount <= 0)
            {
                tipCom.gameObject.SetActive(false);
            }
        }

        void TipVanish()
        {
            tipCom.gameObject.SetActive(false);
        }

        void ChangeBody()
        {
            PostureItem.SetBodyPosture(MainGameCtr.instance.bodyIsLeft);
        }

        IEnumerator ScoreTipTextMiss(float time)
        {
            yield return new WaitForSeconds(time);
            ScoreTipNum--;
            if(ScoreTipNum <= 0)
                ScoreTipText.gameObject.SetActive(false);
        }

        

        protected override void DoUpdata()
        {
            base.DoUpdata();
        }

        public void ShowTime(int timeValue)
        {
            int m = timeValue / 60;
            int s = timeValue % 60;
            if (s >= 10)
                time.text = m + ":" + s;
            else
                time.text = m + ":0" + s;
        }

        public void SetPower(int power)
        {
            PowerProgressBar.SetValue((float)power / GameVariable.FullEnergy);
        }
        
        public void SetHp(int hp)
        {
            HpProgressBar.SetValue((float)hp/GameVariable.PlayerHp);
        }
        
        public void SetGrade(string grade)
        {
            gradeGO.gameObject.SetActive(true);
            gradeGO.SetGrade(grade);
        }
        
        public void SetScore(int Score)                     
        {
            gradeScore.text = Score.ToString();
        }
        
        public void SetEnemyHP(int HP)
        {
            EnemyHpProgressBar.SetValue((float)HP/GameVariable.EnemyHp[GameVariable.CurBattleIndex]);
        }

        public override void DoClose()
        {
            movingBelt.Close();
            base.DoClose();
        }

        public override void DoDestory()
        {
            EventCenter.ins.RemoveEventListener<TipModel>("PlayTip",PlayTip);
            EventCenter.ins.RemoveEventListener("ChangeBody",ChangeBody);
            EventCenter.ins.RemoveEventListener("MonsterChange",MonsterChange);
            //EventCenter.ins.RemoveEventListener<ScoreTipTextModel>("ShowScoreTipText", ShowScoreTipText);
            base.DoDestory();
        }
    }
}