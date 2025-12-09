using TMPro;
using UnityEngine;

public enum TubeType { Cross, Diamond, Vertical, Horizontal, Trash }

public class Tube : MonoBehaviour
{
    public TubeType tubeType;
    public TextMeshProUGUI scoreUI;

    public int score = 0;

    public void AddScore()
    {
        score++;
        UpdateScoreDisplay();
    }

    public void SubtractScore()
    {
        score -= 5;
        if (score <= 0)
        {
            score = 0;
        }

        UpdateScoreDisplay();
    }

    public void MultiplyScore(int value)
    {
        score *= value;
        UpdateScoreDisplay();
    }

    public void DivideScore(int value)
    {
        score /= value;
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        if (scoreUI != null)
        {
            scoreUI.text = "x" + score.ToString();
        }
    }
}
