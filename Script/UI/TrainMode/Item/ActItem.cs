using OtherMgr;
using Script.Excel.Table;
using UIModel;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TrainMode
{
    public class ActItem : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private GameObject _selectedGo;
        public TDAction Data;

        public void SetData(TDAction data)
        {
            this.Data = data;
            Refresh();
        }

        void Refresh()
        {
            if(Data == null)
                gameObject.SetActive(false);
            else
            {
                gameObject.SetActive(true);
                SpriteModel spriteModel = SpritesMgr.Get(Data.icon);
                if(spriteModel.Sprite == null)
                    Debug.LogError(Data.icon);
                _image.sprite = spriteModel.Sprite;
                Vector3 scale = spriteModel.IsOverturn ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
                _image.rectTransform.localScale = scale;
                
            }
        }

        public void BeSelected()
        {
            _selectedGo.SetActive(true);
        }
        
        public void BeLeft()
        {
            _selectedGo.SetActive(false);
        }
    }
}