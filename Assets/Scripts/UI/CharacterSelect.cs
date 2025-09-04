using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CharacterSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CharacterData[] characters;
    [SerializeField] private GameObject highlight;
    [SerializeField] private TMP_Text bigName;
    [SerializeField] private TMP_Text description;
    private string characterName;

    private void Start()
    {
        highlight.SetActive(false);
    }

    public void OnButtonClick(GameObject button)
    {
        Debug.Log("button clicked = " + button.name);
        //set global character for that player to be the one that was clicked
    }

    public void OnPointerEnter(PointerEventData _)
    {
        characterName = GetName();

        if (gameObject.name != "CharacterSelect")
        {
            //change name header to character's name
            bigName.text = characterName;

            //change description

            //change border highlight color to character's color

            //set border location to the same as the button selected
            highlight.transform.position = gameObject.transform.position;

            //set border to active
            highlight.SetActive(true);

        }
    }

    public void OnPointerExit(PointerEventData _)
    {
        //set border to inactive
        highlight.SetActive(false);
    }

    private string GetName()
    {
        string charName = gameObject.name.Replace("Button", "");
        return charName;
    }

    // private string GetDescription(string charName)
    // {
        
    //     //return description;
    // }
}
