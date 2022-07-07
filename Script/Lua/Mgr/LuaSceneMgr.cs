using Script.Scene.Base;
using XLua;

namespace Script.Lua.Mgr
{
    public static class LuaSceneMgr
    {
        public static IScene GetScene(string name)
        {
            return LuaMgr.Instance.Luaenv.Global.GetInPath<IScene>(name);
        }
    }
}