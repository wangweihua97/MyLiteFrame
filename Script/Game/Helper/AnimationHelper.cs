using OtherMgr;
using Player;
using Script.Model;
using UnityEngine;

namespace Script.Game
{
    public class AnimationHelper
    {
        private const int maxTaskCount = 10;
        float[] timeArr;
        bool[] isPlayArr;
        string[] playNameArr;
        public bool isLife;
        public void Init()
        {
            timeArr = new float[maxTaskCount];
            isPlayArr = new bool[maxTaskCount];
            playNameArr = new string[maxTaskCount];
            isLife = true;
        }

        public void AddTask(string animationName,float time)
        {
            for (int i = 0; i < maxTaskCount; i++)
            {
                if (!isPlayArr[i])
                {
                    isPlayArr[i] = true;
                    timeArr[i] = time;
                    playNameArr[i] = animationName;
                    return;
                }
            }
            Debug.LogError("播放数列已经满了，没有添加播放任务");
        }

        public void Clear()
        {
            isLife = false;
        }

        public void OnUpdate()
        {
            if(!isLife)
                return;
            for (int i = 0; i < maxTaskCount; i++)
            {
                if(!isPlayArr[i])
                    continue;
                timeArr[i] -= Time.deltaTime;
                if (timeArr[i] <= 0)
                {
                    isPlayArr[i] = false;
                    Play(i);
                }
            }
        }

        void Play(int index)
        {
            PlayerMgr.instance.playerAnimationMgr.PlayAnimation(playNameArr[index]);
            PlayerMgr.CoachInstance.playerAnimationMgr.PlayAnimation(playNameArr[index]);
        }
    }
}