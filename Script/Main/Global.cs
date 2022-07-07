using System;
using Events;
using OtherMgr;
using Script.DB;
using Script.Main.Base;
using Script.Mgr;
using UnitMgr;
using UnityEngine;

namespace Script.Main
{
    public class Global : MonoBehaviour
    {
        public static SceneMgr SceneMgr;
        public static InputMgr InputMgr;
        public static ExcelMgr ExcelMgr;
        //public static LuaMgr LuaMgr;
        
        public static Global instance;

        private void Awake()
        {
            instance = this;
        }

        public void Init()
        {
            DontDestroyOnLoad(gameObject);
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            AddressablesHelper.Init(gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Attach(GameFlowMgr.EnterGame);
            GameFlowMgr.LoadedInitData.AddListener(FirstInit);
            GameFlowMgr.LoadedInitData.AddListener(AddMgr);
            GameFlowMgr.LoadedInitData.AddListener(InitLoaodingUI);
            //SceneMgr.StartScene("TestScene",null);
        }

        public void LoadFirstScene()
        {
            SceneMgr.StartScene("LogoScene" ,null);
            //测试lua
            //SceneMgr.StartLuaScene("Test" ,null);
            //SceneMgr.EnterGameScene("C6");StartScene
        }

        void FirstInit()
        {
            DBManager.CheckTable();
        }
        
        void AddMgr()
        {
            SceneMgr = AddMgr<SceneMgr>();
            ExcelMgr = AddMgr<ExcelMgr>();
            //测试lua
            //LuaMgr = AddMgr<LuaMgr>();
            InputMgr = gameObject.AddComponent<InputMgr>();
            gameObject.AddComponent<AudioManager>();
            /*gameObject.AddComponent<JoyconInputMgr>();
            gameObject.AddComponent<LeftJoyconMgr>();
            gameObject.AddComponent<RightJoyconMgr>();*/
            gameObject.AddComponent<SpritesMgr>();
            WaitTimeMgr waitTimeMgr = new WaitTimeMgr();

            PoolManager.CreatPoolManager();
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
            RootUIMgr.instance.CreatUIMgr<LoadingViewMgr>(true);
        }
    }
}