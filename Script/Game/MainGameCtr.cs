﻿using System;
using System.Collections;
 using System.Runtime.InteropServices;
 using System.Threading.Tasks;
 using Enemy;
 using Events;
 using Ghost;
 using OtherMgr;
 using Player;
 using Script.Main;
 using Script.Mgr;
 using Script.Model;
 using UI;
using UnityEngine;
namespace Script.Game
{
    public class MainGameCtr
    {
        public static MainGameCtr instance;
        public MovingBelt movingBelt;
        public bool isStop { get; private set; }
        public GradeHelper GradeHelper = new GradeHelper();
        public ComboHelper ComboHelper = new ComboHelper();
        public ActionFlowModel ActionFlowModel = new ActionFlowModel();
        public AnimationHelper AnimationHelper = new AnimationHelper();
        public VoiceTipsHelper VoiceTipsHelper = new VoiceTipsHelper();
        
        private ItemDropHelper _ItemDropHelper = new ItemDropHelper();
        private RecordHelper _recordHelper;
        private BTMain _btMain;
        public bool IsComBo;
        public bool isLife;
        public bool bodyIsLeft;
        public float gameTime;

        private int _formatTime;
        private bool _isExecuteFinalFlow;

        public void PreInit()
        {
            InitVariable();
            AnimationHelper.Init();
            GradeHelper.Init();
            ComboHelper.Init();
            VoiceTipsHelper.Init();
            InitEvent();
            AudioManager.StopPlayAudioBackGround();
        }
        
        public void ReInit()
        {
            isLife = true;
            isStop = false;
            GameUIMgr.GameMainView.DoOpen();
            GameVariable.InitMusic();
            AudioManager.PlayBackground(ExcelMgr.TDMusic.Get(GameVariable.MusicId[GameVariable.CurBattleIndex]).FolderName);
            AudioManager.BackgroundVolumeAscending(2f ,null);
            ActionFlowModel.Init(gameTime);
        }

        void InitVariable()
        {
            bodyIsLeft = true;
            _isExecuteFinalFlow = false;
            gameTime = 0;
            _formatTime = 0;
            _btMain = new BTMain();
            _recordHelper = new RecordHelper();
            _btMain.Init();
            instance = this;
            isStop = true;
            movingBelt = GameUIMgr.GameMainView.movingBelt;
            GameVariable.GameState = GameState.Gaming;
        }

        void InitEvent()
        {
            EventCenter.ins.AddEventListener<KeyCode>("KeyDown",KeyDown);
            EventCenter.ins.AddEventListener("FinalFlow",FinalFlow);
            EventCenter.ins.AddEventListener("HpZero",HpZero);
            EventCenter.ins.AddEventListener("EnemyDie",EnemyDie);
            EventCenter.ins.AddEventListener<ActionModel>("CreatActionFlow",CreatActionFlow);
            
        }

        void ClearEvent()
        {
            EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
            EventCenter.ins.RemoveEventListener("FinalFlow",FinalFlow);
            EventCenter.ins.RemoveEventListener("HpZero",HpZero);
            EventCenter.ins.RemoveEventListener<ActionModel>("CreatActionFlow",CreatActionFlow);
            EventCenter.ins.RemoveEventListener("EnemyDie",EnemyDie);
        }

        public void OnClose()
        {
            ActionFlowModel.Clear();
            AnimationHelper.Clear();
            GradeHelper.Clear();
            ComboHelper.Clear();
            VoiceTipsHelper.Clear();
            ClearEvent();
        }
        #region 事件处理
        void KeyDown(KeyCode keyCode)
        {
            _btMain.SetValue("KeyDown",keyCode.ToString());    
            _btMain.execute();
        }

        void FinalFlow()
        {
            _isExecuteFinalFlow = true;
            isStop = true;
            AudioManager.BackgroundVolumeDecline(2f ,null);
            WaitTimeMgr.WaitTime(GameVariable.beatTime, () =>
            {
                if (!_isExecuteFinalFlow)
                    return;
                if(EnemyMgr.instance.IsLived)
                    Global.instance.StartCoroutine(WaitEndGame( GameState.Failure ,2f));
                else
                {
                    EnemyMgr.instance.PlayDissolve();
                    AudioManager.BackgroundVolumeDecline(2f , () =>
                    {
                        AudioManager.PlayBackground(ExcelMgr.TDMusic.Get(GameVariable.PlayerRunMusic).FolderName);
                        AudioManager.BackgroundVolumeAscending(1f ,null);
                    });
                    CloseUI();
                    BattleMgr.instance.IntoEndStory();
                }
            });
        }

        void HpZero()
        {
            //_isExecuteFinalFlow = false;
            isStop = true;
            WaitTimeMgr.WaitTime(0.5f, () =>
                {
                    Global.instance.StartCoroutine(WaitEndGame( GameState.Failure ,2f));
                }
            );
        }
        
        void EnemyDie()
        {
            EnemyMgr.instance.Die();
            //_isExecuteFinalFlow = false;
            //isStop = true;
            /*AudioManager.BackgroundVolumeDecline(2f , () =>
            {
                AudioManager.PlayBackground(ExcelMgr.TDMusic.Get(GameVariable.PlayerRunMusic).FolderName);
                AudioManager.BackgroundVolumeAscending(1f ,null);
            });*/
            //GameUIMgr.GameMainView.DoClose();
            //BattleMgr.instance.IntoEndStory();
        }

