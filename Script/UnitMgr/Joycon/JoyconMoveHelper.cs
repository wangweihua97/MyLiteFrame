using System.Collections;
using System.Collections.Generic;
using Events;
using Script.Mgr;
using UnityEngine;

namespace UnitMgr
{
    public class JoyconMoveHelper
    {
        /*Queue<Vector3> mQ = new Queue<Vector3>();*/
        Queue<float> mQ = new Queue<float>();
        public int maxCount;
        //public Vector3 total = Vector3.zero;
        public float total = 0;
        public float threshold;
        public bool isCD;
        private bool isLeft;
        private float maxPower;

        public void Init(bool isLeft)
        {
            isCD = false;
            this.isLeft = isLeft;
            //maxCount = JoyconInputMgr.instance.recondCount;
            //threshold = JoyconInputMgr.instance.powar;
            maxPower = 2f * threshold / maxCount;
        }

        public void Clear()
        {
            //total = Vector3.zero;
            total = 0;
            mQ.Clear();
        }

        public void EnQueue(Vector3 vector3)
        {
            /*if(isCD)
                return;
            float xIn = GetAbs(vector3.x);
            float yIn = GetAbs(vector3.y);
            float zIn = GetAbs(vector3.z);
            mQ.Enqueue(new Vector3(xIn ,yIn ,zIn));
            if (mQ.Count <= maxCount)
            {
                total.x += xIn;
                total.y += yIn;
                total.z += zIn;
                Check();
                return;
            }
            Vector3 a = mQ.Dequeue();
            float xOut = a.x;
            float yOut = a.y;
            float zOut = a.z;
            total.x += xIn - xOut;
            total.y += yIn - yOut;
            total.z += zIn - zOut;
            Check();*/
            if(isCD)
                return;
            float len = Mathf.Sqrt(Mathf.Pow(vector3.x, 2) + Mathf.Pow(vector3.y, 2) + Mathf.Pow(vector3.z, 2));
            len = len > maxPower ? maxPower : len;
            mQ.Enqueue(len);
            if (mQ.Count <= maxCount)
            {
                total += len;
                Check();
                return;
            }
            float a = mQ.Dequeue();
            total += len - a;
            Check();
        }

        void Check()
        {
            //float v = Mathf.Abs(total.x) + Mathf.Abs(total.y) + Mathf.Abs(total.z);
            float v = total;
            if(v > threshold)
            {
                if(isLeft)
                    EventCenter.ins.EventTrigger("KeyDown",KeyCode.Q);
                else
                    EventCenter.ins.EventTrigger("KeyDown",KeyCode.E);
                Clear();
                isCD = true;
                //JoyconInputMgr.instance.StartCoroutine(WaitCD());
            }
        }

        IEnumerator WaitCD()
        {
            yield return new WaitForSeconds(0.02f);
            isCD = false;
        }

        float GetAbs(float f)
        {
            float a = f;
            a = a > maxPower ? maxPower : a;
            a = a < -maxPower ? -maxPower : a;
            return a;
        }
        
    }
}