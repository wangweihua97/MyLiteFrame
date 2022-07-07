using System;
using UnityEngine;

namespace Coach
{
    public class CoachMgr : MonoBehaviour
    {
        public static CoachMgr instance;
        public Transform BrithTransform;
        public Camera Camera;
        public RenderTexture RenderTexture;

        private void Awake()
        {
            instance = this;
            RenderTexture = new RenderTexture(280 ,700 ,1);
            Camera = GetComponent<Camera>();
            Camera.targetTexture = RenderTexture;
        }
    }
}