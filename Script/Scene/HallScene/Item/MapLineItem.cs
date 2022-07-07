using System;
using OtherMgr;
using UnityEngine;

namespace Script.Scene.Hall.Item
{
    public class MapLineItem : MonoBehaviour
    {
        public  bool IsAlive;
        public float PlayTime;
        private float _totalTime;
        private ParticleSystem _particle;

        private Vector3 _star;
        private Vector3 _end;
        private Vector3 _s2e;
        private Vector3 _vertical;
        private float _radius;
        private void Awake()
        {
            _particle = GetComponent<ParticleSystem>();
            _particle.Stop();
        }

        private void Update()
        {
            if (IsAlive)
            {
                PlayTime += Time.deltaTime;
                if (PlayTime >= _totalTime)
                {
                    PlayTime = _totalTime;
                    IsAlive = false;
                    _particle.Pause();
                }
                Vector3 position = GetVector(GetRate(PlayTime / _totalTime));
                transform.parent.localPosition = position;
            }
        }

        public void Play(Vector3 end ,float time)
        {
            IsAlive = true;
            _particle.Play();
            _totalTime = time;
            InitVariable(end);
        }

        public void Dispose()
        {
            IsAlive = false;
            PoolManager.CommonPool.DesSpawn(gameObject);
        }

        void InitVariable(Vector3 end)
        {
            PlayTime = 0;
            _star = transform.parent.localPosition;
            _end = end;
            _s2e = _end - _star;
            _radius = (_s2e / 2).magnitude;
            _vertical = (_star + _end).normalized;
        }

        float GetRate(float lerp)
        {
            float a = Mathf.PI * lerp;
            return 0.5f - 0.5f * Mathf.Cos(a);
        }

        Vector3 GetVector(float lerp)
        {
            Vector3 v = _star + (_s2e * lerp);
            Vector3 a = _vertical * _radius * Mathf.Sqrt(0.25f - (0.5f - lerp) * (0.5f - lerp));
            return v + a;
        }
    }
}