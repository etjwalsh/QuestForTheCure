using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class SymptomChecker : MonoBehaviour
{
    public GameObject sicklyUI;
    public Sprite sickImage;
    public Sprite healthyImage;

    private bool clicked = false;
    public bool isSick;

    void Awake()
    {
        //disable the UI
        sicklyUI.SetActive(false);
    }
    void Start()
    {

    }

    void OnMouseEnter()
    {
        sicklyUI.SetActive(true);
        if (!clicked)
        {

        }
    }

    void OnMouseExit()
    {
        sicklyUI.SetActive(false);
        if (!clicked)
        {

        }
    }

    void OnMouseDown()
    {

        Debug.Log($"Clicked on {gameObject.name} at position {transform.position}");

        // Add your click logic here
        OnClicked();
    }

    void OnClicked()
    {
        Debug.Log("Clicked: " + gameObject.name);
        clicked = true;
        // Do whatever you want when clicked
        // For example: destroy the object, change its properties, etc.
    }
}