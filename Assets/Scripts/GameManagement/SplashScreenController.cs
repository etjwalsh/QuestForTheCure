using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenManager : MonoBehaviour
{
    public CanvasGroup logo;
    public CanvasGroup elcnLogos;

    void Awake()
    {
        logo.alpha = 0;
        elcnLogos.alpha = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SplashSequence());
    }

    public IEnumerator FadeIn(CanvasGroup cg, float duration)
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }

        cg.alpha = 1f;
    }

    public IEnumerator FadeOut(CanvasGroup cg, float duration)
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, t / duration);
            yield return null;
        }

        cg.alpha = 0f;
    }

    public IEnumerator SplashSequence()
    {
        StartCoroutine(FadeIn(elcnLogos, 2.0f)); //fade ELCN Logos in
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(FadeOut(elcnLogos, 2.0f)); //fade ELCN Logos out
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(FadeIn(logo, 2.0f)); //fade Starbit Studio Logo in
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(FadeOut(logo, 2.0f)); //fade Starbit Studio Logo out
        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene("MainMenu"); //load the main menu screen
    }
}
