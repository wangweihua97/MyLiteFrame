using System;
using DG.Tweening;
using OtherMgr;
using Player;
using Script.Tool.PoolManager.Model;
using Script.UnitMgr.DoTween;
using UnityEngine;

namespace Script.Item
{
    public struct ItemShowData
    {
        public GameObject Item;
    }
    public class ItemShowPrefab : MonoBehaviour ,IPool
    {
        [Header("Item的放置位置")]
        public Transform ItemTransformParent;

        public static float MoveSpeed = 40f;
        private bool _isPlayShow;
        private GameObject _item;
        public void InPool()
        {
            
        }

        public void OutPool()
        {
            
        }

        public void SetData(ItemShowData data)
        {
            _item = data.Item;
            data.Item.transform.SetParent(ItemTransformParent);
            data.Item.transform.localPosition = Vector3.zero;
            data.Item.transform.localRotation = Quaternion.Euler(Vector3.zero);
            data.Item.transform.localScale = Vector3.one;
        }

        public void PlayShow(Vector3 endPosition)
        {
            Vector3 startPositon = transform.position;
            transform.localScale = new Vector3(0.1f,0.1f,0.1f);
            transform.DOScale(new Vector3(1f, 1f, 1f), 1f);
            Tweener tweener = DOTween.To(setter: value =>
                {
                    transform.position = ParabolaTweener.Parabola(startPositon, endPosition, 6f, value);
                }, startValue: 0, endValue: 1, duration: 1f)
                .SetEase(Ease.Linear);
            tweener.onComplete += () =>
            {
                WaitTimeMgr.WaitTime(1f, () =>
                {
                    _isPlayShow = true;
                });
            };
        }

        private void Update()
        {
            if (_isPlayShow)
            {
                Vector3 direction = PlayerMgr.instance.GetTransform().position - transform.position +
                                    new Vector3(0, 1, 0);
                float distance = direction.magnitude;
                if (distance < 1f)
                {
                    _isPlayShow = false;
                    PoolManager.ItemPool.DesSpawn(_item);
                    PoolManager.CommonPool.DesSpawn(gameObject);
                    return;
                    
                }
              
                Vector3 move = direction.normalized * MoveSpeed * Time.deltaTime;
                transform.position += move;
            }
        }
    }
}