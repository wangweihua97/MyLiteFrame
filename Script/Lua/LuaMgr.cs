using System;
using System.Text;
using Events;
using Script.Lua;
using Script.Main.Base;
using Script.Mgr;
using UI.Loading;
using XLua;

public class LuaMgr : BaseGameFlow
{
    public static LuaMgr Instance;
    private LuaAddressables _luaAddressables;
    public LuaEnv Luaenv;
    private bool _isInit = false;
    protected override void DoAwake()
    {
        base.DoAwake();
        Instance = this;
        Luaenv = new LuaEnv();
        Luaenv.AddLoader(CustomLoader);
        GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
        gameFlowTaskGroup.Add();
        _luaAddressables = new LuaAddressables(() =>
        {
            gameFlowTaskGroup.CompleteATask();
        });
        gameFlowTaskGroup.Attach(GameFlowMgr.LoadedInitData);
    }

    protected override void StartNewScene()
    {
        base.StartNewScene();
        if (!_isInit)
        {
            _isInit = true;
            Init();
        }
    }
    
    protected override void DoUpdata()
    {
        base.DoUpdata();
        if(Luaenv != null)
            Luaenv.Tick();
    }

    protected override void DoDestroy()
    {
        if (Luaenv != null)
        {
            Luaenv.Dispose();
        }
    }

    protected override void UnLoadData()
    {
        base.UnLoadData();
        Luaenv.FullGc();
    }

    void Init()
    {
        Luaenv.DoString("require 'Init'");
    }
    
    public  byte[] CustomLoader(ref string filepath)
    {
        StringBuilder scriptPath = new StringBuilder("Lua/");
        scriptPath.Append(filepath.Replace(".", "/")).Append(".lua.txt");
        return ASCIIEncoding.UTF8.GetBytes(_luaAddressables.Get(scriptPath.ToString()).text);
    }
}