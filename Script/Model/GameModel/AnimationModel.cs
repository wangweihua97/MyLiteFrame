﻿using UnityEngine;
using UnityEngine.Animations;

namespace Script.Model
{
    public class AnimationModel
    {
        public float attackTime = 0.6f;
        public float bTime = 1.2f;
        public float backTime = 1.2f;
        public float totalTime  = 2;
        public string aniName;
        public AnimationClipPlayable animationClip;

        public AnimationModel(string aniName, AnimationClipPlayable animationClip)
        {
            this.aniName = aniName;
            this.animationClip = animationClip;
        }

        public void SetData(float FTime, float BTime, float ATime, float Time)
        {
            attackTime = FTime == 0 ? 0.3f : FTime;
            bTime = BTime ==0 ? 0.5f : BTime;
            totalTime = Time == 0 ? animationClip.GetAnimationClip().length : Time;
            backTime = ATime == 0 ? totalTime - 0.3f : ATime;
            
        }
    }
}