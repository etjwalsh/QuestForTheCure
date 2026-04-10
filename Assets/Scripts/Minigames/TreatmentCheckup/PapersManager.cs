using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PapersManager : MonoBehaviour
{
    [Header("Paper Stats")]
    public List<Sprite> rats = new List<Sprite>();
    public List<Sprite> ratsHappy = new List<Sprite>();
    public List<Sprite> ratsSad = new List<Sprite>();
    public List<Sprite> treatmentPhotos = new List<Sprite>();
    public List<Color> ratColors = new List<Color>();
    public List<Sprite> approvalSprites = new List<Sprite>();
    public List<string> doseLevel = new List<string> { "Too Low...", "Just Right!", "Too High..!" };
    private int currentPageIndex = 1;

    [Header("MinigameStats")]
    public int numPages = 5;

    [Header("UI Stats")]
    public Image treatmentImage;
    public Image ratImage;
    public Image ratBG;
    public Image approvalImage;
    public TextMeshProUGUI percentText;
    public TextMeshProUGUI doseText;
    public TextMeshProUGUI scoreUI;
    public GameObject tutorialUI;
    public Button approve;
    public Button deny;
    [Header("End UI")]
    public CanvasGroup endUI;
    private float duration = 2.0f;
    public TextMeshProUGUI approvedText;
    public TextMeshProUGUI deniedText;
    public TextMeshProUGUI endMessage;
    public TextMeshProUGUI approvedNumber;
    public TextMeshProUGUI deniedNumber;
    public GameObject exitButton;

    //vars for statistics tracking
    private int numShouldApprove = 0;
    private int numShouldDeny = 0;
    private int numApproved = 0;
    private int numDenied = 0;

    void Awake()
    {
        //set up the first page
        GeneratePage();

        //make sure tutorial screen is showing
        tutorialUI.SetActive(true);

        //make sure the endUI is not active
        endUI.gameObject.SetActive(false);
    }

    public void OnApproveClicked()
    {
        numApproved++;

        //disable the buttons
        approve.interactable = false;
        deny.interactable = false;

        //check answer
        StartCoroutine(CheckAnswer(true));

        //set the approve / deny
        approvalImage.gameObject.SetActive(true);
        approvalImage.sprite = approvalSprites[1];
    }
    public void OnDenyClicked()
    {
        numDenied++;

        //disable the buttons
        approve.interactable = false;
        deny.interactable = false;

        //check the answer
        StartCoroutine(CheckAnswer(false));

        //set the approve / deny
        approvalImage.gameObject.SetActive(true);
        approvalImage.sprite = approvalSprites[0];
    }
    public void OnStartClicked()
    {
        //genereate the first page
        GeneratePage();

        //disable the tutorial UI
        tutorialUI.SetActive(false);
    }

    private void GeneratePage()
    {
        //reactivate the buttons
        approve.interactable = true;
        deny.interactable = true;

        //make sure the approval thing is gone
        approvalImage.gameObject.SetActive(false);

        //change the rat's background color
        ratBG.color = ratColors[Random.Range(1, ratColors.Count)];

        //check every step of the way to see if shouldApprove should be set to false
        //get random number 1-4 for treatment photo
        treatmentImage.sprite = treatmentPhotos[Random.Range(0, treatmentPhotos.Count - 1)];

        //get random number 1-3 for how the rat felt
        int whichRat = Random.Range(1, 7);
        //get random number 1-3 for doseage level
        int randDose = Random.Range(1, 4);

        //get random number 1-7 for rat photo based on last random number
        if (whichRat == 1) //normal
        {
            Debug.Log("this one should be denied");
            //set the rat sprite
            ratImage.sprite = ratsSad[Random.Range(0, ratsSad.Count - 1)];
            numShouldDeny++;

            //set the percentage 
            percentText.text = Random.Range(42, 70).ToString() + "%";
            percentText.color = Hex("#D2082E");

            //set the dose text
            doseText.text = doseLevel[randDose - 1];

            Debug.Log("numShouldDeny is now: " + numShouldDeny);
        }
        else if (whichRat == 2) //sad
        {
            Debug.Log("this one should be denied");

            //set the rat sprite
            ratImage.sprite = rats[Random.Range(0, rats.Count - 1)];
            numShouldDeny++;

            //set the percentage 
            percentText.text = Random.Range(70, 90).ToString() + "%";
            percentText.color = Hex("#FFC526");

            //set the dose text
            doseText.text = doseLevel[randDose - 1];

            Debug.Log("numShouldDeny is now: " + numShouldDeny);
        }
        else if (whichRat >= 3) //happy
        {
            Debug.Log("this one should be approved");

            //set the rat sprite
            ratImage.sprite = ratsHappy[Random.Range(0, ratsHappy.Count - 1)];
            numShouldApprove++;

            //set the percentage 
            percentText.text = Random.Range(90, 100).ToString() + "%";
            percentText.color = Hex("#45A682");

            //set the dose text
            doseText.text = doseLevel[1];

            Debug.Log("numShouldApprove is now: " + numShouldApprove);
        }

    }
    private IEnumerator CheckAnswer(bool tf)
    {
        //check to make sure that you aren't on the last page
        if (currentPageIndex < numPages)
        {
            //move to the next page
            currentPageIndex++;
            //update the page number
            scoreUI.text = "Treatment: " + currentPageIndex.ToString();

            //generate the next page
            yield return new WaitForSeconds(1.0f);
            GeneratePage();

        }
        else
        {
            StartCoroutine(EndGame());
        }
    }

    Color Hex(string hex)
    {
        UnityEngine.ColorUtility.TryParseHtmlString(hex, out Color c);
        return c;
    }

    private IEnumerator EndGame()
    {
        //deactivate all of the end results to start
        approvedText.gameObject.SetActive(false);
        deniedText.gameObject.SetActive(false);
        approvedNumber.gameObject.SetActive(false);
        deniedNumber.gameObject.SetActive(false);
        endMessage.gameObject.SetActive(false);

        scoreUI.text = "That's all\nthe pages!";

        yield return new WaitForSeconds(1.0f);

        //vars for fading the end UI in
        float elapsedTime = 0f;
        endUI.alpha = 0f; //Start fully transparent
        endUI.blocksRaycasts = true;

        endUI.gameObject.SetActive(true);

        //fade the end screen UI in
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime; //Use unscaledDeltaTime so it works even when Time.timeScale = 0
            endUI.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            yield return null;
        }

        //set all the numbers correctly before showing them
        approvedNumber.text = numApproved + " / " + numShouldApprove;
        deniedNumber.text = numDenied + " / " + numShouldDeny;

        bool good;
        bool bad;

        //set the color of approved numbers depending on how well they did
        if (numApproved == numShouldApprove)
        {
            good = true;
            bad = false;
            approvedNumber.color = Hex("#45A682");
        }
        else if (numApproved <= numShouldApprove && numApproved > 0)
        {
            good = false;
            bad = false;
            approvedNumber.color = Hex("#FFC526");
        }
        else //this will catch when too many were approved 
        {
            good = false;
            bad = true;
            approvedNumber.color = Hex("#D2082E");
        }

        //set the color of denied numbers depending on how well they did
        if (numDenied == numShouldDeny)
        {
            good = true;
            bad = false;
            deniedNumber.color = Hex("#45A682");
        }
        else if (numDenied <= numShouldDeny && numDenied > 0)
        {
            good = false;
            bad = false;
            deniedNumber.color = Hex("#FFC526");
        }
        else //this will catch when too many were denied 
        {
            good = false;
            bad = true;
            deniedNumber.color = Hex("#D2082E");
        }

        //change the ending message depending on how they did
        if (good && !bad)
        {
            endMessage.text = "Wow great job!";
        }
        else if (!good && !bad)
        {
            endMessage.text = "Most of them were right, but we can definitely do better.";
        }
        else if (!good && bad)
        {
            endMessage.text = "We might need to try that again...";
        }

        //show the results one at a time
        approvedText.gameObject.SetActive(true); //approved text
        yield return new WaitForSeconds(0.5f);
        deniedText.gameObject.SetActive(true); //denied text
        yield return new WaitForSeconds(1.0f);
        approvedNumber.gameObject.SetActive(true); //approved number
        yield return new WaitForSeconds(0.5f);
        deniedNumber.gameObject.SetActive(true); //denied number
        yield return new WaitForSeconds(0.5f);
        endMessage.gameObject.SetActive(true); //end message
        yield return new WaitForSeconds(0.5f);

        //activate the exit button
        exitButton.SetActive(true);
    }

    public void OnDoneClicked()
    {
        //change scenes
        StartCoroutine(PlayerManager.instance.LoadPlayerLocations(LevelLoader.instance.previousScene));
    }
}