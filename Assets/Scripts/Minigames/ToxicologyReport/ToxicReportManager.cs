using UnityEngine;
using System.Collections;

public class ToxicReportManager : MonoBehaviour
{
    public RectTransform container;
    public CanvasGroup[] screens; // Your 3 UI panels
    public float transitionDuration = 0.5f;
    
    private int currentScreen = 0;
    private Vector2 screenSize;
    private int n = 0;
    
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
