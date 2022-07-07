using UnityEngine;
using UnityEngine.UI;

namespace UI.TrainMode
{
    public class DegreeItem : MonoBehaviour
    {
        public Image SelectImg;
        public Text Text;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value)
                {
                    SelectImg.gameObject.SetActive(true);
                    Text.color = new Color(0.113f , 0.277f ,0.39f ,1);
                }
                else
                {
                    SelectImg.gameObject.SetActive(false);
                    Text.color = new Color(1f , 1f ,1f ,1);
                }

                _isSelected = value;
            }
        }

        private bool _isSelected = true;
    }
}