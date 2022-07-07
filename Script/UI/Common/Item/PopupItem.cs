using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Common.Item
{
    public class PopupItem : MonoBehaviour
    {
        [SerializeField] private RectTransform _image;
        [SerializeField] private Text _text;
        public void SetText(string text)
        {
            gameObject.SetActive(true);
            _text.text = text;
            LayoutRebuilder.ForceRebuildLayoutImmediate(_text.rectTransform);
        }
    }
}