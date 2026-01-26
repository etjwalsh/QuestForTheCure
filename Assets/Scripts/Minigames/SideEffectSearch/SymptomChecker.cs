using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SymptomChecker : MonoBehaviour
{
    public GameObject sicklyUI;
    public Image faceImage;
    public Sprite sickImage;
    public Sprite healthyImage;
    private bool sick;

    public bool isSick;
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


        //Check if the person is sick
        // if (sick)
        // {
        //     //Add one to the number of sick people
        //     numSickPeople++;

        //     //display that number in the UI
        //     numSickText.text = "Number of people with\nside effects found: " + numSickPeople.ToString();
        // }

        // //Add one to the number of people that are sick

        //Make this person inactive

    }

    public void CheckSideEffects(float percentChance)
    {
        Debug.Log("Checking side effects at a rate of: " + percentChance);
        //check if this person is going to be sick based on the percent chance passed into the function
        if (Random.value < percentChance / 100)
        {
            //make the person sick
            sick = true;
            faceImage.sprite = sickImage;
            ssc.totalSickPeople++;
        }
    }
}