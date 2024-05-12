using UnityEngine;
using UnityEngine.UI;

public class SkinSelectorMainMenu : MonoBehaviour 
{
    public Image _image;

    public void Set(Color color)
    {
        _image.color = color;
    }
}