        public async Task CurMonsterFightEnd()
        {
            GameVariable.CurBattleIndex++;
            await _ItemDropHelper.DropRewards();
            if (GameVariable.CurBattleIndex >= GameVariable.BattleSum)
            {
                GameVariable.CurBattleIndex--;
                Global.instance.StartCoroutine(WaitEndGame( GameState.Succeed ,2f));
            }
            else
            { 
                WaitTimeMgr.WaitTime(2f, () =>
                    {
                        BattleMgr.instance.InterPlayerRunNode((GameVariable.Born[GameVariable.CurBattleIndex] - GameVariable.Born[GameVariable.CurBattleIndex - 1]) * GameVariable.RunTimePerBlock);
                    }
                );
            }
        }

        void CreatActionFlow(ActionModel actionModel)
        {
            AnimationHelper.AddTask(actionModel.animationName ,GameVariable.BirthToClickTime + actionModel.offeTime - actionModel.BTime);
            if (actionModel.ActionClass == ActionClass.Defense || actionModel.ActionClass == ActionClass.Dodge)
                WaitTimeMgr.WaitTime(GameVariable.BirthToClickTime + actionModel.offeTime ,() =>
                {
                    EnemyMgr.instance.Play(EnemyAnimation.Attack);
                });
            if (actionModel.acitonLocat == AcitonLocat.Left)
            {
                movingBelt.belt1.DoEnQueue(actionModel);
            }
            else if(actionModel.acitonLocat == AcitonLocat.Right)
            {
                movingBelt.belt2.DoEnQueue(actionModel);
            }
            else
            {
                movingBelt.belt1.DoEnQueue(actionModel);
                movingBelt.belt2.DoEnQueue(actionModel);
            }
        }
        #endregion


        IEnumerator WaitEndGame(GameState state,float time)
        {
            if(!isLife)
                yield break;
            StopMoveBelt();
            CloseUI();
            GameVariable.GameState = state;
            yield return new WaitForSeconds(time);
            BattleMgr.instance.PlayEndAnimation();
            if (state == GameState.Succeed)
                PlayerMgr.instance.playerAnimationMgr.PlayVictory();
            else
                PlayerMgr.instance.playerAnimationMgr.PlayLose();
            yield return new WaitForSeconds(1.5f);
            EndGame(state);
            yield return new WaitForSeconds(1f);
            AudioManager.PlayBackground("结算界面背景音");
        }
        
        public void EndGame(GameState state)
        {
            GradeHelper.EndGame();
            _recordHelper.RecordTheBattle(state);
            SettlementUIMgr.SettlementView.Show(true);
        }

        void CloseUI()
        {
            GameUIMgr.GameMainView.DoClose();
        }

        //停止游戏两边传送带
        void StopMoveBelt()
        {
            isLife = false;
            SetBeltLife(false);
        }

        public void AtOnceEndGame()
        {
            if(!isLife)
                return ;
            if(Time.timeScale == 0)
                Time.timeScale = 1;
            StopMoveBelt();
            Global.SceneMgr.StartScene("HallScene" ,LoadingViewMgr.LoadingView);
        }

        public void StopGame()
        {
            GameVariable.GameState = GameState.Pause;
            isStop = true;
            AudioManager.PausePlayAudioBackGround();
        }

        public void IntoStory()
        {
            GameVariable.GameState = GameState.Story;
            isStop = true;
            AudioManager.PausePlayAudioBackGround();
        }

        /*IEnumerator SetPauseCoroutine()
        {
            yield return -1;
            Time.timeScale = 0;
            AudioManager.PausePlayAudioBackGround();
        }*/

        public void BackGame()
        {
            GameVariable.GameState = GameState.Gaming;
            isStop = false;
            AudioManager.ContinuePlayAudioBackGround(0);
            AudioManager.BackgroundVolumeAscending(2f ,null);
        }

        public void OnUpdate()
        {
            if(!isLife || isStop)
                return;
            gameTime += Time.deltaTime;
            UIShowTime();
            movingBelt.DoUpdate();
            ActionFlowModel.OnUpdate();
            AnimationHelper.OnUpdate();
            ComboHelper.DoUpdate();
        }

        //用于跟新ui界面的时间
        void UIShowTime()
        {
            int t = (int)gameTime;
            if (t > _formatTime)
            {
                _formatTime = t;
                GameUIMgr.GameMainView.ShowTime(_formatTime);
            }
        }
        
        void SetBeltLife(bool life)
        {
            movingBelt.belt1.isLife = life;
            movingBelt.belt2.isLife = life;
        }

        public void ClickCheck(BeltType type ,ActionType actionType ,bool isRepeat = false)
        {
            if(isStop)
                return;
            BeltItem beltItem = type == BeltType.Left ? movingBelt.belt1 : movingBelt.belt2;
            ActionItem actionItem = beltItem.GetCanClickItem();
            if (actionItem == null)
                return;
            if (actionItem.rectTransform.localPosition.y - GameVariable.CenterY > GameVariable.Radius)
            {
                beltItem.AddIndex();
                ClickCheck(type, actionType, isRepeat);
                return;
            }
            float distance = Math.Abs(actionItem.rectTransform.localPosition.y - GameVariable.CenterY);
            if(distance > GameVariable.Radius)
                return;
            
            if (actionItem.actionModel.acitonLocat == AcitonLocat.All && type == BeltType.Left && isRepeat == false)
            {
                ClickCheck(BeltType.Right ,actionType ,true);
            }
            else if(actionItem.actionModel.acitonLocat == AcitonLocat.All && type == BeltType.Right && isRepeat == false)
            {
                ClickCheck(BeltType.Left ,actionType ,true);
            }
            GradeHelper.Strike(beltItem ,actionItem ,distance);
        }

        public async void DropCoin()
        {
            await _ItemDropHelper.DropOneCoin();
        }

        public void OnDestroy()
        {
            OnClose();
            _btMain.OnDestory();
            instance = null;
        }
    }
}