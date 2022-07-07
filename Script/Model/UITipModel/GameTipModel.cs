﻿using System.Collections.Generic;
 using Events;
 using Ghost;
 using Script.Game;

 namespace Script.Model
{
    public class GameTipModel
    {
        private Queue<float> timeQue ;
        private Queue<TipModel> tipModelQue;
        private int _group;
        private bool IsContinuous;
        

        public GameTipModel()
        {
            timeQue = new Queue<float>();
            tipModelQue = new Queue<TipModel>();
        }

        public void RefreshGroup(int group)
        {
            if (group == 0 || group != _group)
                IsContinuous = false;
            else
                IsContinuous = true;
            _group = group;
        }

        public void Check()
        {
            if(timeQue.Count <= 0)
                return;
            float time = timeQue.Peek();
            if (time < MainGameCtr.instance.gameTime)
            {
                timeQue.Dequeue();
                EventCenter.ins.EventTrigger("PlayTip",tipModelQue.Dequeue());
            }
        }

        public void EnQueue(float preTime,string text ,string textSoundRes)
        {
            float time = MainGameCtr.instance.gameTime + GameVariable.BirthToClickTime + preTime;
            timeQue.Enqueue(time);
            tipModelQue.Enqueue(new TipModel(text ,textSoundRes ,IsContinuous));
        }

        public void Clear()
        {
            timeQue.Clear();
            tipModelQue.Clear();
        }
    }
}