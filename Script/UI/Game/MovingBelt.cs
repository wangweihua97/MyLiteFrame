using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace UI
{
    public class MovingBelt : MonoBehaviour
    {
        public GameObject ActionItemPrefab;
        public BeltItem belt1;
        public BeltItem belt2;
        public GameMainView GameMainView;
        private bool isInit;

        public void Init()
        {
            isInit = true;
            belt1.Init(this);
            belt1.isLeft = true;
            belt2.Init(this);
            belt2.isLeft = false;
        }

        public void DoUpdate()
        {
            belt1.UpdateMove();
            belt2.UpdateMove();
        }

        public void Close()
        {
            belt1.Close();
            belt2.Close();
        }
        private void Update()
        {
            
        }

        private void OnDestroy()
        {
            belt1.OnDestory();
            belt2.OnDestory();
        }
    }
}