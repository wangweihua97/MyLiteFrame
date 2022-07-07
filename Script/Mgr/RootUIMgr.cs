using System;
using System.Collections.Generic;
using Script.Lua.Mgr;
using Tool.Others;
using UnityEngine;

namespace Script.Mgr
{
    public class RootUIMgr : MonoBehaviour
    {
        public static RootUIMgr instance;

        private Dictionary<Type, UUIMgr> _dictionary;
        private DoubleLinkedList<UUIMgr> _linkedList;
        private void Awake()
        {
            instance = this;
            _dictionary = new Dictionary<Type, UUIMgr>();
            _linkedList = new DoubleLinkedList<UUIMgr>();
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            
        }

        public TUIMgr CreatUIMgr<TUIMgr>(bool isTop = false) where TUIMgr : UUIMgr
        {
            Type type = typeof(TUIMgr);
            GameObject GO = new GameObject();
            GO.transform.SetParent(transform);
            TUIMgr uiMgr =GO.AddComponent<TUIMgr>();
            uiMgr.RootIndex = _linkedList.Size;
            uiMgr.DoCreat();
            _dictionary.Add(type,uiMgr);
            if (!_linkedList.Contain(uiMgr))
            {
                if(isTop)
                    _linkedList.PushBack(uiMgr);
                else
                    _linkedList.PushHead(uiMgr);
            }
            return uiMgr;
        }

        public bool ContainsUIMgr<TUIMgr>() where TUIMgr : UUIMgr
        {
            Type type = typeof(TUIMgr);
            if (!_dictionary.ContainsKey(type))
            {
                return false;
            }
            return true;
        }

        public void Top<TUIMgr>()where TUIMgr : UUIMgr
        {
            Top(typeof(TUIMgr));
        }
        
        public void Top(Type type)
        {
            UUIMgr uiMgr = GetUIMgr(type);
            int index = _linkedList.Find(uiMgr);
            if (index != _linkedList.Size - 1)
            {
                _linkedList.MoveToBack(_linkedList.Find(uiMgr));
                uiMgr.RootIndex = _linkedList.Size - 1;
                Refresh();
            }
        }
        
        public void Head(Type type)
        {
            UUIMgr uiMgr = GetUIMgr(type);
            int index = _linkedList.Find(uiMgr);
            if (index != 0)
            {
                _linkedList.MoveToHead(_linkedList.Find(uiMgr));
                uiMgr.RootIndex = 0;
                Refresh();
            }
        }
        
        public bool IsTop<TUIMgr>()where TUIMgr : UUIMgr
        {
            return IsTop(typeof(TUIMgr));
        }
        
        public bool IsTop(Type type)
        {
            UUIMgr uiMgr = GetUIMgr(type);
            DoubleLinkedList<UUIMgr>.DoubleLinkedNode node = _linkedList.End;
            for (int i = 0; i < _linkedList.Size; i++)
            {
                if (node.Value.IsMask)
                {
                    if (node.Value.Equals(uiMgr))
                        return true;
                    return false;
                }
                node = node.Prior;
            }
            return false;
        }

        public UUIMgr GetTop()
        {
            DoubleLinkedList<UUIMgr>.DoubleLinkedNode node = _linkedList.End;
            for (int i = 0; i < _linkedList.Size; i++)
            {
                if (node.Value.IsMask)
                { 
                    return node.Value;
                }
                node = node.Prior;
            }
            return default;
        }

        public bool IsLayerTop<TUIMgr>(string sortingLayerName)where TUIMgr : UUIMgr
        {
            return IsLayerTop(typeof(TUIMgr), sortingLayerName);
        }
        
        public bool IsLayerTop(Type type ,string sortingLayerName)
        {
            UUIMgr uiMgr = GetUIMgr(type);
            DoubleLinkedList<UUIMgr>.DoubleLinkedNode node = _linkedList.End;
            for (int i = 0; i < _linkedList.Size; i++)
            {
                if (node.Value.IsMask && node.Value.sortingLayerName == sortingLayerName)
                {
                    if (node.Value.Equals(uiMgr))
                        return true;
                    return false;
                }
                node = node.Prior;
            }
            return false;
        }

        public int Index<TUIMgr>() where TUIMgr : UUIMgr
        {
            TUIMgr uiMgr = GetUIMgr<TUIMgr>();
            return _linkedList.Find(uiMgr);
        }

        public void Refresh()
        {
            int i = 0;
            foreach (var uuiMgr in _linkedList)
            {
                uuiMgr.RootIndex = i;
                uuiMgr.Refresh();
                i++;
            }
        }

        public TUIMgr GetUIMgr<TUIMgr>() where TUIMgr : UUIMgr
        {
            Type type = typeof(TUIMgr);
            if (!_dictionary.ContainsKey(type))
            {
                Debug.LogError("不存在"+type);
                return null;
                    
            }
            return _dictionary[type] as TUIMgr;
        }
        
        public UUIMgr GetUIMgr(Type type) 
        {
            if (!_dictionary.ContainsKey(type))
            {
                Debug.LogError("不存在"+type);
                return null;
                    
            }
            return _dictionary[type];
        }
        
        public void DestroyUIMgr<TUIMgr>() where TUIMgr : UUIMgr
        {
            Type type = typeof(TUIMgr);
            if (!_dictionary.ContainsKey(type))
            {
                Debug.LogError("不存在"+type);
                return;
            }
            TUIMgr uiMgr = _dictionary[type] as TUIMgr;
            _dictionary.Remove(type);
            _linkedList.Pop(uiMgr);
            uiMgr.DoDestroy();
        }

        #region Lua测试

        private Dictionary<string, BaseLuaUIMgr> _luaUiMgrs = new Dictionary<string, BaseLuaUIMgr>();

        public void CreatLuaUIMgr(string name)
        {
            BaseLuaUIMgr uiMgr = LuaUIMgr.GetUIMgr(name);
            uiMgr.transform.SetParent(transform);
            uiMgr.DoCreat();
            _luaUiMgrs.Add(name ,uiMgr);
        }
        
        public BaseLuaUIMgr GetLuaUIMgr(string name)
        {
            return _luaUiMgrs[name];
        }

        public void DestroyLuaUIMgr(string name)
        {
            _luaUiMgrs[name].DoDestroy();
            _luaUiMgrs.Remove(name);
        }
        
        public bool ContainsLuaUIMgr(string name)
        {
            return _luaUiMgrs.ContainsKey(name);
        }

        #endregion
    }
}