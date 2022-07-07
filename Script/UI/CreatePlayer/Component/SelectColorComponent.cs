using UI.Base;
using UnityEngine;

namespace UI.Character.Component
{
    public class SelectColorComponent : UComponent
    {
        [Header("预制件")] public GameObject Item;
        [Header("生成位置")] public RectTransform Content;


        public override void DoCreat()
        {
            base.DoCreat();
        }

        public override void DoOpen()
        {
            base.DoOpen();
        }
        
        public override void DoClose()
        {
            base.DoClose();
        }

        public override void DoDestory()
        {
            base.DoDestory();
        }
    }
}