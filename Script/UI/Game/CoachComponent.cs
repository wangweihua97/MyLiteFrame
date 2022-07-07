using Coach;
using UI.Base;
using UnityEngine.UI;

namespace UI
{
    public class CoachComponent : UComponent
    {
        public RawImage RawImage;
        void Start()
        {
            RawImage.texture = CoachMgr.instance.Camera.targetTexture;
        }
    }
}