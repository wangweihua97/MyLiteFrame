using UnityEditor;
using UnityEngine;

namespace Script.Tool
{
    public static class NativeStoreTool
    {
        public static bool HasKey(string name)
        {
            return PlayerPrefs.HasKey(name);
        }

        public static void Set(string name ,int value)
        {
            PlayerPrefs.SetInt(name , value);
        }
        
        public static void Set<T>(string name ,T value)
        {
            string str = JsonUtility.ToJson(value);
            PlayerPrefs.SetString(name , str);
        }
        
        public static void Set(string name ,string value)
        {
            PlayerPrefs.SetString(name , value);
        }

        public static void Set(string name ,float value)
        {
            PlayerPrefs.SetFloat(name , value);
        }

        public static T Get<T>(string name) where T : class
        {
            if(!HasKey(name))
                return null;
            T tJson = JsonUtility.FromJson<T>(PlayerPrefs.GetString(name));
            return tJson;
        }
        
        public static int GetInt(string name)
        {
            return PlayerPrefs.GetInt(name);
        }
        
        public static float GetFloat(string name)
        {
            return PlayerPrefs.GetFloat(name);
        }
        
        public static string GetString(string name)
        {
            return PlayerPrefs.GetString(name);
        }
    }
}