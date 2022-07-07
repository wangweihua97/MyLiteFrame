using System.Collections;
using DG.Tweening;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ComboCom : UComponent
    {
        public Text ComboTime;
        public GameObject ComboEffect;
        public Animator Animator;

        private int _coroutinesAliveCount = 0;

        public void DoComboEffect(int time)
        {
            ComboTime.text = time.ToString();
            if (Animator == null)
                Animator = GetComponent<Animator>();
            //transform.DOKill(false);
            DoComboEffect();
        }

        void DoComboEffect()
        {
            if(_coroutinesAliveCount <= 0) 
                gameObject.SetActive(true);
            _coroutinesAliveCount ++;
            //transform.localScale = new Vector3(0.7f,0.7f,0.7f);
            //transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
            Animator.Rebind();
            ComboEffect.SetActive(false);
            ComboEffect.SetActive(true);
            StartCoroutine(WaitEffectVanish());
        }

        IEnumerator WaitEffectVanish()
        {
            yield return new WaitForSeconds(Animator.GetCurrentAnimatorStateInfo(0).length);
            _coroutinesAliveCount--;
            if(_coroutinesAliveCount <= 0)
                gameObject.SetActive(false);
        }
    }
}