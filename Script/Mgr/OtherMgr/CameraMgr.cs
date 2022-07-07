﻿using System;
using System.Collections;
 using System.Collections.Generic;
 using DG.Tweening;
 using Events;
using UnityEngine;

namespace OtherMgr
{
    /// <summary> 相机震动类型</summary>
    public enum ECameraShakeType
    {
        /// <summary> 垂直弹性</summary>
        eVerticalElastic,
        /// <summary> 双轴偏移</summary>
        ePlaneOffset
    }

    public class CameraMgr : MonoBehaviour
    {
        private Vector3 rePos;
        private Dictionary<string, Transform> _locations;
        public string CurLocation = "";

        private void Awake()
        {
        }

        private void Start()
        {
            rePos = transform.localPosition;
            EventCenter.ins.AddEventListener<int>("AttackEnemy",CameraShake);
            EventCenter.ins.AddEventListener("CameraInitLocations" ,InitLocations);
            EventCenter.ins.AddEventListener<string>("CameraMove" ,CameraMove);
            EventCenter.ins.AddEventListener<string>("CameraMoveStraightaway" ,CameraMoveStraightaway);

            //EventCenter.ins.AddEventListener<KeyCode>("KeyDown", KeyDown);
        }

        void InitLocations()
        {
            _locations = new Dictionary<string, Transform>();
            foreach (var t in transform.parent.GetComponentsInChildren<Transform>())
            {
                _locations.Add(t.gameObject.name ,t);
            }
        }

        void CameraMove(string location)
        {
            if(CurLocation.Equals(location))
                return;
            transform.DOLocalMove(_locations[location].transform.localPosition, 0.5f);
            transform.DOLocalRotate(_locations[location].transform.localRotation.eulerAngles, 0.5f);
            CurLocation = location;
        }

        void CameraMoveStraightaway(string location)
        {
            if(CurLocation.Equals(location))
                return;
            transform.localPosition = _locations[location].transform.localPosition;
            transform.localRotation = _locations[location].transform.localRotation;
            CurLocation = location;
        }

        void CameraShake(int hurt)
        {
            Shake();
        }

        #region 相机震动
        /// <summary> 当前的相机震动类型</summary>
        public ECameraShakeType curCamShakeType = ECameraShakeType.eVerticalElastic;        
        private bool shakeOver = true;

        /// <summary> 相机震动 </summary>
        void Shake()
        {
            if (!shakeOver)
                return;
            shakeOver = false;
            
            switch (curCamShakeType)
            {
                default:
                case ECameraShakeType.eVerticalElastic:
                    Shake_VerticalElastic();
                    break;
                case ECameraShakeType.ePlaneOffset:
                    StartCoroutine(Shake_PlaneOffset());
                    break;
            }
        }

        /// <summary> 垂直弹性震动 </summary>
        void Shake_VerticalElastic()
        {
            Ease eOut = Ease.OutCubic;
            Ease eIn = Ease.InCubic;
            float s = 0.2f;
            float t = 0.15f;
            float perS = s / 6f;
            float perT = t / 18f;

            //第一次上弹
            transform.DOLocalMoveY(rePos.y + perS * 6, perT * 5).SetEase(eOut).OnComplete(() =>
            {
                transform.DOLocalMoveY(rePos.y, perT * 5).SetEase(eIn).OnComplete(() =>
                {
                    //第二次上弹
                    transform.DOLocalMoveY(rePos.y - perS * 3, perT * 3).SetEase(eOut).OnComplete(() =>
                    {
                        transform.DOLocalMoveY(rePos.y, perT * 3).SetEase(eIn).OnComplete(() =>
                        {
                            //第三次上弹
                            transform.DOLocalMoveY(rePos.y + perS, perT).SetEase(eOut).OnComplete(() =>
                            {
                                transform.DOLocalMoveY(rePos.y, perT).SetEase(eIn).OnComplete(() =>
                                {
                                    shakeOver = true;
                                });
                            });
                        });
                    });
                });
            });
        }
        /// <summary> 双轴偏移震动 </summary>
        IEnumerator Shake_PlaneOffset()
        {
            float shake = 0.1f;
            int cnt = 0;
            while (cnt < 10)
            {
                transform.localPosition = new Vector3(
                    UnityEngine.Random.Range(0f, shake * 2f) - shake + rePos.x,
                    UnityEngine.Random.Range(0f, shake * 1f) - shake + rePos.y,
                    rePos.z);

                shake = shake / 1.05f;
                cnt++;

                yield return null;
            }

            shakeOver = true;
            transform.localPosition = rePos;

            yield return null;
        }
        #endregion

        /*void KeyDown(KeyCode keyCode)
        {
            switch (keyCode)
            {
                case KeyCode.K:
                    Shake();
                    break;
            }
        }*/

        private void OnDestroy()
        {
            EventCenter.ins.RemoveEventListener<int>("AttackEnemy",CameraShake);
            EventCenter.ins.RemoveEventListener("CameraInitLocations" ,InitLocations);
            EventCenter.ins.RemoveEventListener<string>("CameraMove" ,CameraMove);
            EventCenter.ins.RemoveEventListener<string>("CameraMoveStraightaway" ,CameraMoveStraightaway);

            //EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown", KeyDown);
        }
    }
}