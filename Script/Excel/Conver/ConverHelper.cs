using System;
using System.Collections.Generic;
using System.Reflection;
using Script.Main;
using UnityEngine;

namespace Script.Excel.Conver
{
    public class ConverHelper
    {
        public string ToString(string str)
        {
            return str;
        }
        
        public int ToInt(string str)
        {
            int a = 0;
            try
            {
                a = Int32.Parse(str);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
            return a;
        }
        
        public float ToFloat(string str)
        {
            float a = 0;
            try
            {
                a = float.Parse(str);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
            return a;
        }
        
        public double ToDouble(string str)
        {
            double a = 0;
            try
            {
                a = double.Parse(str);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
            return a;
        }
        
        public bool ToBool(string str)
        {
            if (str.Length != 4 && str.Length != 1)
                return false;
            if (str.Equals("TRUE") || str.Equals("true") || str.Equals("1"))
                return true;
            return false;
        }

        public List<T> ToList<T>(string str)
        {
            
            List<T> list = new List<T>();
            if (str[0] == '[')
            {
                str = str.Substring(1,str.Length - 2);
            }
                
            string[] sArray=str.Split(',');
            Type type = typeof(T);
            Type typeList = list.GetType();
            MethodInfo mi = typeList.GetMethod("Add");
            if (type.FullName.Equals(typeof(int).FullName))
            {
                foreach (var a in sArray)
                {
                    mi.Invoke(list, new []{(object)ToInt(a)});
                }
            }
            else if(type.FullName.Equals(typeof(float).FullName))
            {
                foreach (var a in sArray)
                {
                    mi.Invoke(list, new []{(object)ToFloat(a)});
                }
            }
            else if(type.FullName.Equals(typeof(double).FullName))
            {
                foreach (var a in sArray)
                {
                    mi.Invoke(list, new []{(object)ToDouble(a)});
                }
            }
            else if(type.FullName.Equals(typeof(string).FullName))
            {
                foreach (var a in sArray)
                {
                    mi.Invoke(list, new []{(object)ToString(a)});
                }
            }
            else if(type.FullName.Equals(typeof(bool).FullName))
            {
                foreach (var a in sArray)
                {
                    mi.Invoke(list, new []{(object)ToBool(a)});
                }
            }
            return list;
        }

        public Dictionary<string ,V> ToDictionary<V>(string str)
        {
            Dictionary<string ,V> dictionary = new Dictionary<string ,V>();
            
            if (str[0] == '[')
            {
                str = str.Substring(1,str.Length - 2);
            }
            
            string[] sArray=str.Split(',');
            Type type = typeof(V);
            Type dictionaryList = dictionary.GetType();
            MethodInfo mi = dictionaryList.GetMethod("Add");
            if (type.FullName.Equals(typeof(int).FullName))
            {
                foreach (var a in sArray)
                {
                    string[] kv = a.Split(':');
                    if(kv.Length < 2)
                    {
                        mi.Invoke(dictionary, new []{(object)kv[0],(object)Const.NULL_INT});
                    }
                    else
                    {
                        mi.Invoke(dictionary, new []{(object)kv[0],(object)ToInt(kv[1])});
                    }
                }
            }
            else if(type.FullName.Equals(typeof(float).FullName))
            {
                foreach (var a in sArray)
                {
                    string[] kv = a.Split(':');
                    if(kv.Length < 2)
                    {
                        mi.Invoke(dictionary, new []{(object)kv[0],(object)Const.NULL_FLOAT});
                    }
                    else
                    {
                        mi.Invoke(dictionary, new []{(object)kv[0],(object)ToFloat(kv[1])});
                    }
                }
            }
            else if(type.FullName.Equals(typeof(double).FullName))
            {
                foreach (var a in sArray)
                {
                    string[] kv = a.Split(':');
                    if(kv.Length < 2)
                    {
                        mi.Invoke(dictionary, new []{(object)kv[0],(object)Const.NULL_FLOAT});
                    }
                    else
                    {
                        mi.Invoke(dictionary, new []{(object)kv[0],(object)ToDouble(kv[1])});
                    }
                }
            }
            else if(type.FullName.Equals(typeof(string).FullName))
            {
                foreach (var a in sArray)
                {
                    string[] kv = a.Split(':');
                    if(kv.Length < 2)
                        mi.Invoke(dictionary, new []{(object)kv[0],(object)Const.NULL_STRING});
                    else
                        mi.Invoke(dictionary, new []{(object)kv[0],(object)ToString(kv[1])});
                }
            }
            else if(type.FullName.Equals(typeof(bool).FullName))
            {
                foreach (var a in sArray)
                {
                    string[] kv = a.Split(':');
                    if(kv.Length < 2)
                        mi.Invoke(dictionary, new []{(object)kv[0],(object)false});
                    else
                        mi.Invoke(dictionary, new []{(object)kv[0],(object)ToBool(kv[1])});
                }
            }
            return dictionary;
        }
    }
}