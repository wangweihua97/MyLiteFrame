using System;
using Script.Game.BattleScene;
using UnityEngine;

namespace Script.Scene.Game
{
    public class SceneBlock : MonoBehaviour
    {
        [Header("该片的长度，默认是77.55")]
        public float Extent = 77.55f;

        [HideInInspector]
        public int Index
        {
            get;
            private set;
        }
        [HideInInspector]
        public bool IsUsed = false;
        public string Name
        {
            get
            {
                return gameObject.name;
            }
        }

        private bool _init = false;
        public BattleShift BattleShift;

        private void Awake()
        {
            if(_init)
                return;
            gameObject.SetActive(false);
            IsUsed = false;
        }

        public void SetPosition(float x)
        {
            _init = true;
            gameObject.SetActive(true);
            IsUsed = true;
            transform.localPosition = new Vector3(x ,0 ,0);
        }

        public void Shelve()
        {
            _init = true;
            gameObject.SetActive(false);
            IsUsed = false;
        }
        
        
        public void SetIndex(int index)
        {
            Index = index;
        }
        
        public Vector3 GetPlayerLocalPosition()
        {
            return BattleShift.PlayerPoint.position - transform.position;
        }
        
        public Vector3 GetMonsterLocalPosition()
        {
            return BattleShift.MonsterBrithPoint.position - transform.position;
        }
        
        public Quaternion GetPlayerRotation()
        {
            return BattleShift.PlayerPoint.rotation;
        }
        
        public Quaternion GetMonsterRotation()
        {
            return BattleShift.MonsterBrithPoint.rotation;
        }
    }
}