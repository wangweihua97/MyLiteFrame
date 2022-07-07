using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using Events;
using OtherMgr;
using Script.Excel.Table;
using Script.Main;
using Script.Mgr;
using Script.Model;
using UI.Base;
using UnityEditor;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace UI.Main
{
    public class LoginView : UView
    {
        public override bool DefaultShow => true;
        public Animator title_root;
        //public RectTransform select_root;
        private int itemIdx=0;
        /**-1=初始化,0=点击任意继续,1=有选项*/
        public int viewType = -1;
        public Image testImg;
        public Image anyKeyDownBg; 
        public Text anyKeyDown; 
        public Text des; 
        public RectTransform selectItem;
        public TipsCtr tipsCtr;
        private Tweener ti;
        private Tweener ti1;
        private bool isAddEvent=false;
        
        private bool AnyKeyDownFunAcitve = false;
        private bool KeyDownFunAcitve = false;

        public override void DoCreat()
        {
            base.DoCreat();
            HallViewMgr.LoginView = this;
            EventCenter.ins.AddEventListener("AnyKeyDown",AnyKeyDown);
            //EventCenter.ins.AddEventListener<KeyCode>("KeyDown",KeyDown);
            tipsCtr.Show(false);
        }
        protected override void EnterNewScene()
        {
            base.EnterNewScene();
            if(!gameObject.activeInHierarchy)
                Show(true);
            AudioManager.PlayBackground("主页等待背景音");
            Global.instance.StartCoroutine(EnterStep4(0.5f));
            // if (GameVariable.LoginEnterCount == 0)
            // {
                // GameVariable.LoginEnterCount++;
                /*DPlayMode cfg = ExcelMgr.TDPlayMode.Get(itemIdx+101+"");
                select_tips_des.text = cfg.desc1;*/
                // tipsCtr.Show(false);
                //SetSelectPart();
                // Global.instance.StartCoroutine(EnterStep4(0.5f));
            // }
            // else
            // {
                /*AnyKeyDownFunAcitve = true;
                KeyDownFunAcitve = true;
                anyKeyDown.gameObject.SetActive(false);
                des.gameObject.SetActive(false);
                title_root.gameObject.SetActive(false);
                select_root.gameObject.SetActive(true);
                head_root.gameObject.SetActive(true);
                int len = item_list.Count;
                for (int i = 1; i < len; i++)
                {
                    item_list[i].color = new Color(1,1,1,1);
                }
                HallViewMgr.LevelSelectUIView.JumpShow();
                EventCenter.ins.EventTrigger("set_camera_act");
                EventCenter.ins.EventTrigger("set_camera_act_to_complete");
                Show(false);*/
                // DoClose();
                // switch (GameVariable.GameMode)
                // {
                //     case GameMode.LevelMode:
                //         HallViewMgr.LevelSelectUIView.DoOpen();
                //         break;
                //     case GameMode.TrainMode:
                //         TrainViewMgr.TrainSelectView.DoOpen();
                //         break;
                // }
            // }
        }


        private IEnumerator EnterStep4(float time)
        {
            AudioManager.PlayBackground("主页等待背景音");
            yield return new WaitForSeconds(time);
            title_root.gameObject.SetActive(true);
            anyKeyDown.gameObject.SetActive(true);
            des.gameObject.SetActive(true);
            title_root.Rebind();
            title_root.Play("Logo_View");
            viewType=0;
            anyKeyDownBg.gameObject.SetActive(true);
            des.gameObject.SetActive(true);
            anyKeyDownBg.color = new Color(1f,1f,1f,0.31f);
            ti1 = anyKeyDownBg.DOFade(1f, 1);
            ti1.SetLoops(-1, LoopType.Yoyo);                           
            ti1.SetAutoKill(false);                                    
            ti1.Play(); 
            anyKeyDown.color = new Color(1f,1f,1f,0.31f);
            ti = anyKeyDown.DOFade(1f, 1);
            ti.SetLoops(-1, LoopType.Yoyo);                           
            ti.SetAutoKill(false);                                    
            ti.Play();
            EventCenter.ins.EventTrigger("set_camera_act");
            yield return new WaitForSeconds(1.5f);
            AnyKeyDownFunAcitve = true;
        }

        void AnyKeyDown()
        {
            if(!AnyKeyDownFunAcitve)
                return;
            if (0==viewType)
            {
                AudioManager.PlayAudioEffectA("选中确认");
                AnyKeyDownFunAcitve=false;
                Global.instance.StartCoroutine(delayViewType(0.1f));
                //
                if(title_root.gameObject.activeInHierarchy)
                    title_root.Play("logo_fade");
                anyKeyDownBg.gameObject.SetActive(false);
                anyKeyDown.gameObject.SetActive(false);
                des.gameObject.SetActive(false);
                //
                // EventCenter.ins.AddEventListener("move_camera_complete",OnMoveCameraCom);
                //推送镜头
                // EventCenter.ins.EventTrigger("start_move_login_camera");
                //Debug.Log("推送镜头");
                //
                Global.instance.StartCoroutine(delayEnter(0.8f));
            }
            
        }
        /**推送镜头完成*/
        private void OnMoveCameraCom()
        {
            KeyDownFunAcitve = true;
            this.DoClose();
            HallViewMgr.MainView.DoOpen();
        }
        
        private IEnumerator delayEnter(float time)
        {
            yield return new WaitForSeconds(time);
            this.DoClose();
            Global.SceneMgr.StartScene("HallScene" ,LoadingViewMgr.LoadingView);
            // HallViewMgr.MainView.DoOpen();
            // EventCenter.ins.EventTrigger("enter_game_befor_check");
        } 

        private IEnumerator delayViewType(float time)
        {
            yield return new WaitForSeconds(time);
            viewType=1;
        }

        protected override void DoDestroy()
        {
            base.DoDestroy();
            isAddEvent = false;
            ti?.Kill();
            ti1?.Kill();
            EventCenter.ins.RemoveEventListener("move_camera_complete",OnMoveCameraCom);
            EventCenter.ins.RemoveEventListener("AnyKeyDown",AnyKeyDown);
        }
    }
}