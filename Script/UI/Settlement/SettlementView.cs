using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Events;
using Script.Game;
using Script.Main;
using Script.Model;
using OtherMgr;
using UI.Base;

namespace UI.SettlementView
{
    /// <summary> 结算界面 </summary>
    public class SettlementView : UView
    {

        public override bool DefaultShow => false;

        /// <summary> 数据面板根节点 </summary>
        public Transform DataPanelRoot;

        /// <summary> 不错 </summary>
        public Text GoodCount;
        /// <summary> 完美 </summary>
        public Text PerfectCount;
        /// <summary> 未命中 </summary>
        public Text MissCount;
        /// <summary> 完美连击 </summary>
        public Text PerfectComboCount;
        /// <summary> combo次数 </summary>
        public Text ComboCount;
        /// <summary> 游玩时间 </summary>
        public Text PlayTime;
        /// <summary> 出拳数 </summary>
        public Text PunchesNumber;
        /// <summary> 消耗卡路里 </summary>
        public Text BurnCalories;
        /// <summary> 最终得分 </summary>
        public Text Score;
        /// <summary> 身体年龄 </summary>
        public Text BodyAge;

        /// <summary> 4类评级百分比圆圈（S/A/B/C） </summary>
        public GameObject[] Grade;
        private int grade = 0;

        /// <summary> 右下角的返回按键提示 </summary>
        public CanvasGroup ReturnKeyHint;

        private bool showOver = false;

        public override void DoCreat()
        {
            base.DoCreat();
            SettlementUIMgr.SettlementView = this;
            
            //gameObject.transform.parent.position = new Vector3(1000,0,0);

            /*Camera a = gameObject.transform.parent.gameObject.GetComponent<RootViewMgr>().Camera;
            a.orthographic = false;//透视相机
            a.fieldOfView = 45;
            a.transform.localPosition = new Vector3(0, 0, -1304);*/
        }

        public override void Show(bool show)
        {
            base.Show(show);
            if (show)
            {
                AudioManager.PlayBackground("结算音效");
                EventCenter.ins.AddEventListener<KeyCode>("KeyDown", KeyDown);
                //EventCenter.ins.AddEventListener("GameOver", GameOver);
                InitViewSet();
                Global.instance.StartCoroutine(TempDynamicEffects());
            }
            
        }

        /**初始化页面设置*/
        private void InitViewSet()
        {
            DataPanelRoot.localEulerAngles = new Vector3(0, 120, 0);

            ReturnKeyHint.alpha = 0;

            GradeHelper gh = null;
            int goodCount = 14;
            int perfectCount = 63;
            int missCount = 5;
            int perfectComboCount = 80;
            int comboCount = 10;
            int playTime = 325;
            int punchesNumber = 96;
            float burnCalories = 9527;
            int score = 12138;
            int bodyAge = 28;
            if (MainGameCtr.instance != null)
            {
                gh = MainGameCtr.instance.GradeHelper;
            }
            if(gh != null){
                goodCount = gh.veryGoodCount;
                perfectCount = gh.perfectCount;
                missCount = gh.missCount;
                perfectComboCount = gh.maxPerfectTime;
                comboCount = MainGameCtr.instance.ComboHelper.MaxComboCount;
                playTime = (int)gh.gameTime;
                punchesNumber = gh.actNumber;
                burnCalories = gh.curCalorie;
                score = gh.curScore;
                bodyAge = gh.bodyAge;
                grade = (int)gh.gradeType;
                if (grade >= Grade.Length)
                {
                    grade = (int)GradeType.Correct;
                }
            } 

            GoodCount.transform.localPosition += new Vector3(0, 50, 0);
            GoodCount.GetComponent<CanvasGroup>().alpha = 0;
            GoodCount.text = goodCount.ToString();

            PerfectCount.transform.localPosition += new Vector3(0, 50, 0);
            PerfectCount.GetComponent<CanvasGroup>().alpha = 0;
            PerfectCount.text = perfectCount.ToString();

            MissCount.transform.localPosition += new Vector3(0, 50, 0);
            MissCount.GetComponent<CanvasGroup>().alpha = 0;
            MissCount.text = missCount.ToString();

            PerfectComboCount.transform.localPosition += new Vector3(0, 50, 0);
            PerfectComboCount.GetComponent<CanvasGroup>().alpha = 0;
            PerfectComboCount.text = perfectComboCount.ToString();
            
            ComboCount.transform.localPosition += new Vector3(0, 50, 0);
            ComboCount.GetComponent<CanvasGroup>().alpha = 0;
            ComboCount.text = comboCount.ToString();

            PlayTime.transform.localPosition += new Vector3(0, 50, 0);
            PlayTime.GetComponent<CanvasGroup>().alpha = 0;
            int minute = (int)((int)playTime / 60);
            int second = (int)playTime % 60;
            PlayTime.text = (minute < 10 ? "0" : "") + minute + ":" + (second < 10 ? "0" : "") + second;

            PunchesNumber.transform.localPosition += new Vector3(0, 50, 0);
            PunchesNumber.GetComponent<CanvasGroup>().alpha = 0;
            PunchesNumber.text = punchesNumber + "次";

            BurnCalories.transform.localPosition += new Vector3(0, 50, 0);
            BurnCalories.GetComponent<CanvasGroup>().alpha = 0;
            BurnCalories.text = burnCalories.ToString("F2") + "kcal";

            Score.transform.parent.GetComponent<CanvasGroup>().alpha = 0;
            Score.color = new Color(1, 1, 1, 0);
            Score.text = score.ToString();

            BodyAge.transform.parent.GetComponent<CanvasGroup>().alpha = 0;
            BodyAge.color = new Color(1, 1, 1, 0);
            BodyAge.text = bodyAge.ToString();

            
            for (int i = 0; i < Grade.Length; i++)
            {
                Grade[i].SetActive(false);
            }
        }

