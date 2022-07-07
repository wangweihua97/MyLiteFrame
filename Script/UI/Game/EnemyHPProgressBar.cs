using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EnemyHPProgressBar : MonoBehaviour
    {
        private Slider _slider;
        private float width;
        private RectTransform _rectTransform;

        public GameObject missEffect;
        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _rectTransform = transform as RectTransform;
            width = _rectTransform.rect.width;
        }

        public void SetValue(float value)
        {
            float oldValue = _slider.value;
            float newValue = value;
            _slider.value = value;
            if(!gameObject.activeInHierarchy)
                return;
            missEffect.SetActive(true);
            RectTransform rtf = missEffect.transform as RectTransform;
            float w = (oldValue - newValue) * width;
            rtf.sizeDelta = new Vector2(w, rtf.rect.height);
            rtf.localPosition = new Vector3(newValue*width - width/2,rtf.localPosition.y,rtf.localPosition.z);
            StartCoroutine(Miss());
        }

        IEnumerator Miss()
        {
            yield return new WaitForSeconds(0.3f);
            missEffect.SetActive(false);
        }
    }
}