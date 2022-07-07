using System.Collections;
using DG.Tweening;
using Events;
using Script.Main;
using Script.Model;
using UI.Base;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace UI.CreatePlayer
{
    public class SelectSexView : InOutUView
    {
        enum UIName
        {
            SelectUI,
            ConfirmUI
        }
        public override bool DefaultShow => false;

        [Header("选择性别UI")]
        [SerializeField] GameObject SelectUI;
        [Header("确认UI")]
        [SerializeField] GameObject ConfirmUI;

        [SerializeField] private Image famaleImage;
        
        [SerializeField] private Image maleImage;
        
        [SerializeField] private GameObject famaleBtn;
        
        [SerializeField] private GameObject maleBtn;
        
        [SerializeField] private GameObject famaleConfirm;
        
        [SerializeField] private GameObject maleConfirm;
        
        [SerializeField] private Text[] txtList;
        [SerializeField] private Image[] imgList;

        private UIName _uiName;
        private Sex _sex;
        
        public override void DoCreat()
        {
            base.DoCreat();
            CreatePlayerViewMgr.SelectSexView = this;
        }

        public override void DoOpen()
        {
            _uiName = UIName.SelectUI;
            _sex = Sex.Famale;
            ResetSexIconXY();
            Refresh();
            RefreshSelectUI(true);
            GraduallyShow();
            base.DoOpen();
        }

        protected override void enterStageComplete()
        {
            
            EventCenter.ins.AddEventListener<KeyCode>("KeyDown",KeyDown);
        }

        public override void DoDestory()
        {
            EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
            base.DoDestory();
        }

        private bool keyDownLock = false;
        void KeyDown(KeyCode keyCode)
        {
            if(!IsTop() || !IsActive() || keyDownLock)
                return;
            switch (keyCode)
            {
                case KeyCode.W:
                    break;
                case KeyCode.A:
                    if(_uiName != UIName.SelectUI)
                        return;
                    if(_sex == Sex.Famale)
                        return;
                    _sex = Sex.Famale;
                    RefreshSelectUI(false);
                    break;
                case KeyCode.S:
                    break;
                case KeyCode.D:
                    if(_uiName != UIName.SelectUI)
                        return;
                    if(_sex == Sex.Male)
                        return;
                    _sex = Sex.Male;
                    RefreshSelectUI(false);
                    break;
                case KeyCode.J:
                    if (_uiName == UIName.SelectUI)
                    {
                        keyDownLock = true;
                        // _uiName = UIName.ConfirmUI;
                        // Refresh();
                        // RefreshConfirmUI();
                        playAni(_sex == Sex.Male?"SelectSexView_confirm_w_in":"SelectSexView_confirm_m_in", false
                        , delegate
                        {
                            _uiName = UIName.ConfirmUI;
                            Refresh();
                            RefreshConfirmUI();
                            keyDownLock = false;
                        });//
                    }
                    else if (_uiName == UIName.ConfirmUI)
                    {
                        // playAni("SelectSexView_confirm_m_in");//女
                        // playAni("SelectSexView_confirm_w_in");//男
                        // Debug.Log("EventTrigger_1");
                        EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
                        playInOutAni(false, delegate()
                        {
                            DoClose();
                            EventCenter.ins.EventTrigger("create_player_step1");
                        });
                    }
                    break;
                case KeyCode.K:
                    if (_uiName == UIName.ConfirmUI)
                    {
                        keyDownLock = true;
                        _uiName = UIName.SelectUI;
                        //
                        // playAni(_sex == Sex.Male?"SelectSexView_confirm_w_in":"SelectSexView_confirm_m_in", true
                        // , () =>
                        // {
                        ResetAlpha1();
                        ResetSexIconXY();
                        Refresh();
                        RefreshConfirmUI();
                        RefreshSelectUI(true);
                        keyDownLock = false;
                        // });
                    }
                    // else
                    // {
                    //     StartCoroutine(OpenMainView());
                    // }
                    break;
            }
            
        }

        protected delegate void playAniCb();
        private playAniCb _playAniCb = null;
        void playAni(string actName, bool isBackPlay, playAniCb cb)
        {
            _playAniCb = cb;
            InOutAni.enabled = true;
            InOutAni.Rebind();
            
            // InOutAni.speed = isBackPlay ? -1f : 1f;
            // InOutAni.playbackTime = 1f;
            InOutAni.Play(actName,0,isBackPlay ? 1f : 0);
            // InOutAni.Play(actName);
            // if(isBackPlay)
            //     InOutAni.StartPlayback();
            // else
            //     InOutAni.Play(actName);
            InOutAni.SetFloat("Speed", isBackPlay ? -1f : 1f);
            // Playable playable = new Playable(InOutAni.GetCurrentAnimatorClipInfo(0).to);InOutAni.GetCurrentAnimatorClipInfo(0).to
            // InOutAni.speed
            Global.instance.StartCoroutine(playAniComplete(GetAniLength(actName)));
        }
        
        private IEnumerator playAniComplete(float time)
        {
            yield return new WaitForSeconds(time);
            //设置为第一帧属性
            // InOutAni.Play(0,0,0);//
            // InOutAni.speed = -1f;
            // InOutAni.StartPlayback();
            // //下一帧
            yield return new WaitForSeconds(0);
            //禁用-解锁控制属性
            InOutAni.enabled = false;
            if (null != _playAniCb)
            {
                playAniCb tmpCb = _playAniCb;
                _playAniCb = null;
                tmpCb();
            }
        }
        
        IEnumerator OpenMainView()
        {
            yield return 0;
            DoClose();
            HallViewMgr.MainView.DoOpen();
        }

        void Refresh()
        {
            switch (_uiName)
            {
                case UIName.SelectUI:
                    SelectUI.SetActive(true);
                    ConfirmUI.SetActive(false);
                    break;
                case UIName.ConfirmUI:
                    SelectUI.SetActive(false);
                    ConfirmUI.SetActive(true);
                    break;
            }
        }

        void RefreshConfirmUI()
        {
            switch (_sex)
            {
                case Sex.Famale:
                    famaleConfirm.SetActive(true);
                    maleConfirm.SetActive(false);
                    break;
                case Sex.Male:
                    famaleConfirm.SetActive(false);
                    maleConfirm.SetActive(true);
                    break;
            }
        }
        
        void RefreshSelectUI(bool isImmediately)
        {
            switch (_sex)
            {
                case Sex.Famale:
                    if (isImmediately)
                    {
                        famaleImage.transform.localScale = Vector3.one;
                        maleImage.transform.localScale = new Vector3(0.65f,0.65f,0.65f);
                        famaleImage.color = new Color(1f, 1f, 1f, 1f);
                        maleImage.color = new Color(1f, 1f, 1f, 0.5f);
                    }
                    else
                    {
                        maleImage.transform.DOScale(new Vector3(0.65f, 0.65f, 0.65f), 0.3f);
                        famaleImage.transform.DOScale(Vector3.one ,0.3f);
                        famaleImage.DOColor(new Color(1f, 1f, 1f, 1f), 0.3f);
                        maleImage.DOColor(new Color(1f, 1f, 1f, 0.5f), 0.3f);
                    }
                    famaleBtn.SetActive(true);
                    maleBtn.SetActive(false);
                    break;
                case Sex.Male:
                    if (isImmediately)
                    {
                        maleImage.transform.localScale = Vector3.one;
                        famaleImage.transform.localScale = new Vector3(0.65f,0.65f,0.65f);
                        maleImage.color = new Color(1f, 1f, 1f, 1f);
                        famaleImage.color = new Color(1f, 1f, 1f, 0.5f);
                    }
                    else
                    {
                        famaleImage.transform.DOScale(new Vector3(0.65f, 0.65f, 0.65f), 0.3f);
                        maleImage.transform.DOScale(Vector3.one ,0.3f);
                        //
                        maleImage.DOColor(new Color(1f, 1f, 1f, 1f), 0.3f);
                        famaleImage.DOColor(new Color(1f, 1f, 1f, 0.5f), 0.3f);
                    }
                    famaleBtn.SetActive(false);
                    maleBtn.SetActive(true);
                    break;
            }
        }

        void ResetSexIconXY()
        {
            maleImage.transform.localPosition = new Vector3(441,-92);
            famaleImage.transform.localPosition = new Vector3(192,-87);
        }

        void ResetAlpha1()
        {
            int len = txtList.Length;
            for (int i = 0; i < len; i++)
            {
                txtList[i].color = Color.white;
            }
            len = imgList.Length;
            for (int i = 0; i < len; i++)
            {
                imgList[i].color = Color.white;
            }
        }
    }
}