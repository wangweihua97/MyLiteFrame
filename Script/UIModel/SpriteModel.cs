using UnityEngine;

namespace UIModel
{
    public class SpriteModel
    {
        public Sprite Sprite;
        public bool IsOverturn;

        public SpriteModel(Sprite sprite , bool isOverturn)
        {
            Sprite = sprite;
            IsOverturn = isOverturn;
        }
    }
}