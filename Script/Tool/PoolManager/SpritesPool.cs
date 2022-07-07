using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Script.Tool.PoolManager.Model;
using UIModel;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

namespace Script.Tool.PoolManager
{
    public class SpritesPool
    {
        private Dictionary<string, SpriteAtlas> SpriteAtlas;
        private Dictionary<string, AsyncOperationHandle> AsyncOperationHandles;
        private Dictionary<string, SpritesModel> Sprites;
        public SpritesPool()
        {
            if(AsyncOperationHandles != null)
                ReleaseAll();
            SpriteAtlas = new Dictionary<string, SpriteAtlas>();
            Sprites = new Dictionary<string, SpritesModel>();
            AsyncOperationHandles = new Dictionary<string, AsyncOperationHandle>();
        }
        public async Task AddSpriteAtlas(string key ,Action callBack)
        {
            if (SpriteAtlas.ContainsKey(key))
            {
                callBack.Invoke();
                return;
            }
            AsyncOperationHandle<IList<SpriteAtlas>> handle= AddressablesHelper.instance.LoadAssetsAsync<SpriteAtlas>(key, "SpriteAtlas");
            await handle.Task;
            callBack.Invoke();
            AsyncOperationHandles.Add(key ,handle);
            SpriteAtlas.Add(key ,handle.Result[0]);
        }
        
        public async Task AddSprites(string key ,Action callBack)
        {
            if (Sprites.ContainsKey(key))
            {
                callBack.Invoke();
                return;
            }
            AsyncOperationHandle<IList<Sprite>> handle= AddressablesHelper.instance.LoadAssetsAsync<Sprite>(key);
            await handle.Task;
            SpritesModel spritesModel = new SpritesModel(handle);
            AsyncOperationHandles.Add(key ,handle);
            Sprites.Add(key ,spritesModel);
            callBack.Invoke();
        }

        public void Release(string key)
        {
            if (AsyncOperationHandles.ContainsKey(key))
            {
                AddressablesHelper.instance.Release(AsyncOperationHandles[key]);
                AsyncOperationHandles.Remove(key);
                SpriteAtlas.Remove(key);
            }
        }

        public SpriteAtlas GetSpriteAtlas(string key)
        {
            if (!SpriteAtlas.ContainsKey(key))
                return null;
            return SpriteAtlas[key];
        }
        
        public Sprite GetSprite(string key ,string name)
        {
            if (SpriteAtlas.ContainsKey(key))
                return SpriteAtlas[key].GetSprite(name);
            else if (Sprites.ContainsKey(key))
                return Sprites[key].Get(name);
            return null;
        }
        
        public bool ContainsKey(string key)
        {
            return SpriteAtlas.ContainsKey(key) || Sprites.ContainsKey(key);
        }

        public void ReleaseAll()
        {
            foreach (var kvp in AsyncOperationHandles)
            {
                AddressablesHelper.instance.Release(kvp.Value);
            }
        }
    }
    
}