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
    // [SerializeField] private GameObject characterImage;
    [SerializeField] private GameObject highlight;
    [SerializeField] private TMP_Text bigName;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text playerTurn;
    private string characterName;

    public int numPlayersToSelect;

    private void Start()
    {
        highlight.SetActive(false);
        // characterImage.SetActive(false);
        characters = CharacterSelectUI.GetComponent<CharacterSelect>().characters;
    }

    public void OnButtonClick(GameObject button)
    {
        Button newButton = button.GetComponent<Button>();
        characterName = button.name.Replace("Button", "");
        //create new player 
        Player newPlayer = ScriptableObject.CreateInstance<Player>();

        //initialize player
        for (int i = 0; i < characters.Length; i++)
        {
            if (characterName == characters[i].charName)
            {
                //assign player name
                newPlayer.playerName = characterName;
                //assign player model
                newPlayer.playerModel = characters[i].model;
                //assign the player game piece
                newPlayer.characterPiece = characters[i].charPiece;
            }
        }
        PlayerManager.instance.players.Add(newPlayer);

        //deincrement numPlayersToSelect
        numPlayersToSelect--;

        //if no more players to select
        if (numPlayersToSelect == 0)
        {
            // //print the list
            // foreach (Player p in PlayerManager.instance.players)
            // {
            //     Debug.Log("Player Name: " + p.playerName);
            //     Debug.Log("Player Model = " + p.playerModel);
            // }

            //change game state to game start state
            GameStateMachine.instance.currentState = GameStateMachine.GameState.GameStart;
        }

        //set that button to not be interactable anymore
        newButton.interactable = false;

        //change the text to say the next player's turn
        playerTurn.text = "Player " + (PlayerManager.instance.players.Count + 1) + "'s turn!";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //get reference to the button being hovered over
        GameObject enteredObject = eventData.pointerEnter;
        Button button = enteredObject.GetComponent<Button>();

        //save the name of the character who's button you're hovering over
        characterName = GetName();

        //check to make sure the pointer isn't just on the background, and that the button isn't already deactivated
        if (gameObject.name != "CharacterSelect" && button.interactable == true) 
        {
            //change name header to character's name
            bigName.text = characterName;
            bigName.color = GetColor(characterName); //and set the name to the character's color

            //change description
            description.text = GetDescription(characterName);

            //change border highlight color to character's color
            highlight.GetComponent<Image>().color = GetColor(characterName);

            //change character image to the character's portrait
            // characterImage.GetComponent<Image>().sprite = GetCharacterImage(characterName);
            // characterImage.SetActive(true);

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

    // private Sprite GetCharacterImage(string characterName)
    // {
    //     Sprite newSprite = null;

    //     for (int i = 0; i < characters.Length; i++)
    //     {
    //         if (characterName == characters[i].charName)
    //         {
    //             newSprite = characters[i].model;
    //         }
    //     }
    //     return newSprite;
    // }
}
