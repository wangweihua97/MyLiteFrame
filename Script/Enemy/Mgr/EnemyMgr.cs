using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Events;
using OtherMgr;
using Player;
using Script.Enemy.Model;
using Script.Excel.Table;
using Script.Main;
using Script.Mgr;
using Script.Model;
using UIModel;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Enemy
{
    public class EnemyMgr : MonoBehaviour
    {
        public static EnemyMgr instance;
        public EnemyAnimationMgr EnemyAnimationMgr;
        private EnemySkillMgr EnemySkillMgr;

        public Transform Punch_Hit_Dummy;
        public Transform Dummy_Hit;
        public EnemyHp EnemyHp;

        public bool IsLived;

        public bool isSpelling;
        public static Texture DissolveTexture;

        private void Awake()
        {
            
        }

        public void WaitBorn()
        {
            ToNormalFromDissolve();
            EnemyAnimationMgr = gameObject.GetComponent<EnemyAnimationMgr>()??gameObject.AddComponent<EnemyAnimationMgr>();
            EnemyAnimationMgr.Init();
            gameObject.SetActive(false);
        }

        public void Attack()
        {
            if(!IsLived)
                return;
            EnemySkillMgr.Attack();
        }

        public void PlayShow()
        {
            EnemyAnimationMgr.PlayShow();
        }

        public async void Born(bool playerEffect = true)
        {
            if (playerEffect)
            {
                await Task.Delay(TimeSpan.FromSeconds(1f));
                EnemyLoadingEffect.ShowLoadingEffect(transform ,Vector3.zero);
                await Task.Delay(TimeSpan.FromSeconds(0.5f));
            }
            gameObject.SetActive(true);
            EnemyAnimationMgr.ResetIdle();
            
        }

        public void WakeUp()
        {
            IsLived = true;
            EnemyHp = new EnemyHp();
            EnemyHp.Init();
            EnemySkillMgr = gameObject.AddComponent<EnemySkillMgr>();
            EventCenter.ins.AddEventListener<int>("AttackEnemy", AttackEnemy);
            GameVariable.InitMoster();
            EventCenter.ins.EventTrigger("MonsterChange");
        }

        public EnemySkillInfo GetAttackSkillInfo()
        {
            return EnemySkillMgr.GetAttackSkillInfo();
        }

        public void Play(EnemyAnimation animation)
        {
            EnemyAnimationMgr.Play(animation);
        }

        public void PlayHurtEffect()
        {
            GameObject hurtEffect = PoolManager.EffectPool.Spawn("HitEffect", Dummy_Hit, Vector3.zero, Vector3.zero);
            hurtEffect.transform.localScale = new Vector3(2f,2f,2f);
            StartCoroutine(RecycleEffect(hurtEffect, "HitEffect", 2f));
        }

        void AttackEnemy(int hurt)
        {
            if (isSpelling)
                return;
            EnemyAnimationMgr.Play(EnemyAnimation.Hurt);
        }

        public IEnumerator RecycleEffect(GameObject effect, string path, float time)
        {
            yield return new WaitForSeconds(time);
            PoolManager.EffectPool.DesSpawn(path, effect);
        }

        public void Die()
        {
            IsLived = false;
            Play(EnemyAnimation.Dead);
        }
        
        private void OnDisable()
        {
            EventCenter.ins.RemoveEventListener<int>("AttackEnemy", AttackEnemy);
        }
        
        public void PlayDeathEffect()
        {
            string fullPath = AttackEffectMgr.GetFullPath("Monster_Diss");
            GameObject deathEffect = PoolManager.EffectPool.Spawn(fullPath, EnemyMgr.instance.transform.parent, Vector3.zero, Vector3.zero);
            StartCoroutine(RecycleEffect(deathEffect, fullPath ,3f));
            Dissolve();
        }

        public void PlayDissolve()
        {
            Dissolve();
        }

        #region 溶解效果
        private List<List<Shader>> recordNormalShader = new List<List<Shader>>();
        private bool dissolveOver = true;
        private bool isStopParticle = false;

        /// <summary>
        /// 溶解效果
        /// </summary>
        /// <param name="duration">持续时长</param>
        /// <param name="stopParticle">是否停播特效</param>
        private void Dissolve(float duration = 2.0f, bool stopParticle = true)
        {
            Shader dissolveShader = Shader.Find("Model/Dissolve");
            if (dissolveShader == null)
                return;

            recordNormalShader.Clear();
            dissolveOver = false;
            isStopParticle = stopParticle;

            SkinnedMeshRenderer[] smr = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            for (int i = 0; i < smr.Length; i++)
            {
                List<Shader> oldShaderLst = new List<Shader>();

                for (int j = 0; j < smr[i].materials.Length; j++)
                {
                    Material mat = smr[i].materials[j];
                    oldShaderLst.Add(mat.shader);
                    mat.shader = dissolveShader;

                    mat.SetColor("_DissolveColor", new Color(40f / 255f, 164f / 255f, 197f / 255f, 1f));
                    mat.SetFloat("_DissolveRandFactor", 0);
                    mat.SetFloat("_DissolveUpDownFactor", 0);
                    mat.SetFloat("_StartFactor", 0.0334f);
                    mat.SetFloat("_ParamFactor", 0);
                    mat.SetFloat("_Illuminate", 20);
                    mat.SetTexture("_ChannelTex", DissolveTexture);
                    mat.DOFloat(1, "_DissolveRandFactor", duration).SetEase(Ease.InCubic).OnComplete(() =>
                    {
                        dissolveOver = true;
                    });
                }

                recordNormalShader.Add(oldShaderLst);
            }

            if(stopParticle)
            {
                ParticleSystem[] ps = gameObject.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < ps.Length; i++)
                {
                    ps[i].Stop();
                }
            }
        }
        
        /// <summary>
        /// 从溶解消失后的状态恢复到正常显示状态
        /// </summary>
        public void ToNormalFromDissolve()
        {
            if (!dissolveOver)
                return;

            SkinnedMeshRenderer[] smr = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            if (recordNormalShader.Count == smr.Length)
            {
                for (int i = 0; i < smr.Length; i++)
                {
                    if (recordNormalShader[i].Count == smr[i].materials.Length)
                    {
                        for (int j = 0; j < smr[i].materials.Length; j++)
                        {
                            smr[i].materials[j].shader = recordNormalShader[i][j];
                        }
                    }
                }
            }

            if (isStopParticle)
            {
                ParticleSystem[] ps = gameObject.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < ps.Length; i++)
                {
                    ps[i].Play();
                }
            }
        }
        #endregion
    }
}