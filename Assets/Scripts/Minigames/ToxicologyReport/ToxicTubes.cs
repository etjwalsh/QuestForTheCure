using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToxicTubes : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.2f, 5.0f));

            if (isToxic)
            {
                stank.SetActive(true);
                yield return new WaitForSeconds(1.0f);
                stank.SetActive(false);
            }
        }
    }
}
