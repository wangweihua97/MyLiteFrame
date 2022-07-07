using DG.Tweening;
using OtherMgr;
using Script.Main;
using UI.Common.Item;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Common
{
    public class PopupFrame : UView
    {
        public override bool DefaultShow => false;
        [SerializeField] private PopupItem PopupItem;
        
        private Coroutine _graduallyShowCoroutine;
        private Coroutine _showCoroutine;
        private Coroutine _graduallyVanishCoroutine;
        private TipsPlayState _tipsPlayState = TipsPlayState.None;
        /// <summary>
        /// 页面创建时执行
        /// </summary>
        public override void DoCreat()
        {
            base.DoCreat();
            CommonUIMgr.PopupFrame = this;
        }

        public void ShowTips(string content ,float time = 2f)
        {
            if (_tipsPlayState != TipsPlayState.None)
            {
                ClearAllCoroutine();
                for (int i = 0; i < PopupItem.transform.childCount; i++)
                {
                    PopupItem.transform.GetChild(i).DOKill(false);
                }
            }
            else
            {
                DoOpen();
            }

            _tipsPlayState = TipsPlayState.PlayGraduallyShow;
            PopupItem.SetText(content);
            GraduallyShow(PopupItem.gameObject ,0.5f ,1f);
            
            _graduallyShowCoroutine = WaitTimeMgr.WaitTime(0.5f, () =>
            {
                GraduallyShowComplected();
            });
        }

        void GraduallyShowComplected()
        {
            _graduallyShowCoroutine = null;
            _tipsPlayState = TipsPlayState.Show;
            _showCoroutine = WaitTimeMgr.WaitTime(2f, () =>
            {
                ShowComplected();
            });
        }

        void ShowComplected()
        {
            _showCoroutine = null;
            _tipsPlayState = TipsPlayState.PlayGraduallyVanish;
            GraduallyVanish(PopupItem.gameObject);
            _graduallyVanishCoroutine = WaitTimeMgr.WaitTime(0.5f, () =>
            {
                GraduallyVanishComplected();
            });
        }
        

        void GraduallyVanishComplected()
        {
            _graduallyVanishCoroutine = null;
            _tipsPlayState = TipsPlayState.None;
            PopupItem.gameObject.SetActive(false);
            DoClose();
        }

        void ClearAllCoroutine()
        {
            WaitTimeMgr.CancelWait(ref _graduallyShowCoroutine);
            WaitTimeMgr.CancelWait(ref _showCoroutine);
            WaitTimeMgr.CancelWait(ref _graduallyVanishCoroutine);
        }
        
        /// <summary>
        /// 页面打开时
        /// </summary>
        public override void DoOpen()
        {
            base.DoOpen();
        }
        
        /// <summary>
        /// 页面关闭时
        /// </summary>
        public override void DoClose()
        {
            base.DoClose();
        }

        /// <summary>
        /// 页面摧毁时执行
        /// </summary>
        public override void DoDestory()
        {
            base.DoDestory();
        }
    }

    enum TipsPlayState
    {
        None,
        PlayGraduallyShow,
        Show,
        PlayGraduallyVanish
    }
}