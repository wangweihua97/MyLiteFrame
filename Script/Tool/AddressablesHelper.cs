using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class AddressablesHelper
{
    public static AddressablesHelper instance;
    public Dictionary<string, AsyncOperationHandle> Handles = new Dictionary<string, AsyncOperationHandle>();
    public Dictionary<AsyncOperationHandle, string> Handles2 = new Dictionary<AsyncOperationHandle, string>();

    /// <summary>
    /// 初始化
    /// </summary>
    public static void Init(Action callBack)
    {
        instance = new AddressablesHelper();
        AsyncOperationHandle<IResourceLocator> handle = Addressables.InitializeAsync();
        handle.Completed += operationHandle =>
        {
            callBack.Invoke();
        };
    }

    /// <summary>
    /// 判断是否包含key
    /// </summary>
    public bool ContainsKey(string key, string label = "")
    {
        return Handles.ContainsKey(key + label);
    }
    
    /// <summary>
    /// 判断是否包含Handle
    /// </summary>
    public bool ContainsHandle(AsyncOperationHandle handle)
    {
        return Handles2.ContainsKey(handle);
    }
    
    /// <summary>
    /// 由key值得到得到Handle
    /// </summary>
    public AsyncOperationHandle GetHandle(string key, string label = "")
    {
        return Handles[key + label];
    }
    
    
    /// <summary>
    /// 由Handle值得到得到key
    /// </summary>
    public string GetKey(AsyncOperationHandle handle)
    {
        return Handles2[handle];
    }

    /// <summary>
    /// 判断是否存在key的Addressables地址
    /// </summary>
    public bool IsAddressEnable(string key)
    {
        return true;
    }

    public AsyncOperationHandle<IList<IResourceLocation>> LoadResourceLocationsAsync(string label ,Addressables.MergeMode mode)
    {
        return Addressables.LoadResourceLocationsAsync(new object[]{
            "",label
        } , mode);
    }

    /// <summary>
    /// 载入Bundle
    /// </summary>
    public AsyncOperationHandle<IList<TObject>> LoadAssetsAsync<TObject>(string key, string label = "")
    {
        string handleKey = key + label;
        if (ContainsKey(handleKey))
            return Handles[handleKey].Convert<IList<TObject>>();
        List<object> list = new List<object> {key, label};
        if(label.Length <= 0)
            list =  new List<object> {key};
        AsyncOperationHandle<IList<TObject>> handle;
        handle = Addressables.LoadAssetsAsync<TObject>((IEnumerable)list, null ,Addressables.MergeMode.Intersection);
        
        Handles.Add(handleKey, handle);
        Handles2.Add(handle ,handleKey);
        return handle;
    }

    /// <summary>
    /// 载入Bundle
    /// </summary>
    public AsyncOperationHandle<TObject> LoadAssets<TObject>(string key)
    {
        AsyncOperationHandle<TObject> handle = Addressables.LoadAsset<TObject>(key);
        string handleKey = key;
        Handles.Add(handleKey, handle);
        Handles2.Add(handle ,handleKey);
        return handle;
    }
    
    /// <summary>
    /// 载入Bundle
    /// </summary>
    public AsyncOperationHandle<TObject> LoadAssets<TObject>(IResourceLocation key)
    {
        AsyncOperationHandle<TObject> handle = Addressables.LoadAsset<TObject>(key);
        string handleKey = key.PrimaryKey;
        Handles.Add(handleKey, handle);
        Handles2.Add(handle ,handleKey);
        return handle;
    }

    /// <summary>
    /// 加载场景
    /// </summary>
    public AsyncOperationHandle<SceneInstance> LoadSceneAsync(string sceneKey)
    {
        AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(sceneKey, LoadSceneMode.Additive);
        return handle;
    }

    /// <summary>
    /// 卸载场景
    /// </summary>
    public AsyncOperationHandle<SceneInstance> UnloadSceneAsync(SceneInstance loadedScene)
    {
        AsyncOperationHandle<SceneInstance> handle =Addressables.UnloadSceneAsync(loadedScene);
        return handle;
    }
    
    /// <summary>
    /// 释放Bundle
    /// </summary>
    public void Release(string key)
    {
        AsyncOperationHandle handle = Handles[key];
        if (handle.IsValid())
            Addressables.Release(handle);
        Handles.Remove(key);
        Handles2.Remove(handle);
    }

    /// <summary>
    /// 释放Bundle
    /// </summary>
    public void Release<T>(AsyncOperationHandle<T> handle)
    {
        if (Handles2.ContainsKey(handle))
        {
            Handles.Remove(Handles2[handle]);
            Handles2.Remove(handle);
        }
        if (handle.IsValid())
            Addressables.Release(handle);
    }
    
    /// <summary>
    /// 释放Bundle
    /// </summary>
    public void Release(AsyncOperationHandle handle)
    {
        if (Handles2.ContainsKey(handle))
        {
            Handles.Remove(Handles2[handle]);
            Handles2.Remove(handle);
        }
        if (handle.IsValid())
            Addressables.Release(handle);
    }
}
