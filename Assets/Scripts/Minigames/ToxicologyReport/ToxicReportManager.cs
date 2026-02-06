using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ToxicReportManager : MonoBehaviour
{
    [Header("GeneralSettings")]
    public RectTransform container;
    public CanvasGroup[] screens;
    public float transitionDuration = 0.5f;

    [Header("Game 1 Settings")]
    public List<ToxicTubes> tubes;

    private int currentScreen = 0;
    private Vector2 screenSize;
    public int n = 0;
    public ToxicTubes tubeSelected;

    [Header("Game 2 Settings")]
    public int isGoodDose = -1;

    [Header("Game 3 Settings")]
    public List<Sprite> rats = new List<Sprite>();

    void Awake()
    {
        //set screens 2 and 3 to inactive to start
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
    }

    public void GoToScreen(int screenIndex)
    {
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
    }
}
