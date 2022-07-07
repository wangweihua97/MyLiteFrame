using System;
using System.ComponentModel.Design.Serialization;
using System.Net.NetworkInformation;
using System.Text;
using Script.DB;
using Script.DB.DBModel;
using Script.Tool;
using UnityEngine;

namespace Player
{
    public class PlayerInfo
    {
        public static PlayerInfo Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new PlayerInfo();
                return _instance;
            }
        }

        private static PlayerInfo _instance;

        public long UId
        {
            get
            {
                if (_uId == 0)
                {
                    if (!TryInitUId())
                    {
                        Debug.Log("角色没有创建过");
                        _uId = GetUId();
                    }
                }
                return _uId;
            }
        }
        
        public string Name;

        public PlayerTable PlayerData
        {
            get
            {
                if (_playerData == null)
                {
                    if (!HavePlayerInfo())
                        return new PlayerTable();
                    _playerData = DBManager.Instance.Get<PlayerTable>(UId);
                }

                return _playerData;
            }
        }

        private PlayerTable _playerData;
        private long _uId = 0;
        

        public PlayerInfo()
        {
            TryInitUId();
        }

        private bool TryInitUId()
        {
            if(!HavePlayerInfo())
                return false;
            _uId = Int64.Parse(NativeStoreTool.GetString("PlayerUId"));
            return true;
        }

        public bool HavePlayerInfo()
        {
            return NativeStoreTool.HasKey("PlayerUId");
        }

        public void CreatPlayerInfo()
        {
            NativeStoreTool.Set("PlayerUId" ,_uId.ToString());
            DBManager.Instance.Insert(new PlayerTable(),true);
            
        }

        long GetUId()
        {
            string str = GetMacAddress() + SystemInfo.deviceUniqueIdentifier;
            StringBuilder stringBuilder = new StringBuilder();
            int a = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (a >= 3)
                {
                    stringBuilder.Append(str[i]);
                    a = 0;
                }
                a++;
                if(stringBuilder.Length >= 15)
                    break;
            }
            return Convert.ToInt64(stringBuilder.ToString(),16);
        }
        
        string GetMacAddress()
        {
            string physicalAddress = "";
            NetworkInterface[] nice = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adaper in nice)
            {
                Debug.Log(adaper.Description);
                if (adaper.Description == "en0")
                {
                    physicalAddress = adaper.GetPhysicalAddress().ToString();
                    break;
                }
                else
                {
                    physicalAddress = adaper.GetPhysicalAddress().ToString();
                    if (physicalAddress != "")
                    {
                        break;
                    };
                }
            }
            return physicalAddress;
        }
    }
}