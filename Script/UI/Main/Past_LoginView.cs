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
    public class Past_LoginView : UView
    {
        public override bool DefaultShow => true;
        
        public List<Text> item_list;
        public Animator title_root;
        public RectTransform select_root;
        private int itemIdx=0;
        /**-1=初始化,0=点击任意继续,1=有选项*/
        public int viewType = -1;
        public Image testImg;
        public Text anyKeyDown; 
        public Text des; 
        public RectTransform selectItem;
        public TipsCtr tipsCtr;
        public Text select_tips_des;
        public Animator head_root;
        private Tweener ti;
        private bool isAddEvent=false;
        
        private bool AnyKeyDownFunAcitve = false;
        private bool KeyDownFunAcitve = false;

        public override void DoCreat()
        {
            base.DoCreat();
            //HallViewMgr.LoginView = this;
            EventCenter.ins.AddEventListener("AnyKeyDown",AnyKeyDown);
            EventCenter.ins.AddEventListener<KeyCode>("KeyDown",KeyDown);
        }
        protected override void EnterNewScene()
        {
            base.EnterNewScene();
            if(!gameObject.activeInHierarchy)
                Show(true);
            if (GameVariable.LoginEnterCount == 0)
            {
                GameVariable.LoginEnterCount++;
                TDPlayMode cfg = ExcelMgr.TDPlayMode.Get(itemIdx+101+"");
                select_tips_des.text = cfg.desc1;
                tipsCtr.Show(false);
                SetSelectPart();
                Global.instance.StartCoroutine(EnterStep4(0.5f));
            }
            else
            {
                AudioManager.PlayBackground("主页等待背景音");
                AnyKeyDownFunAcitve = true;
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
                // HallViewMgr.LevelSelectUIView.JumpShow();
                EventCenter.ins.EventTrigger("set_camera_act");
                EventCenter.ins.EventTrigger("set_camera_act_to_complete");
                Show(false);
            }
        }


        private IEnumerator EnterStep4(float time)
        {
            AudioManager.PlayBackground("主页等待背景音");
            yield return new WaitForSeconds(time);
            title_root.gameObject.SetActive(true);
            anyKeyDown.gameObject.SetActive(true);
            des.gameObject.SetActive(true);
            title_root.Play("Logo_View");
            viewType=0;
            anyKeyDown.gameObject.SetActive(true);
            des.gameObject.SetActive(true);
            anyKeyDown.color = new Color(1,1,1,0);
            ti = anyKeyDown.DOFade(1, 1);
            ti.SetLoops(-1, LoopType.Yoyo);                           
            ti.SetAutoKill(false);                                    
            ti.Play();  
            EventCenter.ins.EventTrigger("set_camera_act");
            yield return new WaitForSeconds(1.5f);
            AnyKeyDownFunAcitve = true;
        }

        void KeyDown(KeyCode keyCode)
        {
            if(!IsTop() || !IsActive() || !KeyDownFunAcitve)
                return;
            if (1==viewType)
            {
                switch (keyCode)
                {
                    case KeyCode.W:
                        item_list[itemIdx].color = Color.white;
                        itemIdx = itemIdx>0?itemIdx-1:item_list.Count-1;
                        SetSelectPart();
                        break;
                    case KeyCode.A:
                   
                        break;
                    case KeyCode.S:
                        item_list[itemIdx].color = Color.white;
                        itemIdx = itemIdx<item_list.Count-1?itemIdx+1:0;
                        SetSelectPart();
                        break;
                    case KeyCode.D:
                        break;
                    case KeyCode.J:
                        //Global.SceneMgr.EnterGameScene("C6");
                        if (0==itemIdx)
                        {
                            // HallViewMgr.LevelSelectUIView.Show(true);
                            
                            //title_root.gameObject.SetActive(false);
                        }
                        else
                        {
                            TDPlayMode cfg = ExcelMgr.TDPlayMode.Get(itemIdx+101+"");
                            tipsCtr.ShowTxt(cfg.desc2);
                        }
                        break;
                    case KeyCode.K:
                        Show(true);
                        break;
                }
            }
        }
        
        private void SetSelectPart()
        {
            item_list[itemIdx].color = new Color(0.99f,0.83f,0.35f,1);
            selectItem.localPosition = new Vector3(item_list[itemIdx].rectTransform.localPosition.x-200, item_list[itemIdx].rectTransform.localPosition.y-15,0);
            selectItem.gameObject.SetActive(false);
            selectItem.gameObject.SetActive(true);
            //
            TDPlayMode cfg = ExcelMgr.TDPlayMode.Get(itemIdx+101+"");
            select_tips_des.text = cfg.desc1;
            
        }

        void AnyKeyDown()
        {
            if(!AnyKeyDownFunAcitve)
                return;
            if (0==viewType)
            {
                Global.instance.StartCoroutine(delayViewType(0.1f));
                //
                if(title_root.gameObject.activeInHierarchy)
                    title_root.Play("logo_fade");
                anyKeyDown.gameObject.SetActive(false);
                des.gameObject.SetActive(false);
                //
                EventCenter.ins.AddEventListener("move_camera_complete",OnMoveCameraCom);
                //推送镜头
                EventCenter.ins.EventTrigger("start_move_login_camera");
                //Debug.Log("推送镜头");
            }
            
        }
        /**推送镜头完成*/
        private void OnMoveCameraCom()
        {
            KeyDownFunAcitve = true;
            select_root.gameObject.SetActive(true);
            int len = item_list.Count;
            for (int i = 0; i < len; i++)
            {
                item_list[i].color = new Color(item_list[i].color.r,item_list[i].color.g,item_list[i].color.b,0);
                item_list[i].DOFade(1, 0.5f);
            }
            //
            Global.instance.StartCoroutine(OnSelectFade(1));
        }
        
        private IEnumerator OnSelectFade(float time)
        {
            yield return new WaitForSeconds(time);
            if(!head_root)
                yield break;
            head_root.gameObject.SetActive(true);
            head_root.Play("HeadIcon");
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
            EventCenter.ins.RemoveEventListener("move_camera_complete",OnMoveCameraCom);
            EventCenter.ins.RemoveEventListener("AnyKeyDown",AnyKeyDown);
            EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
        }
    }
}