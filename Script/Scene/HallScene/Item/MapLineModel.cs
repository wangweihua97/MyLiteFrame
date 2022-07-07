using System;
using OtherMgr;
using Script.Lua.Mgr;
using UnityEngine;

namespace Script.Scene.Hall.Item
{
    public class MapLineModel : MonoBehaviour
    {
        public  bool IsAlive;
        public float PlayTime;
        public MeshFilter MeshFilter;
        private float _totalTime;

        private Mesh _mesh;

        private Vector3 _star;
        private Vector3 _end;
        private Vector3 _s2e;
        private Vector3 _vertical;
        private float _radius;

        private void Awake()
        {
            _mesh = MeshFilter.mesh;
            ChangeMesh(0);
        }

        public void Play( Vector3 targetWorldPositon, Vector3 targetLocalPositon ,float time)
        {
            IsAlive = true;        
            PlayTime = 0;
            _totalTime = time;
            InitVariable(targetLocalPositon);
            transform.localPosition = (_end + _star) / 2;
            transform.forward = transform.position - targetWorldPositon;
            Vector3 up = transform.position - transform.parent.position;
            float angle = Vector3.Angle(up, transform.up);
            Vector3 b = Vector3.Cross(transform.up ,up);
            if(Vector3.Dot(transform.forward ,b) > 0)
                transform.Rotate(new Vector3(0,0,angle));
            else
                transform.Rotate(new Vector3(0,0,- angle));
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
                }
                ChangeMesh(PlayTime / _totalTime);
            }
        }

        void InitVariable(Vector3 end)
        {
            PlayTime = 0;
            _star = transform.localPosition;
            _end = end;
            _s2e = _end - _star;
            _radius = (_s2e / 2).magnitude * 10;
            _vertical = (_star + _end).normalized;
        }
        
        public void Dispose()
        {
            ChangeMesh(0);
            IsAlive = false;
            PoolManager.CommonPool.DesSpawn(gameObject);
        }

        void ChangeMesh(float x)
        {
            Vector3[] vertices = _mesh.vertices;
            int len = vertices.Length;
            float rad = x * Mathf.PI;

            for (int i = 0; i < 4; i++)
            {
                vertices[i].y = 0;
                vertices[i].z = _radius;
            }
            
            for (int i = 4; i < len; i+=2)
            {
                
                var a = Periphery(_radius, rad * i / len);
                vertices[i].y = a.Item2;
                vertices[i].z = a.Item1;
                
                vertices[i + 1].y = a.Item2;
                vertices[i + 1].z = a.Item1;
            }

            _mesh.vertices = vertices;
        }


        (float, float) Periphery(float radius ,float rad)
        {
            return (radius * Mathf.Cos(rad), radius * Mathf.Sin(rad));
        }
    }
}