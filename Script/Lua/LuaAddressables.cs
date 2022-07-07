using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Script.Lua
{
    public class LuaAddressables
    {
        private AsyncOperationHandle _handle;
        private Dictionary<string, TextAsset> _textAssets;
        private Dictionary<string, AsyncOperationHandle> _handles;
        private Action _callBack;
        public int Count;

        public LuaAddressables(Action callBack)
        {
            _textAssets = new Dictionary<string, TextAsset>();
            _handles = new Dictionary<string, AsyncOperationHandle>();
            _callBack = callBack;
            InitTextAssets();
        }

        public TextAsset Get(string key)
        {
            return _textAssets[key];
        }
        
        public void Release(string key)
        {
            if (_textAssets.ContainsKey(key))
            {
                AddressablesHelper.instance.Release(key);
                _textAssets.Remove(key);
                _handles.Remove(key);
            }
        }
        
        private async Task InitTextAssets()
        {
            AsyncOperationHandle<IList<IResourceLocation>> handle= AddressablesHelper.instance.LoadResourceLocationsAsync("Lua" ,Addressables.MergeMode.Union);
            await handle.Task;
            Count = handle.Result.Count();
            int completeCount = 0;
            foreach (var iResourceLocation in handle.Result)
            {
                var handleItem = AddressablesHelper.instance.LoadAssets<TextAsset>(iResourceLocation);
                _handles.Add(iResourceLocation.PrimaryKey ,handleItem);
                handleItem.Completed += obj =>
                {
                    _textAssets.Add(iResourceLocation.PrimaryKey ,obj.Result);
                    completeCount++;
                    if(completeCount >= Count)
                        _callBack.Invoke();
                };
            }
        }
    }
}