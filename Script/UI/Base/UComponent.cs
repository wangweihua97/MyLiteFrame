using UnityEngine;

namespace UI.Base
{
    public class UComponent : MonoBehaviour
    {
        /// <summary>
        /// 页面创建时执行
        /// </summary>
        public virtual void DoCreat()
        {
        }
        
        /// <summary>
        /// 页面打开时
        /// </summary>
        public virtual void DoOpen()
        {
        }
        
        /// <summary>
        /// 页面关闭时
        /// </summary>
        public virtual void DoClose()
        {
        }

        /// <summary>
        /// 页面摧毁时执行
        /// </summary>
        public virtual void DoDestory()
        {
        }
    }
}