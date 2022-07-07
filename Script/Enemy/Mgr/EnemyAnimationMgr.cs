using System.Collections;
using System.Collections.Generic;
using OtherMgr;
using Player;
using Script.Model;
using UnityEngine;

namespace Enemy
{
    public class EnemyAnimationMgr : MonoBehaviour
    {
        private Animator _animator;
        private Dictionary<string, AnimationClip> _animationClips;
        private EnemyMgr _enemyMgr;
        public void Init()
        {
            _animator = GetComponent<Animator>();
            _enemyMgr = GetComponent<EnemyMgr>();
            _animationClips = new Dictionary<string, AnimationClip>();
            foreach (var animation in _animator.runtimeAnimatorController.animationClips)
            {
                if(_animationClips.ContainsKey(animation.name))
                    continue;
                _animationClips.Add(animation.name ,animation);
            }
            ResetIdle();
        }

        public void ResetIdle()
        {
            Play("Idle");
        }

        public void Play(EnemyAnimation animation)
        {
            switch (animation)
            {
                case EnemyAnimation.Attack:
                    if(!_enemyMgr.IsLived)
                        return;
                    _enemyMgr.Attack();
                    break;
                case EnemyAnimation.Dead:
                    _animator.Play("Death");
                    /*WaitTimeMgr.WaitTime(2f, () =>
                    {
                        _enemyMgr.PlayDeathEffect();
                    });*/
                    break;
                case EnemyAnimation.Hurt:
                    if(!_enemyMgr.IsLived)
                        return;
                    _animator.Play("Hurt");
                    break;
            }
        }

        public void Play(string animationName)
        {
            _animator.Play(animationName);
        }

        public void PlayShow()
        {
            StartCoroutine(PlayShowAnimation());
        }

        IEnumerator PlayShowAnimation()
        {
            _enemyMgr.isSpelling = true;
            if (_animationClips.ContainsKey("FIdle_Perform"))
            {
                Play("FIdle_Perform");
                yield return new WaitForSeconds(_animationClips["FIdle_Perform"].length - 0.2f);
            }
            else
            {
                Play("FIdle_Show");
                yield return new WaitForSeconds(_animationClips["FIdle_Show"].length - 0.2f);
            }
            Play("FIdle");
            _enemyMgr.isSpelling = false;
        }
    }
}