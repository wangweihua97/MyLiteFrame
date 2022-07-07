using System;
using Events;
using UnityEngine;

namespace UnitMgr
{
    /*public class RightJoyconMgr : BaseJoyconSingleMgr
    {
        
        public static RightJoyconMgr instance;

        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            isLeft = false;
        }

        public override void Init()
        {
            base.Init();
            isRecord = true;
        }

        void Update()
        {
            base.Update();
        }

        public override void ClickButtonEvent(Joycon.Button key)
        {
            base.ClickButtonEvent(key);
            Debug.Log(key);
            EventCenter.ins.EventTrigger("AnyKeyDown");
            switch (key)
            {
                case Joycon.Button.HOME:
                    EventCenter.ins.EventTrigger("KeyDown",KeyCode.K);
                    break;
                case Joycon.Button.DPAD_UP:
                    EventCenter.ins.EventTrigger("KeyDown",KeyCode.U);
                    break;
                case Joycon.Button.DPAD_DOWN:
                    EventCenter.ins.EventTrigger("KeyDown",KeyCode.K);
                    break;
                case Joycon.Button.DPAD_LEFT:
                    EventCenter.ins.EventTrigger("KeyDown",KeyCode.H);
                    break;
                case Joycon.Button.DPAD_RIGHT:
                    EventCenter.ins.EventTrigger("KeyDown",KeyCode.J);
                    break;
            }
        }
    }*/
}