using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToxicTubes : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public bool isToxic = true;

    [SerializeField] private float hoverOffset = 20f;
    [SerializeField] private float animationSpeed = 10f;
    public GameObject stank;
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Vector2 targetPosition;

    void Awake()
    {
        stank.SetActive(false);
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        targetPosition = originalPosition;

        StartCoroutine(Stank());
    }

    void Update()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, Time.deltaTime * animationSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetPosition = originalPosition + Vector2.up * hoverOffset;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetPosition = originalPosition;
    }

    private IEnumerator Stank()
    {
        yield return new WaitForSeconds(0.2f);

        if(isToxic)
        {
            stank.SetActive(true);
        }
    }
}
