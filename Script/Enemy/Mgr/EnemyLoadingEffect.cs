using Events;
using OtherMgr;
using Script.Tool.PoolManager;
using UnityEngine;

namespace Enemy
{
    public static class EnemyLoadingEffect
    {
        /// <summary>
        /// 从bundle中加载怪兽出现特效
        /// </summary>
        /// <param name="attach">需要绑定的流程</param>
        public static void Init(GameFlowEvent attach)
        {
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            PoolManager.EffectPool.CreatPool("Effect/MonsterLoadingEffect","", obj =>
            {
                gameFlowTaskGroup.CompleteATask();
            });
            SubObjPool loadingEffect = PoolManager.EffectPool.GetPool("Effect/MonsterLoadingEffect") as SubObjPool;
            gameFlowTaskGroup.Add(loadingEffect.Handle);
            gameFlowTaskGroup.Attach(attach);
        }

        public static void ShowLoadingEffect(Transform goTransform ,Vector3 offset)
        {
            GameObject loadingEffectGo =
                PoolManager.EffectPool.Spawn("Effect/MonsterLoadingEffect");
            loadingEffectGo.transform.position = goTransform.position + offset;
            WaitTimeMgr.WaitTime(3f, () =>
            {
                PoolManager.EffectPool.DesSpawn(loadingEffectGo);
            });
        }
    }
}