using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnButtonClick(GameObject button)
    {
        Debug.Log("button clicked = " + button.name);
    }

    public void OnPointerEnter(PointerEventData _)
    {
        Debug.Log("mouse is over = " + gameObject.name);
    }
    public void OnPointerExit(PointerEventData _)
    {
        Debug.Log("mouse just left = " + gameObject.name);
    }
}
