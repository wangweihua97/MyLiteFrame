using UnityEngine;
using UnityEngine.UI;

namespace UI.Character.Item
{
    public class TypeBar : MonoBehaviour
    {
        public Image SelectIcon;
        public Image SelectBg;
        public Image SelectBgArrow;
        public Text Text;
        public int type;
        /**蓝,白,黄*/
        private Color[] cl = {new Color(0.113f , 0.277f ,0.39f ,1f), new Color(1f , 1f ,1f ,1f)
            , new Color(253/255f , 237/255f ,89/255f ,1f)};
        private bool _weaken = false;
    
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value)
                {
                    SelectBg.gameObject.SetActive(true);
                    Text.color = cl[0];// new Color(0.113f , 0.277f ,0.39f ,1f);
                    SelectIcon.color = cl[0];// new Color(23/255f , 63/255f,89/255f ,1f);
                    SelectBg.color = _weaken ? cl[1] : cl[2];
                    SelectBgArrow.color = _weaken ? cl[1] : cl[2];
                }
                else
                {
                    SelectBg.gameObject.SetActive(false);
                    Text.color = cl[1];//new Color(1f , 1f ,1f ,1);
                    SelectIcon.color = cl[1];//new Color(1f , 1f ,1f ,1f);
                }

                _isSelected = value;
            }
        }

        public void SetWeaken(bool b)
        {
            _weaken = b;
            SelectBg.color = _weaken ? cl[1] : cl[2];
            SelectBgArrow.color = _weaken ? cl[1] : cl[2];
        }

        public void updateColor()
        {
            
        }

        private bool _isSelected = true;
    }
}