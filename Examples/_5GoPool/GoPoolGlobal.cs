using System.Collections.Generic;
using Events;
using Examples.GoPool;
using Script.Main.Base;
using Script.Mgr;
using UnityEngine;

public class GoPoolGlobal : MonoBehaviour
{
    // Start is called before the first frame update
        //public static LuaMgr LuaMgr;
        
        public static GoPoolGlobal instance;
        private void Start()
        {
            instance = this;
            Init();
            GoPoolMgr.CreatPoolManager();
            GameFlowMgr.EnterGame.AddListener(PoolInit);
            GameFlowMgr.EnterGame.Invoke();
            
            
            
            
        }

        public void Init()
        {
            DontDestroyOnLoad(gameObject);
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            AddressablesHelper.Init(gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Attach(GameFlowMgr.EnterGame);
            GameFlowMgr.LoadedInitData.AddListener(InitLoaodingUI);
            //SceneMgr.StartScene("TestScene",null);
        }

        void PoolInit()
        {
            string[] paths = new[] {"CubeGo", "SphereGo"};
            GoPoolMgr.GoPool.poolCreatMultiGoHelper.AddGameObject(new List<string>(paths),GameFlowMgr.LoadedInitData, () =>
            {
                ;
            });
        }
        void InitLoaodingUI()
        {
            ;
        }
        
        T AddMgr<T>() where T : BaseGameFlow
        {
            GameObject go = new GameObject(typeof(T).Name);
            T t = go.AddComponent<T>();
            t.transform.SetParent(transform);
            return t;
        
        }
        
}
