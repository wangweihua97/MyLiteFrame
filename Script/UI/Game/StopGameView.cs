using DG.Tweening;
using Script.Game;
using Script.Main;
using Script.Model;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StopGameView : UView
    {
        public override bool DefaultShow => false;
        public Button ContinueGameBtn;
        public Button EndGameBtn;
        public Image SelectIcon;

        private bool isInit;

        public override void DoCreat()
        {
            base.DoCreat();
            GameUIMgr.StopGameView = this;
            isInit = false;
            ContinueGameBtn.onClick.AddListener( () =>
            {
                Show(false);
                MainGameCtr.instance.BackGame();
            });
            EndGameBtn.onClick.AddListener(() =>
            {
                MainGameCtr.instance.EndGame(GameState.Failure);
            });
            SelectIcon.transform.localPosition = ContinueGameBtn.transform.localPosition;
        }
        public override void Show(bool b = true)
        {
            base.Show(b);
            ContinueGameBtn.transform.localScale = Vector3.zero;
            EndGameBtn.transform.localScale = Vector3.zero;
            Tweener scale = ContinueGameBtn.transform.DOScale(new Vector3(1, 1, 1), 0.3f);
            scale.SetUpdate(true);
            Tweener scale2 = EndGameBtn.transform.DOScale(new Vector3(1, 1, 1), 0.3f);
            scale2.SetUpdate(true);
        }
        

        public void MoveTo(int index)
        {
            RectTransform target;
            switch (index)
            {
                case 0:
                    target = ContinueGameBtn.transform as RectTransform;
                    break;
                case 1:
                    target = EndGameBtn.transform as RectTransform;
                    break;
                default:
                    target = ContinueGameBtn.transform as RectTransform;
                    break;
            }
            Tweener move =SelectIcon.transform.DOLocalMove(target.localPosition, 0.5f);
            move.SetUpdate(true);
        }

        public override void DoDestory()
        {
            base.DoDestory();
        }
    }
}