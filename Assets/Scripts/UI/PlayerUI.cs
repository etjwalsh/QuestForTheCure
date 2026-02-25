using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public CharacterData[] characters;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI playerRole;
    [SerializeField] private TextMeshProUGUI currentStage;
    [SerializeField] private Image bgBox;
    private bool passedGameStart = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateMachine.instance.currentState == GameStateMachine.GameState.Spinning)
        {
            passedGameStart = true;
        }

        if (passedGameStart)
        {
            //set the texts to the correct things
            playerName.text = PlayerManager.instance.players[PlayerManager.instance.currentPlayerIndex].playerName;
            playerRole.text = PlayerManager.instance.current.currentRole;
            currentStage.text = GameStateMachine.instance.currentStage;
            bgBox.color = GetColor(playerName.text);
        }
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
        newColor.a = 0.95f;
        return newColor;
    }
}
