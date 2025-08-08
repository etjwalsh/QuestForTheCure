using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelSpin : MonoBehaviour
{
    [SerializeField] int min = 1;
    [SerializeField] int max = 11;
    public TMP_Text number;
    public int numberRolled;

    public void OnButtonClick()
    {
        numberRolled = Random.Range(min, max);
        number.text = numberRolled.ToString();
    }
}
