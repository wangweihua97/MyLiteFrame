using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Script.Mgr;
using UnityEngine;

namespace UnitMgr
{
    /*public class BaseJoyconSingleMgr : MonoBehaviour
    {
        private bool isInit;
        public bool isLeft;
        public int joycon;
        public JoyconInputMgr JoyconInputMgr;
        public Dictionary<Joycon.Button,float> buttons = new Dictionary<Joycon.Button, float>();
        public bool isRecord;
        public JoyconMoveHelper joyconMoveHelper;
        public List<Joycon.Button> CDNum = new List<Joycon.Button>();
        public void Update()
        {
            if(!isInit)
                Init();
            CheckClick();
            CountCD();
        }

        public virtual void Init()
        {
            InitJoycon();
            InitDictionary();
            InitJoyconMoveHelper();
        }

        void InitJoyconMoveHelper()
        {
            joyconMoveHelper = new JoyconMoveHelper();
            joyconMoveHelper.Init(isLeft);
        }

        void CountCD()
        {
            if(CDNum.Count <= 0)
                return;
            buttons[CDNum[0]] -= Time.unscaledDeltaTime;
            if (buttons[CDNum[0]] < 0)
            {
                CDNum.RemoveAt(0);
            }
        }
        
        private void FixedUpdate()
        {
            Record();
        }

        void InitJoycon()
        {
            isInit = true;
            JoyconInputMgr = JoyconInputMgr.instance;
            for (int i = 0; i < JoyconInputMgr.instance.j.Count; i++)
            {
                if (isLeft == JoyconInputMgr.instance.j[i].isLeft)
                {
                    joycon = i;
                    return;
                }
            }
            joycon = 9999;
            EventCenter.ins.EventTrigger("NoJoycon",isLeft);
        }
        
        void Record()
        {
            if(!isRecord || !isEnable())
                return;
            joyconMoveHelper.EnQueue(JoyconInputMgr.instance.j[joycon].GetGyro());
        }
        
        public void ClearRecord()
        {
            joyconMoveHelper.Clear();
        }

        void InitDictionary()
        {
            buttons.Add(Joycon.Button.HOME ,0);
            buttons.Add(Joycon.Button.DPAD_UP ,0);
            buttons.Add(Joycon.Button.DPAD_DOWN ,0);
            buttons.Add(Joycon.Button.DPAD_LEFT ,0);
            buttons.Add(Joycon.Button.DPAD_RIGHT ,0);
        }

        public bool isEnable()
        {
            if (joycon >= JoyconInputMgr.j.Count)
            {
                ReConnect();
                return false;
            }
            return true;
        }

        public void ReConnect()
        {
            for (int i = 0; i < JoyconInputMgr.instance.j.Count; i++)
            {
                if (isLeft == JoyconInputMgr.instance.j[i].isLeft)
                    joycon = i;
                return;
            }
            joycon = 9999;
        }
        
        void CheckClick()
        {
            if (!isEnable())
                return;
            if(JoyconInputMgr.j[joycon].GetButtonDown(Joycon.Button.HOME))
                CheckKey(Joycon.Button.HOME);
            if(JoyconInputMgr.j[joycon].GetButtonDown(Joycon.Button.DPAD_UP))
                CheckKey(Joycon.Button.DPAD_UP);
            if(JoyconInputMgr.j[joycon].GetButtonDown(Joycon.Button.DPAD_DOWN))
                CheckKey(Joycon.Button.DPAD_DOWN);
            if(JoyconInputMgr.j[joycon].GetButtonDown(Joycon.Button.DPAD_LEFT))
                CheckKey(Joycon.Button.DPAD_LEFT);
            if(JoyconInputMgr.j[joycon].GetButtonDown(Joycon.Button.DPAD_RIGHT))
                CheckKey(Joycon.Button.DPAD_RIGHT);
        }

        void CheckKey(Joycon.Button key)
        {
            if(buttons[key] > 0)
                return;
            buttons[key] = 0.1f;
            CDNum.Add(key);
            ClickButtonEvent(key);
        }

        public void SetRumble(float low_freq, float high_freq, float amp, int time = 0)
        {
            if (!isEnable())
                return;
            JoyconInputMgr.instance.j[joycon].SetRumble (low_freq, high_freq, amp, time);
        }

        public virtual void ClickButtonEvent(Joycon.Button key)
        {
            
        }
    }*/
}