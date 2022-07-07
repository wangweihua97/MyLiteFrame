using System;
using Script.Excel.Table;
using Script.Mgr;
using Script.Tool.PoolManager;
using UIModel;
using UnityEngine;
using UnityEngine.U2D;

namespace OtherMgr
{
    public class SpritesMgr : MonoBehaviour
    {
        void Awake()
        {
            PoolManager.SpritesPool = new SpritesPool();
        }
        
        public static void AddSpriteAtlas(string key, Action callBack)
        {
            PoolManager.SpritesPool.AddSpriteAtlas("SpriteAtlas/" + key, callBack);
        }
        
        public static void AddSprites(string key, Action callBack)
        {
            PoolManager.SpritesPool.AddSprites(key, callBack);
        }
        
        public static Sprite Get(string key ,string name)
        {
            if(PoolManager.SpritesPool.ContainsKey("SpriteAtlas/" + key))
                return PoolManager.SpritesPool.GetSprite("SpriteAtlas/" + key, name);
            else if(PoolManager.SpritesPool.ContainsKey(key))
                return PoolManager.SpritesPool.GetSprite(key, name);
            return null;
        }
        
        public static SpriteModel Get(string name)
        {
            /*TDIcon tdIcon = ExcelMgr.TDIcon.Get(name);
            if(tdIcon.FolderName == null)
                return new SpriteModel(null ,false);
            return new SpriteModel(Get(tdIcon.FolderName, tdIcon.FileName) ,tdIcon.IsOverturn);;*/
            return default;
        }
        
        public static SpriteAtlas GetSpriteAtlas(string key)
        {
            return PoolManager.SpritesPool.GetSpriteAtlas("SpriteAtlas/" + key);
        }

        public static void RemoveSpriteAtlas(string key)
        {
            PoolManager.SpritesPool.Release("SpriteAtlas/" + key);
        }
        
        public static void RemoveAll()
        {
            PoolManager.SpritesPool.ReleaseAll();
        }
    }
}