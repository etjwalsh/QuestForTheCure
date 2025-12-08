using TMPro;
using UnityEngine;

public enum TubeType { Cross, Diamond, Vertical, Horizontal, Trash }

public class Tube : MonoBehaviour
{
    public TubeType tubeType;
    public TextMeshProUGUI scoreUI;

    private int score = 0;

    public void AddScore()
    {
        score++;
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
