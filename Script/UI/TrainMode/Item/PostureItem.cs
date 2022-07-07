using UnityEngine;
using UnityEngine.UI;

namespace UI.TrainMode
{
    public class PostureItem : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Text _text;

        public void SetBodyPosture(bool isLeft)
        {
            Vector3 scale = isLeft ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
            _image.rectTransform.localScale = scale;
            if (isLeft)
                _text.text = "左架姿势";
            else
            {
                _text.text = "右架姿势";
            }
        }
    }
}