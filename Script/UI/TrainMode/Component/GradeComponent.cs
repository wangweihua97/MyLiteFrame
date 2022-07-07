using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TrainMode
{
    public class GradeComponent : UComponent
    {
        public int MaxDegree = 10;
        public Image FillImg;
        public Slider Slider;

        public void SetDegree(int value)
        {
            if (value <= MaxDegree * 0.33f)
            {
                FillImg.color = new Color(0.309f ,0.92f ,0.898f);
            }
            else if (value <= MaxDegree * 0.66f)
            {
                FillImg.color = new Color(0.965f ,0.613f ,0.352f);
            }
            else
            {
                FillImg.color = new Color(0.996f ,0.293f ,0.512f);
            }

            Slider.value = (float)value / MaxDegree;
        }

        public override void DoOpen()
        {
            base.DoOpen();
        }
    }
}