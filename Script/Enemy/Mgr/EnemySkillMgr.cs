using System;
using System.Collections;
using DG.Tweening;
using OtherMgr;
using Player;
using Script.Enemy.Model;
using Script.Excel.Table;
using Script.Main;
using Script.Mgr;
using Script.Model;
using Script.UnitMgr.DoTween;
using UnityEngine;

namespace Enemy
{
    public class EnemySkillMgr : MonoBehaviour
    {
        private int skillNum = 0;
        private float[] _skillWeight;
        private EnemySkillInfo[] _skills;
        public int CurSkillIndex;

        private void Start()
        {
            InitSkill();
        }

        public EnemySkillInfo GetAttackSkillInfo()
        {
            return _skills[CurSkillIndex];
        }

        public void Attack()
        {
            RandomSelectionSkill();
            TDSkill skill = ExcelMgr.TDSkill.Get(_skills[CurSkillIndex].skillId);
            EnemyMgr.instance.isSpelling = true;
            StartCoroutine(WaitSpellAnimation(skill.behindTime * 0.001f));
            PlayActionAnimation(skill);
            PlayFireEffect(skill);

            if (skill.bulletEffects != Const.NULL_STRING)
            {
                StartCoroutine(WaitFireBullet(skill));
            }
            else
                StartCoroutine(WaitPlayHitEffect(skill));
        }
        
        void PlayActionAnimation(TDSkill skill)
        {
            EnemyMgr.instance.EnemyAnimationMgr.Play(skill.Action);
        }

        void PlayFireEffect(TDSkill skill)
        {
            if (!skill.skillEffects.Equals(Const.NULL_STRING))
            {
                string EffectPath = AttackEffectMgr.GetFullPath(skill.skillEffects);
                GameObject attackEffect = PoolManager.EffectPool.Spawn(EffectPath, transform.parent, Vector3.zero, Vector3.zero);
                StartCoroutine(EnemyMgr.instance.RecycleEffect(attackEffect, EffectPath ,2f));
            }
        }
        
        
        void InitSkill()
        {
            TDMonster data = ExcelMgr.TDMonster.Get(GameVariable.EnemyId[GameVariable.CurBattleIndex]);
            skillNum = data.ability2 == Const.NULL_INT ? 1 :
                data.ability3 == Const.NULL_INT ? 2 : 3;

            SetSkillWeight(data);
            SetSkills(data);
        }

        void SetSkillWeight(TDMonster data)
        {
            _skillWeight = new float[skillNum];
            int[] weights = new int[skillNum];
            float sum = 0;
            for (int i = 0; i < skillNum; i++)
            {
                if (i == 0)
                    weights[i] = data.weight1;
                if(i == 1)
                    weights[i] = data.weight2;
                if(i == 2)
                    weights[i] = data.weight3;
                sum += weights[i];
            }
            for (int i = 0; i < skillNum; i++)
            {
                _skillWeight[i] = weights[i] / sum;
            }
        }

        void SetSkills(TDMonster data)
        {
            _skills = new EnemySkillInfo[skillNum];
            for (int i = 0; i < skillNum; i++)
            {
                if (i == 0)
                {
                    EnemySkillInfo skillInfo = new EnemySkillInfo();
                    skillInfo.ability = data.ability1;
                    skillInfo.Prop1 = data.a1Prop1;
                    skillInfo.Prop2 = data.a1Prop2;
                    skillInfo.skillId = data.skill1;
                    _skills[i] = skillInfo;
                }
                if (i == 1)
                {
                    EnemySkillInfo skillInfo = new EnemySkillInfo();
                    skillInfo.ability = data.ability2;
                    skillInfo.Prop1 = data.a2Prop1;
                    skillInfo.Prop2 = data.a2Prop2;
                    skillInfo.skillId = data.skill2;
                    _skills[i] = skillInfo;
                }
                if(i == 2)
                {
                    EnemySkillInfo skillInfo = new EnemySkillInfo();
                    skillInfo.ability = data.ability3;
                    skillInfo.Prop1 = data.a3Prop1;
                    skillInfo.Prop2 = data.a3Prop2;
                    skillInfo.skillId = data.skill3;
                    _skills[i] = skillInfo;
                }
            }
        }
        
