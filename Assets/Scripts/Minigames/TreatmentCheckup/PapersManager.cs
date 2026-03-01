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
    private int percent = -1;
    public List<string> doseLevel = new List<string> { "Too Low...", "Just Right!", "Too High..!" };
    private bool shouldApprove = true;
    private int currentPageIndex = 0;

    [Header("MinigameStats")]
    public int numPages = 5;

    [Header("UI Stats")]
    public Image treatmentImage;
    public Image ratImage;
    public Image ratBG;
    public Image approvalImage;
    public GameObject stamp;
    public TextMeshProUGUI percentText;
    public TextMeshProUGUI doseText;

    void Awake()
    {
        GeneratePage();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(NextPage(currentPageIndex));
    }

    public void OnApproveClicked()
    {
        //check answer
        CheckAnswer(true);
    }
    public void OnDenyClicked()
    {
        CheckAnswer(false);
    }

    private void GeneratePage()
    {
        //check every step of the way to see if shouldApprove should be set to false
        //get random number 1-4 for treatment photo
        treatmentImage.sprite = treatmentPhotos[Random.Range(1, 5)];

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
        doseText.text = doseLevel[randDose];
        if (randDose == 1 || randDose == 3)
        {
            shouldApprove = false;
        }

        //get random number 1-3 for what range for the %
        int percentRange = Random.Range(1, 4);


        //get random number 47-70, 70-89, or 90-99 depending on last random number
        //make the % number change color (red green etc)
        //add generated page to the list
    }
    private void CheckAnswer(bool tf)
    {
        //if tf == true, they asnwered approve

        //if tf == false, they answered deny

        //check if the current page in the list should be approved or denied

        //move to the next page
        currentPageIndex++;
        GeneratePage();
    }

    private IEnumerator NextPage(int index)
    {
        yield return null;
    }
}
