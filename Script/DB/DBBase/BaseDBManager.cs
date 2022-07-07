using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SQLite4Unity3d;
using UnityEngine;
using static System.IO.File;

namespace Script.DB.DBBase
{
    public class BaseDBManager
    {
        public const string DatabaseName = "NativeDB";
        private SQLiteConnection _connection;
        private Dictionary<Type, bool> _exists;

        public BaseDBManager()
        {
	        _exists = new Dictionary<Type, bool>();
#if UNITY_EDITOR
	        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
		    // check if file exists in Application.persistentDataPath
		    var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

		    if (!File.Exists(filepath))
		    {
		        Debug.Log("Database not in Persistent path");
		        // if it doesn't ->
		        // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID 
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
			var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
			// then save to Application.persistentDataPath
			File.Copy(loadDb, filepath);
		
#elif UNITY_STANDALONE_OSX
			var loadDb = Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
			// then save to Application.persistentDataPath
			File.Copy(loadDb, filepath);
#else
			var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
			// then save to Application.persistentDataPath
			Copy(loadDb, filepath);

#endif
        }
            var dbPath = filepath;
#endif
	        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        }
        public int Insert(object data , bool orReplace)
        {
	        Type t = data.GetType();
	        if (!Exists(t))
	        {
		        CreateTable(t);
	        }
	        if (orReplace)
		        return _connection.InsertOrReplace(data, t);
	        return _connection.Insert(data, t);
        }
        
        public int InsertAll(object[] datas , bool orReplace)
        {
	        if(datas.Length <= 0)
		        return 0;
	        Type t = datas[0].GetType();
	        if (!Exists(t))
	        {
		        CreateTable(t);
	        }
	        if (orReplace)
		        return _connection.InsertAll(datas, "OR REPLACE");
	        return _connection.InsertAll(datas);
        }

        public int Update(object data)
        {
	        return _connection.Update(data);
        }
        
        public int UpdataAll(object[] datas)
        {
	        return _connection.UpdateAll(datas);
        }
        
        public int DeleteByKey<T>(object key)
        {
	        return _connection.Delete<T>(key);
        }
        
        public int Delete(object data)
        {
	        return _connection.Delete(data);
        }
        
        public int DeleteAll<T>()
        {
	        return _connection.DeleteAll<T>();
        }

        public bool Exists<T>()
        {
	        return Exists(typeof(T));
        }
        
        public bool Exists(Type t)
        {
	        if (_exists.ContainsKey(t))
		        return true;
	        int count = _connection.ExistsTable(t);
	        if(count <= 0)
		        return false;
	        _exists[t] = true;
	        return true;
        }
        
        public int CreateTable<T>()
        {
	        return _connection.CreateTable<T>();
        }
        
        public int CreateTable(Type t)
        {
	        return _connection.CreateTable(t);
        }
        
        public int DropTable<T>()
        {
	        return _connection.DropTable<T>();
        }
        
        public int DropTable(Type t)
        {
	        return _connection.DropTable(t);
        }
        
        public TableQuery<T> Table<T>() where T : new()
        {
	        return _connection.Table<T>();
        }
        
        public T Get<T> (object pk) where T : new()
        {
	        return _connection.Get<T>(pk);
        }
        
        public T Find<T> (object pk) where T : new()
        {
	        return _connection.Find<T>(pk);
        }
    }
    [AttributeUsage(AttributeTargets.Class)]
    public class DBTableAttribute : Attribute
    {
	    private bool _defaultAdd;
	    public DBTableAttribute(bool defaultAdd)
	    {
		    _defaultAdd = defaultAdd;
	    }
 
	    public bool DefaultAdd
	    {
		    get { return _defaultAdd; }
	    }
    }
}