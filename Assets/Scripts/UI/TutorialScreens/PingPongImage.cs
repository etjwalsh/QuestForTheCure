using System.Collections;
using UnityEngine;

public class PingPongImage : MonoBehaviour
{
    [Header("Target Points")]
    public RectTransform pointA;
    public RectTransform pointB;

    [Header("Settings")]
    public float moveDuration = 1f;
    public float pauseDuration = 0.5f;
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    // void Awake()
    // {
    //     pauseDuration = Random.Range(0.5f, 1.0f);


    private void OnEnable()
    {
        Debug.Log("Script is running on: " + gameObject.name);
        StartCoroutine(PingPongRoutine());
    }

    private IEnumerator PingPongRoutine()
    {
        while (true)
        {
            //Move A -> B
            yield return StartCoroutine(MoveToPoint(pointB.anchoredPosition));
            yield return new WaitForSeconds(pauseDuration);

            //Move B -> A
            yield return StartCoroutine(MoveToPoint(pointA.anchoredPosition));
            yield return new WaitForSeconds(pauseDuration);
        }
    }

    private IEnumerator MoveToPoint(Vector2 target)
    {
        RectTransform rect = transform as RectTransform;
        Vector2 startPos = rect.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            float curvedT = moveCurve.Evaluate(t);
            rect.anchoredPosition = Vector2.LerpUnclamped(startPos, target, curvedT);
            yield return null;
        }

        rect.anchoredPosition = target;
    }
}