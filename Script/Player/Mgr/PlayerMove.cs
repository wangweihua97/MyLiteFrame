using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class PlayerMove
    {
        public PlayerMgr Owner;
        public Transform MoveTransform;
        public bool IsMove;
        
        private Tweener _tweener;
        public PlayerMove(PlayerMgr owner)
        {
            Owner = owner;
            IsMove = false;
        }

        public void SetPosition(Vector3 position)
        {
            if(!CheckMoveTransform())
                return;
            MoveTransform.position = position;
        }
        
        public void SetRotation(Quaternion rotation)
        {
            if(!CheckMoveTransform())
                return;
            MoveTransform.rotation = rotation;;
        }

        public async Task DoMove(Vector3 position ,float time ,Action callBack)
        {
            if(!CheckMoveTransform())
                return;
            IsMove = true;
            PlayerMgr.instance.playerAnimationMgr.ChangeIdle("Run" ,0 ,0.3f);
            await Task.Delay(TimeSpan.FromSeconds(0.15f));
            if(_tweener != null && !_tweener.IsComplete())
                _tweener.Kill(false);
            _tweener = MoveTransform.DOMove(position, time).SetEase(Ease.Linear);
            _tweener.onComplete += () =>
            {
                callBack?.Invoke();
                EndMove();
            };
        }

        public void StopMove()
        {
            if(!IsMove)
                return;
            
            _tweener.Kill(false);
            EndMove();
        }

        bool CheckMoveTransform()
        {
            if (MoveTransform == null)
            {
                Debug.Log("MoveTransform没有赋值，但是执行移动");
                return false;
            }

            return true;
        }

        void EndMove()
        {
            IsMove = false;
            Owner.playerAnimationMgr.ChangeDefaultIdle(0.3f);
        }
    }
}