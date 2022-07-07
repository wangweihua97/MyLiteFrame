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
    public class StoryAction : Action
    {
        public SharedString KeyName;

        public override TaskStatus OnUpdate()
        {
            if (GameVariable.GameState == GameState.Story)
                return TaskStatus.Failure;
            CheckKey();
            return TaskStatus.Success;
        }

        void CheckKey()
        {
            switch ((KeyCode)Enum.Parse(typeof(KeyCode),KeyName.Value))
            {
                case KeyCode.W:
                    break;
                case KeyCode.S:
                    break;
                case KeyCode.A:
                    break;
                case KeyCode.D:
                    break;
                case KeyCode.J:
                    break;
                case KeyCode.K:
                    break;
            }
        }
    }
}