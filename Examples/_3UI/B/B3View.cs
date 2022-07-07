using UI.Base;
namespace Examples.UI
{
    public class B3View : UView
    {
        public override bool DefaultShow => false;
        public override void DoCreat()
        {
            base.DoCreat();
            BUIMgr.B3View = this;
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