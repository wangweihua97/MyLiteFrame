using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main
{
    public class SelectItemComponent : UComponent
    {
        public GameObject SelectImg;
        public GameObject NoSelectImg;
        public GameObject Describe;
        public Text DescribeText;
        private bool _isSelected = false;

        public void SetData(string text)
        {
            DescribeText.text = text;
        }

        public bool IsSelected()
        {
            return _isSelected;
        }

        public void BeSelected()
        {
            NoSelectImg.SetActive(false);
            SelectImg.SetActive(true);
            Describe.SetActive(true);
            _isSelected = true;
        }
        
        public void Leave()
        {
            NoSelectImg.SetActive(true);
            SelectImg.SetActive(false);
            Describe.SetActive(false);
            _isSelected = false;
        }
    }
}