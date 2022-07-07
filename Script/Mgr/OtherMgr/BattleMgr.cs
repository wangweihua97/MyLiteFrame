using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody;
using Coach;
using DG.Tweening;
using Enemy;
using Events;
using Player;
using Script.Excel.Table;
using Script.Game;
using Script.Game.BattleScene;
using Script.Main;
using Script.Main.Base;
using Script.Mgr;
using Script.Model;
using Script.Scene.Game;
using UnityEngine;

namespace OtherMgr
{
    public class BattleMgr : BaseGameFlow
    {
        //public GameObject camera;
        public Transform BattleBase;
        public Transform BattleBasePlayerTransform;
        //public Transform cameraBirth;

        [Header("该场景所有地块")]
        public List<SceneBlock> ListSceneBlocks;
        [Header("初始跑步时间")]
        public float StartRunTime = 3f;

        [HideInInspector]
        public Transform mPlayer;
        //public Transform mCamera;

        public Animator viectoryAnimation;
        public static BattleMgr instance;

        private bool isInitPositon;
        private static bool isInitCharacter;
        private List<Transform> mMonstersTransform;
        private List<EnemyMgr> mMonstersComponent;
        private SceneBlockHelper _sceneBlockHelper;

        private void Awake()
        {
            instance = this;
            isInitCharacter = false;
            isInitPositon = false;
            mMonstersTransform = new List<Transform>();
            mMonstersComponent = new List<EnemyMgr>();
            _sceneBlockHelper = new SceneBlockHelper();
        }
        
        protected override void DoUpdata()
        {
            base.DoUpdata();
            if(!isInitCharacter || !isInitPositon)
                return;
            _sceneBlockHelper.DoUpdata();
        }
        
        protected override void LoadPrepareData80Per()
        {
            base.LoadPrepareData80Per();
            _sceneBlockHelper.InitBlocks();
            InitPosition();
        }
        
        protected override void EnterNewScene()
        {
            base.EnterNewScene();
            if(GameVariable.GameMode == GameMode.LevelMode)
                LevelMode();
            else
                TrainMode();
        }

        
        public void PlayEndAnimation()
        {
            viectoryAnimation.Play("Camera_Victory");
        }

        /// <summary>
        /// 初始化战场中的角色，包括玩家角色和场景中所用到怪物
        /// </summary>
        public void InitBattle(GameFlowEvent attach)
        {
            List<string> charactersPath = new List<string>();
            charactersPath.Add("M_Player");
            for (int i = 0; i < GameVariable.EnemyId.Length; i++)
            {
                string monsterPath = "MonsterCharacter/" + ExcelMgr.TDMonster.Get(GameVariable.EnemyId[i]).map + ".prefab";
                charactersPath.Add(monsterPath);
            }
            PoolManager.CharacterPool.poolCreatMultiGoHelper.AddGameObject(charactersPath,attach, () =>
            {
                isInitCharacter = true;
            });
        }

        

        /// <summary>
        /// 初始化玩家角色和教练的位置
        /// </summary>
        private void InitPosition()
        {
            isInitPositon = true;
            mPlayer = PoolManager.CharacterPool.Spawn("M_Player").transform;
            PlayerMgr.instance = mPlayer.gameObject.GetComponentInChildren<PlayerMgr>();
            PlayerMgr.instance.InitPlayer();
            mPlayer.transform.SetParent(BattleBasePlayerTransform);
            mPlayer.localPosition = Vector3.zero;
            mPlayer.localRotation = Quaternion.Euler(Vector3.zero);
            PlayerMgr.instance.SetMoveTransform(BattleBase);
            if (GameVariable.GameMode == GameMode.LevelMode)
            {
                PlayerMgr.instance.SetPosition(new Vector3(2 ,PlayerMgr.instance.GetTransform().position.y ,PlayerMgr.instance.GetTransform().position.z));
                PlayerMgr.instance.SetRotation(_sceneBlockHelper.GetFirstBlock().BattleShift.PlayerPoint.rotation);
            }

            GameObject Coach = PoolManager.CharacterPool.Spawn("M_Player");
            PlayerMgr.CoachInstance = Coach.GetComponentInChildren<PlayerMgr>();
            PlayerMgr.CoachInstance.InitCoach();
            Coach.transform.SetParent(CoachMgr.instance.BrithTransform);
            Coach.transform.localPosition = Vector3.zero;
            Coach.transform.localRotation = Quaternion.Euler(Vector3.zero);

            switch (GameVariable.GameMode)
            {
               case GameMode.TrainMode:
                   InitMonster();
                   break;
               case GameMode.LevelMode:
                   InitMonster();
                   break;
            }
        }
        
