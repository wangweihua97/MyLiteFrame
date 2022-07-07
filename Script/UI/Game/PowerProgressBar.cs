using System;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PowerProgressBar : MonoBehaviour
    {
        private Slider _slider;
        public GameObject ComboEffect;
        private void Awake()
        {
            _slider = GetComponent<Slider>();
            EventCenter.ins.AddEventListener("EnterComBo",EnterComBo);
            EventCenter.ins.AddEventListener("ExitComBo",ExitComBo);
            
        }

        void EnterComBo()
        {
            ComboEffect.SetActive(true);
        }

        void ExitComBo()
        {
            ComboEffect.SetActive(false);
        }

        public void SetValue(float value)
        {
            _slider.value = value;
        }

        public void OnDestroy()
        {
            EventCenter.ins.RemoveEventListener("EnterComBo",EnterComBo);
            EventCenter.ins.RemoveEventListener("ExitComBo",ExitComBo);
        }
    }
}