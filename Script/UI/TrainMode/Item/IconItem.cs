using UnityEngine;
using UnityEngine.UI;

namespace UI.TrainMode
{
    public class IconItem : MonoBehaviour
    {
        public Image Image;

        public void SetImage(Sprite sprite)
        {
            Image.sprite = sprite;
        }

        public void SetImage(Sprite sprite ,float size)
        {
            SetImage(sprite);
            Image.rectTransform.sizeDelta = new Vector2(size ,size);
        }

    }
}