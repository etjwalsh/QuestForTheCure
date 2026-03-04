using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PapersManager : MonoBehaviour
{
    public List<int> papers = new List<int>();
    [Header("Paper Stats")]
    public List<Sprite> rats = new List<Sprite>();
    public List<Sprite> ratsHappy = new List<Sprite>();
    public List<Sprite> ratsSad = new List<Sprite>();
    public List<Sprite> treatmentPhotos = new List<Sprite>();
    public List<Color> ratColors = new List<Color>();
    public List<Sprite> approvalSprites = new List<Sprite>();
    public List<string> doseLevel = new List<string> { "Too Low...", "Just Right!", "Too High..!" };
    private bool shouldApprove = true;
    private int currentPageIndex = 1;

    [Header("MinigameStats")]
    public int numPages = 5;
    private int score = 0;

    [Header("UI Stats")]
    public Image treatmentImage;
    public Image ratImage;
    public Image ratBG;
    public Image approvalImage;
    public GameObject stamp;
    public TextMeshProUGUI percentText;
    public TextMeshProUGUI doseText;
    public TextMeshProUGUI scoreUI;

    void Awake()
    {
        GeneratePage();
    }

    public void OnApproveClicked()
    {
        //check answer
        CheckAnswer(true);
        approvalImage.gameObject.SetActive(true);
        approvalImage.sprite = approvalSprites[1];
    }
    public void OnDenyClicked()
    {
        CheckAnswer(false);
        approvalImage.gameObject.SetActive(true);
        approvalImage.sprite = approvalSprites[0];
    }

    private void GeneratePage()
    {
        //make sure the approval thing is gone
        approvalImage.gameObject.SetActive(false);
        //check every step of the way to see if shouldApprove should be set to false
        //get random number 1-4 for treatment photo
        treatmentImage.sprite = treatmentPhotos[currentPageIndex - 1];

        //get random number 1-3 for how the rat felt
        int whichRat = Random.Range(1, 4);

        //get random number 1-7 for rat photo based on last random number
        if (whichRat == 1) //normal
        {
            ratImage.sprite = rats[Random.Range(0, 7)];
            shouldApprove = false;
        }
        else if (whichRat == 2) //happy
        {
            ratImage.sprite = ratsHappy[Random.Range(0, 7)];
        }
        else if (whichRat == 3) //sad
        {
            ratImage.sprite = ratsSad[Random.Range(0, 7)];
            shouldApprove = false;
        }

        //get random number 1-3 for doseage level
        int randDose = Random.Range(1, 4);
        doseText.text = doseLevel[randDose - 1];
        if (randDose == 1 || randDose == 3)
        {
            shouldApprove = false;
        }

        //get random number 1-3 for what range for the %
        int percentRange = Random.Range(1, 7);

        //get random number 47-70, 70-89, or 90-99 depending on last random number
        //make the % number change color (red green etc)
        if (percentRange == 1)
        {
            percentText.text = Random.Range(42, 70).ToString() + "%";
            percentText.color = Hex("#D2082E");
        }
        else if (percentRange == 2)
        {
            percentText.text = Random.Range(70, 90).ToString() + "%";
            percentText.color = Hex("#FFC526");
        }
        else if (percentRange >= 3)
        {
            percentText.text = Random.Range(90, 100).ToString() + "%";
            percentText.color = Hex("#45A682");
        }
    }
    private void CheckAnswer(bool tf)
    {
        //check if the player's answer is the same as if the current paper should be 
        if (tf == shouldApprove)
        {
            score++;
        }

        //check to make sure that you aren't on the last page
        if (currentPageIndex != numPages)
        {
            //move to the next page
            currentPageIndex++;
            //update the page number
            scoreUI.text = "Treatment: " + currentPageIndex.ToString();
            GeneratePage();
        }
        else
        {
            StartCoroutine(EndGame());
        }
    }

    Color Hex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color c);
        return c;
    }

    private IEnumerator EndGame()
    {
        //stop the game

        //fade the end screen UI in

        //show the results one at a time
        yield return null;
    }
}