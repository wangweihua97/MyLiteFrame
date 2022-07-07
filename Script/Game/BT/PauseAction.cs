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
    public class PauseAction : Action
    {
        public SharedString KeyName;

        GameUIMgr GameUiMgr
        {
            get
            {
                return RootUIMgr.instance.GetUIMgr<GameUIMgr>();
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (GameVariable.GameState == GameState.Gaming)
                return TaskStatus.Failure;
            CheckKey();
            return TaskStatus.Success;
        }

        void CheckKey()
        {
            switch ((KeyCode)Enum.Parse(typeof(KeyCode),KeyName.Value))
            {
                case KeyCode.W:
                    //MoveUP();
                    break;
                case KeyCode.S:
                    //MoveDown();
                    break;
                case KeyCode.A:
                    break;
                case KeyCode.D:
                    break;
                case KeyCode.J:
                    //Choose();
                    break;
                case KeyCode.K:
                    BackToGame();
                    break;
            }
        }

        void MoveUP()
        {
            if(GameUiMgr.curIndex <= 0)
                return;
            GameUiMgr.curIndex--;
            GameUiMgr.SelectIconMove();
        }

        void MoveDown()
        {
            if(GameUiMgr.curIndex >= GameUiMgr.maxIndex - 1)
                return;
            GameUiMgr.curIndex++;
            GameUiMgr.SelectIconMove();
        }

        void Choose()
        {
            switch (GameUiMgr.curIndex)
            {
                case 0 :
                    BackToGame();
                    break;
                case 1 :
                    EndGame();
                    break;
            }
        }

        void BackToGame()
        {
            if(GameUIMgr.StopGameView.IsActive())
                RootUIMgr.instance.GetUIMgr<GameUIMgr>().BackGame();
            MainGameCtr.instance.BackGame();
        }

        void EndGame()
        {
            MainGameCtr.instance.AtOnceEndGame();
        }
    }
}