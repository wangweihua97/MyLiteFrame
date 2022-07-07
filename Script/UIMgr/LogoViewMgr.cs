using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Events;
using Script.Mgr;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Main
{
    public class LogoViewMgr : UUIMgr
    {
        public Image LogoImage;
        public GameObject warn;
        public Image warn_icon;
        public Text warn_title;
        public Text warn_content;
        public override string Name => "LogoView";
        public override string sortingLayerName => "LogoView";

        public float LogoTime = 2;

        private bool isRecord = false;
        private float recordTime = 0;
        
        protected override void DoEnable()
        {
            base.DoEnable();
            StartCoroutine(ShowLogo());
        }
        protected override void DoUpdata()
        {
            base.DoUpdata();
            if (isRecord)
                recordTime += Time.deltaTime;

        }

        //逐渐展示logo
        IEnumerator ShowLogo()
        {
            yield return new WaitForSeconds(0.5f);
            //logo出现
            LogoImage.color = new Color(1,1,1,0);
            LogoImage.gameObject.SetActive(true);
            LogoImage.DOColor(new Color(1, 1, 1, 1), 0.8f);
            yield return new WaitForSeconds(2.3f);
            //logo消失
            LogoImage.DOColor(new Color(1, 1, 1, 0), 0.8f);
            yield return new WaitForSeconds(0.8f);
            warn_fade_in();
            yield return new WaitForSeconds(0.5f);
            //
            Global.instance.Init();
            isRecord = true;
            GameFlowMgr.EnterGame.Invoke();
            //
            GameFlowTask gameFlowTask = FlowTaskFactory.CreatTask(FadeLogo);
            GameFlowMgr.LoadedScene.AddTask(gameFlowTask);

        }
        
        //逐渐消失logo
        IEnumerator FadeLogo(GameFlowTask gameFlowTask)
        {
            yield return new WaitForSeconds(GetResidueLogoShowTime());
            warn_fade_out();
            yield return new WaitForSeconds(1f);
            warn.SetActive(false);
            //
            LogoImage.gameObject.SetActive(false);
            LogoEnd();
            gameFlowTask.Completed.Invoke();
        }

        void warn_fade_in()
        {
            warn.SetActive(true);
            warn_icon.color = new Color(0.34f,0.34f,0.34f,0);
            warn_icon.DOColor(new Color(0.34f, 0.34f, 0.34f, 1), 0.5f);
            warn_title.color = new Color(0.34f,0.34f,0.34f,0);
            warn_title.DOColor(new Color(0.34f, 0.34f, 0.34f, 1), 0.5f);
            warn_content.color = new Color(0.34f,0.34f,0.34f,0);
            warn_content.DOColor(new Color(0.34f, 0.34f, 0.34f, 1), 0.5f);
        }
        void warn_fade_out()
        {
            warn_icon.DOColor(new Color(0.34f, 0.34f, 0.34f, 0), 0.5f);
            warn_title.DOColor(new Color(0.34f, 0.34f, 0.34f, 0), 0.5f);
            warn_content.DOColor(new Color(0.34f, 0.34f, 0.34f, 0), 0.5f);
        }

        float GetResidueLogoShowTime()
        {
            isRecord = false;
            float residue = LogoTime - recordTime;
            if (residue < 0.02f)
                residue = 0.02f;
            return residue;
        }
        

        void LogoEnd()
        {
            Destroy(gameObject);
        }
    }
}