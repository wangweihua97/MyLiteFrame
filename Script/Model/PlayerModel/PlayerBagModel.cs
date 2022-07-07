using System;
using System.Collections.Generic;
using Script.Main;
using UnityEngine;

namespace Script.Model
{
    [Serializable]
    public class PlayerBagModel
    {
        public List<DressUpBagData> DressUpBagData;
        public Dictionary<string,DressUpBagData> DressUpBagDataDic;
        public int gold;
        public Dictionary<string,ItemBagData> ItemBagDataDic;
    }

    [Serializable]
    public struct DressUpBagData
    {
        public string TDCharacterID;
        public int count;
        
    }
    [Serializable]
    public struct ItemBagData
    {
        public string TDItemID;
        public int count;
        
    }
    //
    // [Serializable]
    // public struct DressColor321
    // {
    //     public Color Color;
    //     public float Metallic;
    // }
}