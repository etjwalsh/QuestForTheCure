using UnityEngine;
using UnityEngine.UI;

public class ButtonPress : MonoBehaviour
{
    public Image buttonImage;
    public Sprite normalSprite;
    public Sprite pressedSprite;

    public void OnPress()
    {
        buttonImage.sprite = pressedSprite;
    }

    public void OnRelease()
    {
        buttonImage.sprite = normalSprite;
    }
}