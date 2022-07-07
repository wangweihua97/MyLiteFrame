﻿using Script.Excel.Table;
 using Script.Model;

namespace Ghost
{
    public class ActionModel
    {
        public float speed = 100;
        public float yOff = 0;
        public bool bodyIsLeft = false;
        public AcitonLocat acitonLocat = AcitonLocat.Left;
        public string animationName = "";
        public float FTime = 0.2f;
        public float BTime = 0.33f;
        public float ATime = 0.5f;
        public float Time = 0.74f;
        public float S = 0.9f;
        public int SScore = 100;
        public float A = 0.8f;
        public int AScore = 80;
        public float B = 0.7f;
        public int BScore = 60;
        public int step = 0;
        public ActionClass ActionClass = ActionClass.Common;
        public string Icon;

        public float C = 0.5f;
        public int CScore = 40;
        public int bounsType; 
        public int bounsProp1;
        public int bounsProp2;
        public int bounsProp3;
        public bool voiceTips;

        public float offeTime;

        //初始化
        public ActionModel(float speed ,AcitonLocat acitonLocat)
        {
            this.acitonLocat = acitonLocat;
            this.speed = speed;
        }

        //设置数据
        public void SetData(TDActionFlow data)
        {
            bodyIsLeft = data.body.Equals("左");
            bounsType = data.bounsType;
            bounsProp1 = data.bounsProp1;
            bounsProp2 = data.bounsProp2;
            bounsProp3 = data.bounsProp3;
            voiceTips = data.voiceTips == 1;
            step = data.step;
        }
        public void SetData(TDAction data)
        {
            S = data.gradeS;
            SScore = data.pointS;
            A = data.gradeA;
            AScore = data.pointA;
            B = data.gradeB;
            BScore = data.pointB;
            C = data.gradeC;
            CScore = data.pointC;

            switch (data.actionType)
            {
                case 1:
                    ActionClass = ActionClass.Attack;
                    break;
                case 2:
                    ActionClass = ActionClass.Defense;
                    break;
                case 3:
                    ActionClass = ActionClass.Dodge;
                    break;
                default:
                    ActionClass = ActionClass.Common;
                    break;
            }
            Icon = data.icon;
            animationName = data.actAnimation;
            FTime = data.fTime;
            BTime = data.bTime;
            ATime = data.aTime;
            Time = data.time;
        }
    }
}