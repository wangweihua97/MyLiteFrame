using System.Collections.Generic;
using System.Linq;
using Enemy;
using Events;
using OtherMgr;
using Player;
using Script.Excel.Table;
using Script.Game;
using Script.Item;
using Script.Main;
using Script.Mgr;
using Script.Model;
using Script.Scene.Base;
using Script.Tool.PoolManager;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Int32 = System.Int32;

namespace Script.Scene
{
    public class GameScene: IScene
    {
        public static MainGameCtr MainGameCtr;
        public string Name => "GameScene";
        private List<AsyncOperationHandle> _handles = new List<AsyncOperationHandle>();
        public void OnUpdate()
        {
            MainGameCtr?.OnUpdate();
        }

        public void OnBattleUnLoad()
        {
            
        }

        public void OnBattleUnLoaded()
        {
        }

        public void LoadPrepareData70Per()
        {
            BattleMgr.instance.InitBattle(GameFlowMgr.LoadPrepareData70Per);
            PlayerActionMgr.Init();
        }

        public void LoadPrepareData80Per()
        {
            RootUIMgr.instance.CreatUIMgr<GameUIMgr>(true);
            RootUIMgr.instance.CreatUIMgr<SettlementUIMgr>();
            LoadPoolPrefab();
            LoadEffect();
            if(GameVariable.GameMode == GameMode.LevelMode)
                LoadItem(GameFlowMgr.LoadPrepareData80Per);
            AttackEffectMgr.InitSkillEffect(GameFlowMgr.LoadPrepareData80Per);
            LoadBattleInfo();
            EnemyLoadingEffect.Init(GameFlowMgr.LoadPrepareData80Per);
        }

        public void LoadPrepareData90Per()
        {
            if(MainGameCtr == null)
                MainGameCtr = new MainGameCtr();
            MainGameCtr.PreInit();
        }

        public void OnBattleLoaded()
        {
        }

        public void OnUnLoadData()
        {
            RootUIMgr.instance.DestroyUIMgr<GameUIMgr>();
            RootUIMgr.instance.DestroyUIMgr<SettlementUIMgr>();
            MainGameCtr.OnDestroy();
            PoolManager.GameObjPool.Clear();
            PoolManager.EffectPool.Clear();
            PoolManager.CharacterPool.Clear();
            Release();
        }
        
        public void EnterNewScene()
        {
            ;
        }

        void LoadPoolPrefab()
        {
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            PoolManager.GameObjPool.CreatPool("Prefab/ActionItem","", obj =>
            {
                gameFlowTaskGroup.CompleteATask();
            });
            SubObjPool subObjPool = PoolManager.GameObjPool.GetPool("Prefab/ActionItem") as SubObjPool;
            gameFlowTaskGroup.Add(subObjPool.Handle);
            gameFlowTaskGroup.Attach(GameFlowMgr.LoadPrepareData80Per);
        }
        

        void LoadItem(GameFlowEvent attach)
        {
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            PoolManager.CommonPool.CreatPool("Item/ItemShow","", obj =>
            {
                gameFlowTaskGroup.CompleteATask();
            });
            SubObjPool itemShow = PoolManager.CommonPool.GetPool("Item/ItemShow") as SubObjPool;
            gameFlowTaskGroup.Add(itemShow.Handle);
            gameFlowTaskGroup.Attach(attach);
            
            HashSet<string> paths = new HashSet<string>();
            foreach (var rewardId in GameVariable.Reward)
            {
                TDReward tdReward = ExcelMgr.TDReward.Get(rewardId);
                foreach (var kvp in tdReward.rewardMap)
                {
                    if (!paths.Contains(kvp.Key))
                        paths.Add(kvp.Key);
                }
            }
            var modelPaths = from path in paths select "Item/" + ExcelMgr.TDItem.Get(path).model;
            ItemLoadMgr.Instance.LoadModels(modelPaths.ToArray() ,attach);
        }

        void LoadEffect()
        {
            var handle = AddressablesHelper.instance.LoadAssets<Texture>(
                "Assets/Effect_Sources/Sources/Commontexture/Texture1/wenli007.tga");
            handle.Completed += (obj) =>
            {
                EnemyMgr.DissolveTexture = obj.Result;
            };
            _handles.Add(handle);
        }

        void LoadBattleInfo()
        {
            GameVariable.EnemyHp = new int[GameVariable.BattleId.Length];
            float hpCoefficient = float.Parse(ExcelMgr.TDGlobal.Get("1020").prop1);
            float attackCoefficient = float.Parse(ExcelMgr.TDGlobal.Get("1021").prop1);
            int attackCount = 0;
            int i = 0;
            foreach (var tdActionFlow in ExcelMgr.TDActionFlow)
            {
                int sumHP = 0;
                foreach (var kvp in tdActionFlow.GetDictionary())
                {
                    var data = kvp.Value;
                    if (data.left != Const.NULL_STRING || data.right != Const.NULL_STRING || data.all != Const.NULL_STRING)
                    {
                        TDAction actionData = PlayerActionMgr.GetActionData(data.body, data.left, data.right, data.all);
                        if (actionData.actionType == 1)
                            sumHP += GameVariable.PlayerAttack;
                        if (actionData.actionType == 2)
                            attackCount++;
                    }
                }
                GameVariable.EnemyHp[i] = (int)(sumHP * hpCoefficient);
                i++;
            }
            GameVariable.EnemyAttack = (int)(attackCoefficient * GameVariable.PlayerHp / attackCount);
        }

        void Release()
        {
            foreach (var handle in _handles)
            {
                AddressablesHelper.instance.Release(handle);
            }
        }
    }
}