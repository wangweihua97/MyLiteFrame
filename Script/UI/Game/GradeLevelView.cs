using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GradeLevelView : MonoBehaviour
    {
        public Sprite S;
        public Sprite A;
        public Sprite B;
        public Sprite C;
        public Sprite D;

        public Image _image;

        public void SetGrade(string g)
        {
            switch (g)
            {
                case "S":
                    _image.sprite = S;
                    break;
                case "A":
                    _image.sprite = A;
                    break;
                case "B":
                    _image.sprite = B;
                    break;
                case "C":
                    _image.sprite = C;
                    break;
                case "D":
                    _image.sprite = D;
                    break;
            }
        }
    }
}