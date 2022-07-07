
using System;
using Events;
using UnityEngine;

namespace UnitMgr
{
    /*public class LeftJoyconMgr : BaseJoyconSingleMgr
    {
        public static LeftJoyconMgr instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            isLeft = true;
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
                case Joycon.Button.DPAD_UP:
                    EventCenter.ins.EventTrigger("KeyDown",KeyCode.W);
                    break;
                case Joycon.Button.DPAD_DOWN:
                    EventCenter.ins.EventTrigger("KeyDown",KeyCode.S);
                    break;
                case Joycon.Button.DPAD_LEFT:
                    EventCenter.ins.EventTrigger("KeyDown",KeyCode.A);
                    break;
                case Joycon.Button.DPAD_RIGHT:
                    EventCenter.ins.EventTrigger("KeyDown",KeyCode.D);
                    break;
            }
        }
    }*/
}