        void InitMonster()
        {
            for (int i = 0; i < GameVariable.EnemyId.Length; i++)
            {
                TDMonster tdMonster = ExcelMgr.TDMonster.Get(GameVariable.EnemyId[i]);
                string monsterPath = "MonsterCharacter/" + tdMonster.map + ".prefab";
                Transform monsterTransform = PoolManager.CharacterPool.Spawn(monsterPath).transform;
                monsterTransform.position = _sceneBlockHelper.GetBattleBlockMonsterPosition(i) + new Vector3(tdMonster.deviation,0,0);
                monsterTransform.rotation = _sceneBlockHelper.GetBattleBlockMonsterRotation(i);
                mMonstersTransform.Add(monsterTransform);
                EnemyMgr enemyMgr = monsterTransform.GetComponentInChildren<EnemyMgr>();
                mMonstersComponent.Add(enemyMgr);
                enemyMgr.WaitBorn();
            }
        }

        void TrainMode()
        {
            mMonstersComponent[0].Born();
            EnemyMgr.instance = mMonstersComponent[GameVariable.CurBattleIndex];
            EnemyMgr.instance.WakeUp();
            WaitTimeMgr.WaitTime(1f, () =>
            {
                EnemyMgr.instance.PlayShow();
            });
            MainGameCtr.instance.ReInit();
        }
        
        async void LevelMode()
        {
            AudioManager.PlayBackground(ExcelMgr.TDMusic.Get(GameVariable.PlayerRunMusic).FolderName);
            AudioManager.BackgroundVolumeAscending(1f ,null);
            Vector3 next = _sceneBlockHelper.GetBattleBlockPlayerFightPosition(GameVariable.CurBattleIndex);
            float time = StartRunTime + (GameVariable.Born[0] - 1) * GameVariable.RunTimePerBlock;
            PlayerMgr.instance.DoMove(next, time, () =>
            {
                IntoBeginStory();
            });
            await Task.Delay(TimeSpan.FromSeconds(1f));
            mMonstersComponent[0].Born();
        }

        
        public async void InterPlayerRunNode(float time)
        {
            mMonstersComponent[GameVariable.CurBattleIndex].Born();
            Vector3 next = _sceneBlockHelper.GetBattleBlockPlayerFightPosition(GameVariable.CurBattleIndex);
            PlayerMgr.instance.DoMove(next, time , () =>
            {
                IntoBeginStory();
            });
            WaitTimeMgr.WaitTime(2f, () =>
            {
                IntoMoveStory();
            });
        }

        void IntoMoveStory()
        {
            if (StoryMgr.CheckStoryOnly(CheckStoryStep.MoveBattle))
            {
                StoryMgr.CheckStory(CheckStoryStep.MoveBattle, (story =>
                {

                }));
            }
        }

        void IntoBeginStory()
        {
            EnemyMgr.instance = mMonstersComponent[GameVariable.CurBattleIndex];
            EnemyMgr.instance.WakeUp();
            if (StoryMgr.CheckStoryOnly(CheckStoryStep.BeforeBattle))
            {
                StoryMgr.CheckStory(CheckStoryStep.BeforeBattle, (story =>
                {
                    MainGameCtr.instance.ReInit();
                }));
            }
            else
            {
                WaitTimeMgr.WaitTime(1f, () =>
                {
                    EnemyMgr.instance.PlayShow();
                });
                MainGameCtr.instance.ReInit();
            }
        }

        public void IntoEndStory()
        {
            if (StoryMgr.CheckStoryOnly(CheckStoryStep.AfterBattle) && GameVariable.GameMode != GameMode.TrainMode)
            {
                StoryMgr.CheckStory(CheckStoryStep.AfterBattle, (story =>
                {
                    MainGameCtr.instance.CurMonsterFightEnd();
                }));
            }
            else
            {
                MainGameCtr.instance.CurMonsterFightEnd();
            }
        }

        
        
        protected override void DoDestroy()
        {
            base.DoDestroy();
        }
    }
}