        private IEnumerator TempDynamicEffects()
        {
            DataPanelRoot.DORotate(new Vector3(0, 15, 0), 0.8f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(0.5f);

            Global.instance.StartCoroutine(ShowData_Fall(GoodCount));
            yield return new WaitForSeconds(0.1f);

            Global.instance.StartCoroutine(ShowData_Fall(PerfectCount));
            yield return new WaitForSeconds(0.1f);

            Global.instance.StartCoroutine(ShowData_Fall(MissCount));
            yield return new WaitForSeconds(0.1f);

            Global.instance.StartCoroutine(ShowData_Fall(PerfectComboCount));
            yield return new WaitForSeconds(0.1f);
            
            Global.instance.StartCoroutine(ShowData_Fall(ComboCount));
            yield return new WaitForSeconds(0.1f);

            Global.instance.StartCoroutine(ShowData_Fall(PlayTime));
            yield return new WaitForSeconds(0.1f);

            Global.instance.StartCoroutine(ShowData_Fall(PunchesNumber));
            yield return new WaitForSeconds(0.1f);

            Global.instance.StartCoroutine(ShowData_Fall(BurnCalories));
            yield return new WaitForSeconds(0.1f);

            Global.instance.StartCoroutine(ShowData_Pat(Score, new Color(1, 1, 1, 1)));
            yield return new WaitForSeconds(0.1f);

            Global.instance.StartCoroutine(ShowData_Pat(BodyAge, new Color(75 / 255f, 246 / 255f, 181 / 255f, 1)));
            yield return new WaitForSeconds(0.1f);

            float ScorePercent = 82f / 100f;
            Image Percent = Grade[grade].transform.Find("Percent").GetComponent<Image>();
            Percent.fillAmount = 0;
            Percent.DOColor(new Color(1, 1, 1, 1), 0.1f);
            Transform GradeIcon = Grade[grade].transform.Find("Grade");
            Image GradeIconImg = GradeIcon.GetComponent<Image>();
            GradeIconImg.color = new Color(1, 1, 1, 0);
            Grade[grade].SetActive(true);
            while (true)
            {
                Percent.fillAmount += Time.deltaTime / 0.2f;
                if (Percent.fillAmount >= ScorePercent)
                {
                    GradeIcon.localScale = Vector3.one * 5;
                    break;
                }

                yield return new WaitForEndOfFrame();
            }
            GradeIcon.DOScale(Vector3.one, 0.4f).SetEase(Ease.InQuint);
            GradeIconImg.DOColor(new Color(1, 1, 1, 1), 0.4f).SetEase(Ease.InQuint);

            yield return new WaitForSeconds(0.4f);
            while (true)
            {
                ReturnKeyHint.alpha += Time.deltaTime / 0.2f;
                if (ReturnKeyHint.alpha >= 1)
                {
                    showOver = true;
                    break;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator ShowData_Fall(Text comp)
        {
            CanvasGroup cg = comp.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                comp.transform.DOLocalMove(comp.transform.localPosition - new Vector3(0, 50, 0), 0.2f).SetEase(Ease.OutQuad);

                cg.alpha = 0;
                while (true)
                {
                    cg.alpha += Time.deltaTime / 0.2f;
                    if (cg.alpha >= 1)
                    {
                        yield break;
                    }

                    yield return new WaitForEndOfFrame();
                }
            }
        }

        private IEnumerator ShowData_Pat(Text comp, Color finalColor)
        {

            CanvasGroup cg = comp.transform.parent.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 0;
                while (true)
                {
                    cg.alpha += Time.deltaTime / 0.2f;
                    if (cg.alpha >= 1)
                    {
                        comp.transform.localScale = Vector3.one * 3;
                        break;
                    }

                    yield return new WaitForEndOfFrame();
                }

                comp.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutQuart);
                comp.DOColor(finalColor, 0.2f).SetEase(Ease.OutQuart);
            }
        }

        void KeyDown(KeyCode keyCode)
        {
            if(!IsTop() || !IsActive())
                return;
            switch (keyCode)
            {
                case KeyCode.J:
                    if (showOver)
                    {
                        GameVariable.GameState = GameState.CompletelyEnd;
                        DoClose();
                        SettlementUIMgr.ResultView.DoOpen();
                        EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown", KeyDown);
                        // 
                        // Global.SceneMgr.StartScene("HallScene" ,LoadingViewMgr.LoadingView);
                    }
                    break;
            }
        }

        //void GameOver()
        //{
        //    Show();
        //}

        public override void DoDestory()
        {
            //EventCenter.ins.RemoveEventListener("GameOver", GameOver);
            base.DoDestory();
        }
    }
}