using System;
using System.Collections.Generic;
using System.Reflection;
using Script.Lua.Mgr;
using Script.Mgr;
using UI.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Script.Main.Base
{
    public class BaseUIMgr : BaseGameFlow
    {
        public virtual string Name { get; set; }
        public virtual string sortingLayerName { get; set; }
        public virtual bool IsMask
        {
            get { return true; }
        }
        

        protected Dictionary<string ,UView> _list = new Dictionary<string, UView>();

        Dictionary<string ,AsyncOperationHandle> UIMgrHandles = new Dictionary<string, AsyncOperationHandle>();
        protected void Add<TUI>(string ui,string label,Action callBack ,Action failCallBack) where TUI : UView
        {
            AsyncOperationHandle<IList<GameObject>> handle = AddressablesHelper.instance.LoadAssetsAsync<GameObject>(ui ,label );
            handle.Completed += obj=>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    //GameObject Go = obj.Result[0];
                    GameObject Go = Instantiate(obj.Result[0]);
                    TUI uiComponent = Go.GetComponent<TUI>();
                    CreatUI<TUI>(uiComponent ,ui + "/" + label);
                    callBack.Invoke();
                }
                else
                {
                    if(failCallBack != null)
                        failCallBack.Invoke();
                }
            };
            UIMgrHandles.Add(ui + "/" + label , handle);
        }

        #region Lua测试
        protected void AddLua(string ui,string label,Action callBack ,Action failCallBack)
        {
            AsyncOperationHandle<IList<GameObject>> handle = AddressablesHelper.instance.LoadAssetsAsync<GameObject>(ui ,label );
            handle.Completed += obj=>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    //GameObject Go = obj.Result[0];
                    GameObject Go = Instantiate(obj.Result[0]);
                    LuaUIView uiComponent = Go.GetComponent<LuaUIView>();
                    uiComponent.SetLuaTable(ui);
                    CreatUI(uiComponent ,ui + "/" + label);
                    callBack.Invoke();
                }
                else
                {
                    if(failCallBack != null)
                        failCallBack.Invoke();
                }
            };
            UIMgrHandles.Add(ui + "/" + label , handle);
        }
        #endregion
    
        public virtual void DoCreat()
        {
            gameObject.name = Name;
            transform.localPosition = new Vector3(0,0,-1);
            transform.localScale = new Vector3(1,1,1);
            RectTransform rectTransform =gameObject.AddComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = new Vector2(1,1);
            rectTransform.offsetMin = new Vector2(0.0f, 0.0f);
            rectTransform.offsetMax = new Vector2(0.0f, 0.0f);
        }

        public UView GetUIView(string ui, string label)
        {
            return _list[ui + "/" + label];
        }

        public AsyncOperationHandle GetHandle(string ui, string label)
        {
            return UIMgrHandles[ui + "/" + label];
        }

        public virtual void CreatUI<TUI>(TUI UI ,string UIName) where TUI : UView
        {
            UI.transform.SetParent(transform);
            UI.transform.localScale = new Vector3(1,1,1);
            Canvas canvas = UI.gameObject.GetComponent<Canvas>();
            if (canvas)
            {
                Debug.LogError("不要在View中添加Canvas");
            }
            else
            {
                canvas = UI.gameObject.AddComponent<Canvas>();
            }
            if(!UI.gameObject.GetComponent<GraphicRaycaster>())
                UI.gameObject.AddComponent<GraphicRaycaster>();
            canvas.overrideSorting = true;
            canvas.sortingLayerName = sortingLayerName;
            _list.Add(UIName ,UI);
        }
        
        public virtual void DoDestroy()
        {
            foreach (var uiView in _list)
            {
                uiView.Value.DoDestory();
            }
            foreach (var hand in UIMgrHandles)
            {
                AddressablesHelper.instance.Release(hand.Value);
            }
            _list.Clear();
            UIMgrHandles.Clear();
            Destroy(gameObject);
        }
    }
}