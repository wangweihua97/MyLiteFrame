﻿using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
 using Script.Main;
 using Script.Mgr;
 using Script.Model;
 using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Script.Game
{
    public class GameAciton: Action
    {
        public SharedString KeyName;

        public override TaskStatus OnUpdate()
        {
            if (GameVariable.GameState != GameState.Gaming)
                return TaskStatus.Failure;
            CheckKey();
            return TaskStatus.Success;
        }

        void CheckKey()
        {
            switch ((KeyCode)Enum.Parse(typeof(KeyCode),KeyName.Value))
            {
                case KeyCode.Q:
                    MainGameCtr.instance.ClickCheck(BeltType.Left, ActionType.StraightPunch ,false);
                    break;
                case KeyCode.E:
                    MainGameCtr.instance.ClickCheck(BeltType.Right, ActionType.StraightPunch ,false);
                    break;
                case KeyCode.K:
                    RootUIMgr.instance.GetUIMgr<GameUIMgr>().StopGameUI();
                    MainGameCtr.instance.StopGame();
                    break;
            }
        }
    }
}