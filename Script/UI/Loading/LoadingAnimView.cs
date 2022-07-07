﻿using System;
 using System.Collections;
 using Script.Main;
 using UnityEngine;

namespace UI.Loading
{
    public class LoadingAnimView : MonoBehaviour
    {
        public Animator Animator;
        public void PlayEnter(Action callBack)
        {
            gameObject.SetActive(true);
            Animator.Play("Loading_Anim01");
            Global.instance.StartCoroutine(ShowFalse(callBack));
        }

        public void PlayOut(Action callBack)
        {
            gameObject.SetActive(true);
            Animator.Play("Loading_OK");
            Global.instance.StartCoroutine(ShowFalse(callBack));
        }

        IEnumerator ShowFalse(Action callBack)
        {
            yield return new WaitForSeconds(0.5f);
            gameObject.SetActive(false);
            callBack.Invoke();
        }
    }
}