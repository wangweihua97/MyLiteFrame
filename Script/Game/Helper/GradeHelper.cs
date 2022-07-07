using System;
using System.Collections;
using Enemy;
using Events;
using Ghost;
using OtherMgr;
using Player;
using Script.Main;
using Script.Mgr;
using Script.Model;
using Tool.Others;
using UI;
using UnityEngine;

namespace Script.Game
{
    public class GradeHelper
    {
        public int curScore; //当前得分
        public int totalScore; //总共得分

        public int powar
        {
            set { PlayerMgr.instance.PlayerEnergy.SetEnergy(value);}
            get { return PlayerMgr.instance.PlayerEnergy.GetEnergy(); }
        } //当前能力条
        public int curHp{
            get { return PlayerMgr.instance.PlayerHp.GetHp(); }
        }//当前生命值

        public int EnemyCurHp{
            set { EnemyMgr.instance.EnemyHp.SetHp(value);}
            get { return EnemyMgr.instance.EnemyHp.GetHp(); }
        } //敌人生命值
        public float curCalorie; //当前卡路里
        
        
        public int veryGoodCount; //很好数量
        public int perfectCount; //完美数量
        public int missCount; //错过数量
        public int maxPerfectTime = 5; //最大完美连击的次数
        public float gameTime { 
            get { return MainGameCtr.instance.gameTime; }
            private set { ; }
        } //游戏时间
        
        public int actNumber; //出拳数
        public GradeType gradeType; //玩家评级
        public int bodyAge
        {
            get { return GetAge(); }
        } //身体年龄

        private int curPerfectCount;
        private int S;
        private int A;
        private int B;
        private int C;
        
        public void Init()
        {
            InitVariable();
            EventCenter.ins.AddEventListener<ActionItem>("ActionMiss",ActionMiss);
            EventCenter.ins.AddEventListener("MonsterChange",MonsterChange);
            RefreshScore();
        }

        public void MonsterChange()
        {
            //EnemyMgr.instance.EnemyHp.Init();
        }

        public void EndGame()
        {
        }

        void InitVariable()
        {
            PlayerMgr.instance.PlayerEnergy.Init();
            PlayerMgr.instance.PlayerHp.Init();
            
            curScore = 0;
            totalScore = 0;
            curCalorie = 0;
            veryGoodCount = 0;
            perfectCount = 0;
            missCount = 0;
            maxPerfectTime = 0;
            curPerfectCount = 0;
            actNumber = 0;

            switch (GameVariable.GameMode)
            {
                case GameMode.LevelMode:
                    S = ExcelMgr.TDLevel.Get(GameVariable.CurLevelId).S;
                    A = ExcelMgr.TDLevel.Get(GameVariable.CurLevelId).A;
                    B = ExcelMgr.TDLevel.Get(GameVariable.CurLevelId).B;
                    C = ExcelMgr.TDLevel.Get(GameVariable.CurLevelId).C;
                    break;
                default:
                    S = ExcelMgr.TDTraining.Get(GameVariable.CurTrainId).S;
                    A = ExcelMgr.TDTraining.Get(GameVariable.CurTrainId).A;
                    B = ExcelMgr.TDTraining.Get(GameVariable.CurTrainId).B;
                    C = ExcelMgr.TDTraining.Get(GameVariable.CurTrainId).C;
                    break;
            }
        }

        void ActionMiss(ActionItem actionItem)
        {
            if(actionItem.actionModel.acitonLocat == AcitonLocat.All && !actionItem.parent.isLeft)
            {
                return;
            }
            
            totalScore += actionItem.actionModel.SScore;
            RefreshGrade();
            VoiceTipAndComboPunch(actionItem.actionModel, GradeType.Miss);
            ClearPerfectCount();
            missCount++;
        }
        
        public void Strike(BeltItem beltItem ,ActionItem actionItem ,float gap)
        {
            float prop = 1 - gap / GameVariable.Radius;
            GetPoint(beltItem ,prop, actionItem);
        }

