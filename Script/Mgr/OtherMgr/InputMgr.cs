using System;
using Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering;

namespace OtherMgr
{
    public class InputMgr : MonoBehaviour
    {
        private string typeStr = "";
        private BaseInput _baseInput;
        private void Start()
        {
            EventCenter.ins.AddEventListener<string>("InputMgr_setType", SetType);
            _baseInput = new BaseInput();
        }

        void SetType(string str)
        {
            typeStr = str;
        }

        /*void InitBaseInput()
        {
            _baseInput.Base.KeyPress.performed += context =>
            {
                if (context.interaction is PressInteraction)
                {
                    if (context.duration <= 0)
                    {
                        EventCenter.ins.EventTrigger("AnyKeyDown");
                        int a = context.ReadValue<int>();
                        EventCenter.ins.EventTrigger("KeyDown"+typeStr,a);
                    }
                    else
                    {
                        EventCenter.ins.EventTrigger("KeyUp"+typeStr,context.ReadValueAsObject() as Keyboard);
                    }
                }
            };

            _baseInput.Base.KeyInteraction.performed += context =>
            {
                if (context.interaction is TapInteraction)
                {
                    EventCenter.ins.EventTrigger("KeyClick"+typeStr,context.ReadValueAsObject() as Keyboard);
                }
                else if(context.interaction is HoldInteraction)//按住超过0.4s执行
                {
                    EventCenter.ins.EventTrigger("KeyStartHold"+typeStr,context.ReadValueAsObject() as Keyboard);
                }
            };
            
            _baseInput.Base.KeyInteraction.canceled += context =>
            {
                if (context.interaction is HoldInteraction)
                {
                    EventCenter.ins.EventTrigger("KeyEndHold"+typeStr,context.ReadValueAsObject() as Keyboard,context.duration);
                }
            };
            _baseInput.Enable();
        }*/

        private void Update()
        {
            CheckKey(KeyCode.W);
            CheckKey(KeyCode.A);
            CheckKey(KeyCode.S);
            CheckKey(KeyCode.D);
            CheckKey(KeyCode.H);
            CheckKey(KeyCode.J);
            CheckKey(KeyCode.K);
            CheckKey(KeyCode.U);
            CheckKey(KeyCode.Q);
            CheckKey(KeyCode.E);
            CheckKey(KeyCode.F12);
            CheckKey(KeyCode.F1);
            // if (Input.GetMouseButton(0))
            // {
            //     EventCenter.ins.EventTrigger("KeyDown",KeyCode.J);
            // }
        }

        void CheckKey(KeyCode keyCode)
        {
            CheckKeyDown(keyCode);
            CheckKeyUp(keyCode);
        }

        void CheckKeyDown(KeyCode keyCode)
        {
            if (Input.GetKeyDown(keyCode))
            {
                SortingGroup.UpdateAllSortingGroups();
                EventCenter.ins.EventTrigger("AnyKeyDown");
                EventCenter.ins.EventTrigger("KeyDown"+typeStr,keyCode);
            }
        }
            
        void CheckKeyUp(KeyCode keyCode)
        {
            if(Input.GetKeyUp(keyCode))
                EventCenter.ins.EventTrigger("KeyUp"+typeStr,keyCode);
        }
        private void OnDestroy()
        {
            //_baseInput.Disable();
        }
    }
}