         void RandomSelectionSkill()
        {
            if (skillNum == 1)
            {
                CurSkillIndex = 0;
                return;
            }
            
            float random = UnityEngine.Random.Range(0f, 1f);
            float sum = 0;
            CurSkillIndex = skillNum - 1;
            for (int i = 0 ;i < skillNum ;i++)
            {
                sum += _skillWeight[i];
                if (random < sum)
                {
                    CurSkillIndex = i;
                    AdjustSkillWeight();
                    return;
                }
            }
            AdjustSkillWeight();
        }

        void AdjustSkillWeight()
        {
            float decline = _skillWeight[CurSkillIndex] / 2;
            _skillWeight[CurSkillIndex] = decline;
            int othersSkillNum = skillNum - 1;
            for (int i = 0; i < skillNum; i++)
            {
                if(i == CurSkillIndex)
                    continue;
                _skillWeight[i] += decline / othersSkillNum;
            }
        }

        void HitPlayer(TDSkill skill)
        {
            string fullPath = AttackEffectMgr.GetFullPath(skill.hitEffects);
            GameObject hitEffect = PoolManager.EffectPool.Spawn(fullPath, PlayerMgr.instance.Dummy_Hit, Vector3.zero, Vector3.zero);
            PlayerMgr.instance.GetHurt();
            StartCoroutine(EnemyMgr.instance.RecycleEffect(hitEffect, fullPath ,2f));
        }

        IEnumerator WaitPlayHitEffect(TDSkill skill)
        {
            yield return new WaitForSeconds(skill.hitTime * 0.001f);
            HitPlayer(skill);
        }

        IEnumerator WaitFireBullet(TDSkill skill)
        {
            yield return new WaitForSeconds(skill.hitTime * 0.001f);
            string fullPath = AttackEffectMgr.GetFullPath(skill.bulletEffects);
            GameObject bulletEffect = PoolManager.EffectPool.Spawn(fullPath, EnemyMgr.instance.Punch_Hit_Dummy, Vector3.zero, Vector3.zero);
            
            float flySpeed = skill.bulletFlySpeed / 10;
            
            float flyTime = Mathf.Abs((EnemyMgr.instance.Punch_Hit_Dummy.position - PlayerMgr.instance.Dummy_Hit.position).magnitude) / flySpeed;
            if (skill.bulletFlyType == 0)
                DoBulletFly(bulletEffect, PlayerMgr.instance.Dummy_Hit.position, BulletFlyType.StraightLine, flyTime);
            else
                DoBulletFly(bulletEffect, PlayerMgr.instance.Dummy_Hit.position, BulletFlyType.Parabola, flyTime);
            
            StartCoroutine(EnemyMgr.instance.RecycleEffect(bulletEffect, fullPath, flyTime));
            yield return new WaitForSeconds(flyTime);
            HitPlayer(skill);
        }

        void DoBulletFly(GameObject bulle ,Vector3 target, BulletFlyType type, float time)
        {
            switch (type)
            {
                case BulletFlyType.StraightLine:
                    bulle.transform.DOMove(target, time);
                    break;
                case BulletFlyType.Parabola:
                    Vector3 startPositon = bulle.transform.position;
                    DOTween.To(setter: value =>
                        {
                            bulle.transform.position = ParabolaTweener.Parabola(startPositon, target, 3, value);
                        }, startValue: 0, endValue: 1, duration: time)
                        .SetEase(Ease.Linear);
                    break;
            }
        }
        
        IEnumerator WaitSpellAnimation(float time)
        {
            yield return new WaitForSeconds(time);
            EnemyMgr.instance.isSpelling = false;
        }
        
        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}