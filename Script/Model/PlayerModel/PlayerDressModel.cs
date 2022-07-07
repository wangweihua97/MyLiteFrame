using System;
using System.Collections.Generic;
using Script.Main;
using UnityEngine;

namespace Script.Model
{
    [Serializable]
    public class PlayerDressModel
    {
        public DressColor SkinColor;
        public DressColor HairColor;
        public DressUpData Eyes;
        public DressUpData Body;
        public DressUpData Head;
        public DressUpData Hair;
        public DressUpData Hand;
        public DressUpData Leg;
        public DressUpData Shoes;
    }

    [Serializable]
    public struct DressUpData
    {
        public string Model;
        public List<string> Texture;
        public string Id;
        
    }

    [Serializable]
    public struct DressColor
    {
        public Color Color;
        public float Metallic;
        public string Id;
    }
}