using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Script.Tool.PoolManager.Model
{
    public class SpritesModel
    {
        public Dictionary<string, Sprite> Sprites;

        public SpritesModel(AsyncOperationHandle<IList<Sprite>> handle)
        {
            Sprites = new Dictionary<string, Sprite>();
            foreach (var sprite in handle.Result)
            {
                Sprites.Add(sprite.name ,sprite);
            }
           
        }

        public Sprite Get(string key)
        {
            return Sprites[key];
        }

        string GetFileName(string key)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool isRecord = false;
            for (int i = key.Length - 1; i > 0; i--)
            {
                if (key[i] == '.')
                {
                    isRecord = true;
                }

                if (isRecord)
                {
                    stringBuilder.Insert(0, key[i]);
                }
                
                if(key[i] == '/')
                    break;
            }
            return stringBuilder.ToString();
        }
    }
}