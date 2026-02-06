using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    private int n = 0;
    public ToxicTubes tubeSelected;

    void Awake()
    {
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
    }

    public void GoToScreen(int screenIndex)
    {
        if (screenIndex < 0 || screenIndex >= screens.Length) return;
        
        StartCoroutine(TransitionToScreen(screenIndex));
    }
    
    private IEnumerator TransitionToScreen(int targetScreen)
    {
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
    }
}