        void GetPoint(BeltItem beltItem ,float prop ,ActionItem actionItem)
        {
            int score;
            GradeType grade;
            float gradeValue;
            ActionModel actionModel = actionItem.actionModel;
            float c = (float)GameVariable.Calorie / 1000;
            bool isGet = true;

            SetGrade(prop, actionModel, out score, out grade, out gradeValue);

            if(grade == GradeType.Miss)
            {
                c = GameVariable.Calorie * GameVariable.MissCalorie;
                actNumber++;
                curCalorie += c/1000;
                return;
            }
            if ( beltItem.isLeft == false && actionModel.acitonLocat == AcitonLocat.All)
            {
                DeletItem(actionItem, grade, beltItem);
                return;
            }
            DropCoin(grade);

            VoiceTipAndComboPunch(actionModel, grade);
            GetReward(isGet ,actionModel.bounsType ,actionModel.bounsProp1 ,actionModel.bounsProp2 ,actionModel.bounsProp3);
            PlayAudio(grade ,actionModel.ActionClass);
            AddPerfectCount(grade);
            actNumber++;
            curCalorie += c;
            RecordScore(score ,actionModel.SScore);
            DeletItem(actionItem, grade, beltItem);
            DoAttackOrDefend(isGet ,grade ,score ,actionModel,gradeValue);
        }

        void SetGrade(float prop, ActionModel actionModel ,out int score,out GradeType grade ,out float gradeValue)
        {
            score = 0;
            grade = GradeType.Miss;
            gradeValue = 0;
            if (prop > actionModel.C / 10000 && MainGameCtr.instance.IsComBo)
                prop = 1;
            
            if (prop > actionModel.S/10000)
            {
                perfectCount++;
                gradeValue = actionModel.S;
                score = actionModel.SScore;
                grade = GradeType.Perfect;
            }
            else if(prop > actionModel.A/10000)
            {
                veryGoodCount++;
                gradeValue = actionModel.A;
                score = actionModel.AScore;
                grade = GradeType.VeryGood;
            }
            else if(prop > actionModel.B/10000)
            {
                gradeValue = actionModel.B;
                score = actionModel.BScore;
                grade = GradeType.Good;
            }
            else if(prop > actionModel.C/10000)
            {
                gradeValue = actionModel.C;
                score = actionModel.CScore;
                grade = GradeType.Correct;
            }
        }

        //播放打击音效
        void PlayAudio(GradeType grade,ActionClass actionClass)
        {
            if (actionClass == ActionClass.Defense)
            {
                if (grade != GradeType.Perfect)
                    AudioManager.PlayAudioEffectA("完美格挡");
                else
                    AudioManager.PlayAudioEffectA("普通格挡");
            }
            else if (actionClass == ActionClass.Dodge)
            {
                if (grade != GradeType.Perfect)
                    AudioManager.PlayAudioEffectA("完美闪避");
                else
                    AudioManager.PlayAudioEffectA("一般闪避");
            }
            else
            {
                if (grade != GradeType.Perfect)
                    AudioManager.PlayAudioEffectA("完美拳");
                else
                    AudioManager.PlayAudioEffectA("一般拳");
            }
        }
        
        //将音节放入缓存池
        void DeletItem(ActionItem actionItem,GradeType grade,BeltItem beltItem)
        {
            actionItem.PlayGrade(grade);
            Global.instance.StartCoroutine(InsetPool(beltItem, actionItem));
        }

        IEnumerator InsetPool(BeltItem beltItem ,ActionItem actionItem)
        {
            actionItem.SetLife(false);
            beltItem.ClickMissing();
            yield return new WaitForSeconds(0.65f);
            actionItem.OnDie();
            PoolManager.GameObjPool.DesSpawn("Prefab/ActionItem",actionItem.gameObject);
        }

        void AddPerfectCount(GradeType gradeType)
        {
            if (gradeType != GradeType.Perfect)
            {
                ClearPerfectCount();
                return;
            }
            curPerfectCount++;
        }

