using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HPProgressBar : MonoBehaviour
    {
        private Slider _slider;
        private void Awake()
        {
            _slider = GetComponent<Slider>();
            
        }

        public void SetValue(float value)
        {
            _slider.value = value;
        }
    }
}