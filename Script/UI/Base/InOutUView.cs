using System.Collections;
using System.Collections.Generic;
using Script.Main;
using UnityEngine;

namespace UI.Base
{
    public class InOutUView : UView
    {
        [Header("出入场动画组件")]
        public Animator InOutAni;
        [Header("入场动画名字")]
        public string InAniStr;
        [Header("离场动画名字")]
        public string OutAniStr;

        protected bool aotuPlayIn = true;

        // private bool tmpFlag;
        protected delegate void cb();
        private cb _cb = null;
        /**true=入场,false=离场*/
        protected void playInOutAni(bool b, cb pCb)
        {
            _cb = pCb;
            // tmpFlag = b;
            string pn = b?InAniStr:OutAniStr;
            if (InOutAni&&!string.IsNullOrEmpty(pn))
            {
                InOutAni.enabled = true;
                InOutAni.Rebind();
                InOutAni.Play(pn);
                Global.instance.StartCoroutine(delayFun(GetAniLength(pn)));
            }
            else
            {
                if (null!=pCb)
                {
                    cb tmpCb = pCb;
                    tmpCb();
                }
            }
        }
    
        private IEnumerator delayFun(float time)
        {
            yield return new WaitForSeconds(time);
            //禁用-解锁控制属性
            InOutAni.enabled = false;
            if (null != _cb)
            {
                cb tmpCb = _cb;
                _cb = null;
                tmpCb();
            }
        }

        /**获取动画时长*/
        protected float GetAniLength(string name)
        {
            float re = 0;
            AnimationClip[] clips = InOutAni.runtimeAnimatorController.animationClips;
            int len = clips.Length;
            for (int i = 0; i < len; i++)
            {
                if (clips[i].name.Equals(name))
                {
                    re = clips[i].length;
                    break;
                }
            }
            return re;
        }
        
        /**页面打开时-根据aotuPlayIn播放入场*/
        public override void DoOpen()
        {
            base.DoOpen();
            if(aotuPlayIn)playInOutAni(true, enterStageComplete);
        }

        /**动画执行完成后*/
        protected virtual void enterStageComplete()
        {
            
        }
    }
}


