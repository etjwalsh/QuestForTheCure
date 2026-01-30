using UnityEngine;
using System.Collections.Generic;

//still have to make the material for the sick person change!

public class SymptomChecker : MonoBehaviour
{
    public GameObject sicklyUI;
    public List<Material> sickMatDudes = new List<Material>();
    public List<Material> matDudes = new List<Material>();
    private bool sick;
    private GameObject minigameManager;
    private SymptomSearchController ssc;

    void Awake()
    {
        //disable the UI
        sicklyUI.SetActive(false);

        //get a reference to the minigame manager
        minigameManager = GameObject.FindGameObjectWithTag("MinigameManager");
        ssc = minigameManager.GetComponent<SymptomSearchController>();
    }

    void OnMouseEnter()
    {
        //make their UI pop up
        sicklyUI.SetActive(true);
    }

    void OnMouseExit()
    {
        //make their UI disappear
        sicklyUI.SetActive(false);
    }

    void OnMouseDown()
    {
        Debug.Log($"Clicked on {gameObject.name} at position {transform.position}");
        OnClicked();
    }

    void OnClicked()
    {
        Debug.Log("Clicked: " + gameObject.name);

        //tell the minigame manager that a sick person has been clicked
        if (sick)
        {
            //increase the score
            ssc.IncreaseScore();

            //get rid of the person
            gameObject.SetActive(false);
        }
    }

    public void CheckSideEffects(float percentChance)
    {
        Debug.Log("Checking side effects at a rate of: " + percentChance);
        //check if this person is going to be sick based on the percent chance passed into the function
        if (Random.value < percentChance / 100)
        {
            //make the person sick
            sick = true;

            //change the sprite of the character to be sick
            gameObject.GetComponentInChildren<Renderer>().material = sickMatDudes[Random.Range(0, sickMatDudes.Count)];

            //keep track of how many sick people there are
            ssc.totalSickPeople++;
            ssc.everySickPeople++;
        }
        else //they aren't sick
        {
            //set them to be one of the non sick dudes
            gameObject.GetComponentInChildren<Renderer>().material = matDudes[Random.Range(0, matDudes.Count)];
        }
    }
}