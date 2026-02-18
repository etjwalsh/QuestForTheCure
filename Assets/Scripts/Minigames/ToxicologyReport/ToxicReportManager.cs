using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;

public class ToxicReportManager : MonoBehaviour
{
    [Header("GeneralSettings")]
    public RectTransform container;
    public CanvasGroup[] screens;
    public float transitionDuration = 0.5f;
    public bool isToxic;

    [Header("Game 1 Settings")]
    public List<ToxicTubes> tubes;

    private int currentScreen = 0;
    private Vector2 screenSize;
    public int n = 0;
    public ToxicTubes tubeSelected;

    [Header("Game 2 Settings")]
    public int isGoodDose = -1;

    [Header("Game 3 Settings")]
    public Animator curtains;
    public Image rat;
    public GameObject syringe;
    public List<Sprite> rats = new List<Sprite>();
    public List<Sprite> ratsHappy = new List<Sprite>();
    public List<Sprite> ratsSad = new List<Sprite>();
    private int ratNum;
    public TextMeshProUGUI endText;
    public GameObject exitButton;


    public static ToxicReportManager instance { get; private set; }
    void Awake()
    {
        //singleton pattern
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        //get which rat will be displayed
        ratNum = Random.Range(0,6);
        rat.sprite = rats[ratNum];
        Debug.Log("rat to use: " + rats[ratNum]);

        //set screens 2 and 3 to inactive to start
        exitButton.SetActive(false);
        screens[1].gameObject.SetActive(false);
        screens[2].gameObject.SetActive(false);

        //set one of the tubes to be a good one
        tubes[Random.Range(0, tubes.Count - 1)].isToxic = false;
    }

    void Start()
    {
        screenSize = GetComponent<RectTransform>().rect.size;

        // Position screens side by side
        for (int i = 0; i < screens.Length; i++)
        {
            screens[i].GetComponent<RectTransform>().anchoredPosition =
                new Vector2(i * screenSize.x, 0);
        }
    }

    public void OnButtonPressed()
    {
        n++;
        GoToScreen(n);

        //save a reference to the tube you clicked on
        tubeSelected = EventSystem.current.currentSelectedGameObject.GetComponent<ToxicTubes>();

        //check if that tube was toxic
        if(tubeSelected.isToxic)
        {
            isToxic = true;
        }
    }

    public void GoToScreen(int screenIndex)
    {
        Debug.Log("going to: " + currentScreen);
        if (screenIndex < 0 || screenIndex >= screens.Length) return;

        StartCoroutine(TransitionToScreen(screenIndex));
    }

    private IEnumerator TransitionToScreen(int targetScreen)
    {
        //activate the target screen
        screens[targetScreen].gameObject.SetActive(true);

        Vector2 startPos = container.anchoredPosition;
        Vector2 targetPos = new Vector2(-targetScreen * screenSize.x, 0);

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;

            // Ease in-out for smoother motion
            t = t * t * (3f - 2f * t);

            container.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        container.anchoredPosition = targetPos;
        currentScreen = targetScreen;

        //deactivate the previous screen
        screens[targetScreen - 1].gameObject.SetActive(false);

        //check if you are on the third screen
        if(currentScreen == 2)
        {
            StartCoroutine(Screen3Changes());
        }
    }

    private IEnumerator Screen3Changes()
    {
        yield return new WaitForSeconds(transitionDuration);
        curtains.Play("Curtains");
        yield return new WaitForSeconds(2.0f);
        
        //check if they chose a toxic tube
        if(isToxic || isGoodDose == 2)
        {
            Debug.Log("this is a sad rat " + ratsSad[ratNum]);
            rat.sprite = ratsSad[ratNum];

            if(isGoodDose == 2)
            {
                //this means dose is too high
                endText.text = "I think the dose was too high...";
            }
            else if(isToxic)
            {
                //this means that the drug was toxic
                endText.text = "I think we chose the wrong test tube...";
            }
        }
        //check if they got everything right
        else if (!isToxic && isGoodDose == 0)
        { 
            Debug.Log("this is a happy rat "+ ratsHappy[ratNum]);
            rat.sprite = ratsHappy[ratNum];
            endText.text = "Wow it worked great!";
        }
        else if (!isToxic && isGoodDose == 1)
        {
            Debug.Log("this is a normal rat"+ rats[ratNum]);
            rat.sprite = rats[ratNum];
            endText.text = "Maybe we need to up the dosage next time...";
        }
        else
        {
            Debug.Log("this should never be the case");
        }

        //get rid of the syringe
        Debug.Log("about to set the syringe to false");
        syringe.SetActive(false);

        //open the curtains
        Debug.Log("about to open up the curtains");
        curtains.Play("CurtainsBackwards");

        yield return new WaitForSeconds(1.5f);
        exitButton.SetActive(true);
    }
}
