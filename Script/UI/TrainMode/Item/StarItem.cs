using UnityEngine;
using UnityEngine.UI;

namespace UI.TrainMode
{
    public class StarItem : MonoBehaviour
    {
        public Image UnLockImg;
        public Image LockImg;

        public bool IsLocked
        {
            get { return _isLocked; }
            set
            {
                if (value)
                {
                    LockImg.gameObject.SetActive(true);
                    UnLockImg.gameObject.SetActive(false);
                }
                else
                {
                    UnLockImg.gameObject.SetActive(true);
                    LockImg.gameObject.SetActive(false);
                }

                _isLocked = value;
            }
        }

        private bool _isLocked = true;
    }
}