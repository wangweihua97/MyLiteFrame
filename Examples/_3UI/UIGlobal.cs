using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Events;
using Examples.FirstScene;
using Examples.ProgressBar;
using Examples.UI;
using OtherMgr;
using Script.DB;
using Script.Main;
using Script.Main.Base;
using Script.Mgr;
using UnityEngine;

public class UIGlobal : MonoBehaviour
{
    // Start is called before the first frame update
    public static UISceneMgr UISceneMgr;
        //public static LuaMgr LuaMgr;
        
        public static UIGlobal instance;

        private void Start()
        {
            instance = this;
            Init();
            GameFlowMgr.EnterGame.Invoke();
        }

        public void Init()
        {
            DontDestroyOnLoad(gameObject);
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            AddressablesHelper.Init(gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Attach(GameFlowMgr.EnterGame);
            GameFlowMgr.LoadedInitData.AddListener(FirstInit);
            GameFlowMgr.LoadedInitData.AddListener(AddMgr);
            GameFlowMgr.EnterGame.AddListener(InitLoaodingUI);
            GameFlowMgr.LoadedInitData.AddListener(LoadFirstScene);
            //SceneMgr.StartScene("TestScene",null);
        }

        void LoadFirstScene()
        {
            UISceneMgr.StartScene("UIScene" ,UILoadingViewMgr.UILoadingView);
            //测试lua
            //SceneMgr.StartLuaScene("Test" ,null);
            //SceneMgr.EnterGameScene("C6");StartScene
        }

        void FirstInit()
        {
        }
        
        void AddMgr()
        {
            UISceneMgr = AddMgr<UISceneMgr>();
        }
        
        T AddMgr<T>() where T : BaseGameFlow
        {
            GameObject go = new GameObject(typeof(T).Name);
            T t = go.AddComponent<T>();
            t.transform.SetParent(transform);
            return t;
        }

        void InitLoaodingUI()
        {
            RootUIMgr.instance.CreatUIMgr<UILoadingViewMgr>(true);
        }
}
