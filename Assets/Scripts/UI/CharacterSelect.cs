using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CharacterData[] characters;
    [SerializeField] private GameObject CharacterSelectUI;
    [SerializeField] private GameObject characterImage;
    [SerializeField] private GameObject highlight;
    [SerializeField] private TMP_Text bigName;
    [SerializeField] private TMP_Text description;
    private string characterName;

    private void Start()
    {
        highlight.SetActive(false);
        characterImage.SetActive(false);
        characters = CharacterSelectUI.GetComponent<CharacterSelect>().characters;
    }

    public void OnButtonClick(GameObject button)
    {
        Debug.Log("button clicked = " + button.name);

        //set global character for that player to be the one that was clicked

        //change game state to game start state
        GameStateMachine.instance.currentState = GameStateMachine.GameState.GameStart;
        //change scene to game board scene
        SceneManager.LoadScene("Sandbox"); //CHANGE ME
    }

    public void OnPointerEnter(PointerEventData _)
    {
        characterName = GetName();

        if (gameObject.name != "CharacterSelect")
        {
            //change name header to character's name
            bigName.text = characterName;
            bigName.color = GetColor(characterName); //and set the name to the character's color

            //change description
            description.text = GetDescription(characterName);

            //change border highlight color to character's color
            highlight.GetComponent<Image>().color = GetColor(characterName);

            //change character image to the character's portrait
            characterImage.GetComponent<Image>().sprite = GetCharacterImage(characterName);
            characterImage.SetActive(true);

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
        highlight.GetComponent<Image>().color = Color.white;
    }

    private string GetName()
    {
        string charName = gameObject.name.Replace("Button", "");
        return charName;
    }

    private string GetDescription(string characterName)
    {
        string description = "";

        for (int i = 0; i < characters.Length; i++)
        {
            if (characterName == characters[i].charName)
            {
                description = characters[i].description;
                break;
            }
        }
        return description;
    }

    private Color GetColor(string characterName)
    {
        Color newColor = Color.black;

        for (int i = 0; i < characters.Length; i++)
        {
            if (characterName == characters[i].charName)
            {
                newColor = characters[i].charColor;
            }
        }

        return newColor;
    }

    private Sprite GetCharacterImage(string characterName)
    {
        Sprite newSprite = null;

        for (int i = 0; i < characters.Length; i++)
        {
            if (characterName == characters[i].charName)
            {
                newSprite = characters[i].model;
            }
        }

        return newSprite;
    }
}
