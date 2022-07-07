using System;
using UnityEngine;
using XLua;

namespace Script.Lua.Mgr
{
    public static class LuaUIMgr
    {
        public static BaseLuaUIMgr GetUIMgr(string name)
        {
            GameObject GO = new GameObject();
            BaseLuaUIMgr baseLuaUiMgr = GO.AddComponent<BaseLuaUIMgr>();
            LuaTable luaTable = LuaMgr.Instance.Luaenv.Global.GetInPath<LuaTable>(name);
            baseLuaUiMgr.SetLuaTable(luaTable);
            return baseLuaUiMgr;
        }
    }

    public class BaseLuaUIMgr : UUIMgr
    {
        public override string Name { get; set; }
        private LuaTable _luaTable;

        protected override void DoUpdata()
        {
            base.DoUpdata();
            LuaDoUpdata?.Invoke(_luaTable);
        }
        private Action<LuaTable>  LuaDoUpdata;

        protected override void UnLoadData()
        {
            base.UnLoadData();
            LuaUnLoadData?.Invoke(_luaTable);
        }
        private Action<LuaTable>  LuaUnLoadData;
        
        protected override void UnLoadedScene()
        {
            base.UnLoadedScene();
            LuaUnLoadedScene?.Invoke(_luaTable);
        }
        private Action<LuaTable>  LuaUnLoadedScene;
        
        protected override void LoadScene()
        {
            base.LoadScene();
            LuaLoadScene?.Invoke(_luaTable);
        }
        private Action<LuaTable>  LuaLoadScene;

        protected override void LoadPrepareData70Per()
        {
            base.LoadPrepareData70Per();
            LuaLoadPrepareData70Per?.Invoke(_luaTable);
        }
        private Action<LuaTable>  LuaLoadPrepareData70Per;

        protected override void LoadPrepareData80Per()
        {
            base.LoadPrepareData80Per();
            LuaLoadPrepareData80Per?.Invoke(_luaTable);
        }
        private Action<LuaTable>  LuaLoadPrepareData80Per;

        protected override void LoadPrepareData90Per()
        {
            base.LoadPrepareData90Per();
            LuaLoadPrepareData90Per?.Invoke(_luaTable);
        }
        private Action<LuaTable>  LuaLoadPrepareData90Per;
        
        protected override void LoadedScene()
        {
            base.LoadedScene();
            LuaLoadedScene?.Invoke(_luaTable);
        }
        private Action<LuaTable>  LuaLoadedScene;

        protected override void EnterNewScene()
        {
            base.EnterNewScene();
            LuaEnterNewScene?.Invoke(_luaTable);
        }
        private Action<LuaTable>  LuaEnterNewScene;

        public override void DoCreat()
        {
            base.DoCreat();
            LuaDoCreat?.Invoke(_luaTable);
        }
        private Action<LuaTable>  LuaDoCreat;

        public override void DoDestroy()
        {
            base.DoDestroy();
            LuaDoDestroy?.Invoke(_luaTable);
            _luaTable.Dispose();
        }
        private Action<LuaTable>  LuaDoDestroy;

        public void SetLuaTable(LuaTable luaTable)
        {
            _luaTable = luaTable;
            luaTable.Set("base", this);
            Name = luaTable.Get<string>("Name");
            LuaDoUpdata = luaTable.Get<Action<LuaTable> >("DoUpdata");
            LuaUnLoadData = luaTable.Get<Action<LuaTable> >("UnLoadData");
            LuaUnLoadedScene = luaTable.Get<Action<LuaTable> >("UnLoadedScene");
            LuaLoadScene = luaTable.Get<Action<LuaTable> >("LoadScene");
            LuaLoadPrepareData70Per = luaTable.Get<Action<LuaTable> >("LoadPrepareData70Per");
            LuaLoadPrepareData80Per = luaTable.Get<Action<LuaTable> >("LoadPrepareData80Per");
            LuaLoadPrepareData90Per = luaTable.Get<Action<LuaTable> >("LoadPrepareData90Per");
            LuaLoadedScene = luaTable.Get<Action<LuaTable> >("LoadedScene");
            LuaEnterNewScene = luaTable.Get<Action<LuaTable> >("EnterNewScene");
            LuaDoCreat = luaTable.Get<Action<LuaTable> >("DoCreat");
            LuaDoDestroy = luaTable.Get<Action<LuaTable> >("DoDestroy");
        }
    }
}