        //出拳评价
        void VoiceTipAndComboPunch(ActionModel actionModel, GradeType grade)
        {
            MainGameCtr.instance.VoiceTipsHelper.VoiceTips(actionModel ,grade);
            MainGameCtr.instance.ComboHelper.PunchInComboState(actionModel ,grade);
        }

        void ClearPerfectCount()
        {
            if (curPerfectCount > maxPerfectTime)
                maxPerfectTime = curPerfectCount;
            curPerfectCount = 0;
        }

        void DoAttackOrDefend(bool isGet, GradeType grade, int score, ActionModel actionModel ,float gradeValue)
        {
            if (actionModel.ActionClass == ActionClass.Defense || actionModel.ActionClass == ActionClass.Dodge)
            {
                if (isGet)
                {
                    PlayerMgr.instance.GetDefenseBuff();
                    //EventCenter.ins.EventTrigger("StrikeGrade",new ScoreTipTextModel(grade ,2f ,actionModel.isLeft));
                }
            }
            else
            {
                PlayerMgr.instance.PlayAttackEffect();
                EnemyMgr.instance.PlayHurtEffect();
                AtackEnemy((int)(GameVariable.PlayerAttack * score / 100));
                //EventCenter.ins.EventTrigger("StrikeGrade",new ScoreTipTextModel(grade ,2f ,actionModel.isLeft));
            }
        }

        void RecordScore(int curScore, int fullScore)
        {
            this.curScore += curScore;
            totalScore += fullScore;
            RefreshScore();
        }
        
        void GetReward(bool isGet,int bounsType, int bounsProp1 ,int bounsProp2,int bounsProp3)
        {
            if(!isGet)
                return;
            if (bounsType == 1)
            {
                GetReward(RewardType.Energy , bounsProp1);
            }
        }

        void GetReward(RewardType type, int value)
        {
            if (type == RewardType.Energy)
            {
                PlayerMgr.instance.PlayerEnergy.AddEnergy(value);
            }
        }

        void AtackEnemy(int hurt)
        {
            if(hurt <= 0)
                return;
            EventCenter.ins.EventTrigger("AttackEnemy",hurt);
            EnemyCurHp -= hurt;
        }

        public void RefreshScore()
        {
            GameUIMgr.GameMainView.SetScore(curScore);
            RefreshGrade();
        }
        
        public void RefreshGrade()
        {
            if(totalScore == 0)
                return;
            string grade = "D";
            gradeType = GradeType.Miss;
            float a = (float)curScore / totalScore;
            if (a > (float)S/10000 )
            {
                gradeType = GradeType.Perfect;
                grade = "S";
            }
            else if(a > (float)A/10000)
            {
                gradeType = GradeType.VeryGood;
                grade = "A";
            }
            else if(a > (float)B/10000)
            {
                gradeType = GradeType.Good;
                grade = "B";
            }
            else if(a > (float)C/10000)
            {
                gradeType = GradeType.Correct;
                grade = "C";
            }
            
            GameUIMgr.GameMainView.SetGrade(grade);
        }

        int GetAge()
        {
            switch (GameVariable.GameMode)
            {
                case GameMode.LevelMode:
                    float age = 18.0f.Lerp(60, 1 - (float)curScore/totalScore );
                    return Convert.ToInt32(age);
                default: 
                    age = 18.0f.Lerp(60, 1 - (float)curScore/totalScore );
                    return Convert.ToInt32(age);
            }
        }

        /// <summary>
        /// 当怪物死亡时，得满分得到一个金币
        /// </summary>
        /// <param name="grade">得分</param>
        void DropCoin(GradeType grade)
        {
            if(grade == GradeType.Perfect && !EnemyMgr.instance.IsLived)
                MainGameCtr.instance.DropCoin();
        }

        public void Clear()
        {
            EventCenter.ins.RemoveEventListener<ActionItem>("ActionMiss",ActionMiss);
            EventCenter.ins.RemoveEventListener("MonsterChange",MonsterChange);
        }
    }
}