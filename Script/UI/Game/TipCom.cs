using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TipCom : MonoBehaviour
    {
        public Text text;
        public int showCount = 0;

        public void SetText(string text, bool IsContinuous)
        {
            if (!IsContinuous)
            {
                this.text.text = text;
            }
            else
            {
                this.text.text += ", " + text;
            }
            
        }
    }
}