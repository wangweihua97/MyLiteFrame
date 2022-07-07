using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Events;
using Examples.FirstScene;
using Examples.ReadCsv;
using OtherMgr;
using Script.DB;
using Script.Excel;
using Script.Excel.Table;
using Script.Main;
using Script.Main.Base;
using Script.Mgr;
using UnityEngine;

public class ReadCsvGlobal : MonoBehaviour
{
    // Start is called before the first frame update
        //public static LuaMgr LuaMgr;
        
        public static ReadCsvGlobal instance;
        public static ExcelMgr ExcelMgr;
        public CsvTable<TDTest> table;

        private void Start()
        {
            instance = this;
            Init();
            ExcelMgr = AddMgr<ExcelMgr>();
            GameFlowMgr.EnterGame.Invoke();
            
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            table = new CsvTable<TDTest>("TDTest" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add();
            gameFlowTaskGroup.Attach(GameFlowMgr.LoadedInitData);
        }

        public void Init()
        {
            DontDestroyOnLoad(gameObject);
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            AddressablesHelper.Init(gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Attach(GameFlowMgr.EnterGame);
            GameFlowMgr.LoadedInitData.AddListener(FirstInit);
            GameFlowMgr.LoadedInitData.AddListener(InitLoaodingUI);
            GameFlowMgr.LoadedInitData.AddListener(LoadedCsv);
            //SceneMgr.StartScene("TestScene",null);
        }

        void FirstInit()
        {
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

        void LoadedCsv()
        {
            foreach (var kvp in table.GetDictionary())
            {
                Debug.Log("Id为"+kvp.Key+"--------------------------------");
                Debug.Log("myInt"+kvp.Value.myInt);
                Debug.Log("myBool"+kvp.Value.myBool);
                Debug.Log("myDouble"+kvp.Value.myDouble);
                Debug.Log("myString"+kvp.Value.myString);
                Debug.Log("myListString"+kvp.Value.myListString);
                foreach (var str in kvp.Value.myListString)
                {
                    Debug.Log("    "+str);
                }
                Debug.Log("MyDictionary"+kvp.Value.MyDictionary);
                foreach (var kvp2 in kvp.Value.MyDictionary)
                {
                    Debug.Log("    "+kvp2.Key + ":  "+ kvp2.Value);
                }
                
            }
        }
        
}
