using UnityEngine;
using UnityEngine.UI;

namespace Elements
{
    public class ElementView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        
        public Image Image => _image;

        public void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
            //_image.SetNativeSize();
            _image.transform.localScale = Vector3.one;

        }

        public void SetWinSprite(Sprite sprite)
        {
            _image.sprite = sprite;
            //_image.SetNativeSize();
            _image.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }
}