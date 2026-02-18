using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DoseChecker : MonoBehaviour
{
    [Header("Target Fill Range")]
    [SerializeField] private float minGoodFill;
    [SerializeField] private float maxGoodFill;
    [SerializeField] private Sprite[] letEmKnowSprites;
    AnimatorStateInfo stateInfo;
    float normalizedTime;
    float fillLevel;


    [Header("References")]
    [SerializeField] private Animator animator; //ref to syringe animator
    [SerializeField] private ToxicReportManager trm;
    public Image letEmKnowImage;

    private bool gameActive = false; //bool to check whether or not the game is still going

    void Start()
    {
        //disable the image to start
        letEmKnowImage.sprite = letEmKnowSprites[0];

        //start the game
        StartCoroutine(StartGame());

        //set the good fills to be random
        minGoodFill = Random.Range(.2f, .6f);
        maxGoodFill = minGoodFill + Random.Range(.2f, .3f);
        Debug.Log("heres the range: " + minGoodFill + " - " + maxGoodFill);
    }

    public IEnumerator StartGame()
    {
        //wait to make sure that the screen is fully transitioned over before the player can click
        yield return new WaitForSeconds(1.0f);

        Debug.Log("you can click now");

        gameActive = true;
        animator.speed = 2.0f;
        animator.Play("Syringe", 0, 0f); //start anim from the beginning
    }

    // Update is called once per frame
    void Update()
    {
        //get current animation time (0 to 1, where 1 = 4.1 seconds)
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        normalizedTime = stateInfo.normalizedTime % 1f;

        //calculate actual fill level
        if (normalizedTime <= 0.5f)
        {
            //going forward: 0 to 0.5 maps to 0% to 100%
            fillLevel = normalizedTime * 2f;
        }
        else
        {
            //going backward: 0.5 to 1.0 maps to 100% to 0%
            fillLevel = (1f - normalizedTime) * 2f;
        }

        //check if the fill level is currently in the good range
        if (fillLevel >= minGoodFill && fillLevel <= maxGoodFill)
        {
            letEmKnowImage.sprite = letEmKnowSprites[0];
        }
        else
        {
            letEmKnowImage.sprite = letEmKnowSprites[1];
        }

        //check if the player clicks the mouse
        if (gameActive && Input.GetMouseButtonDown(0))
        {
            //stop the animation and check to see if the player clicked at the right time
            StartCoroutine(FreezeAndCheck());
        }
    }

    private IEnumerator FreezeAndCheck()
    {
        //freeze the animation and stop the game
        animator.speed = 0f;
        gameActive = false;

        //check if within good range
        if (fillLevel >= minGoodFill && fillLevel <= maxGoodFill)
        {
            Debug.Log("Great dose");
            trm.isGoodDose = 0;
        }
        else if (fillLevel <= minGoodFill)//otherwise not enough fill
        {
            Debug.Log("Too little dose");
            trm.isGoodDose = 1;
        }
        else //should just catch if its too much dose
        {
            Debug.Log("Too much dose");
            trm.isGoodDose = 2;
        }

        yield return new WaitForSeconds(1.0f);

        //move to the next screen
        trm.GoToScreen(++trm.n);
    }
}
