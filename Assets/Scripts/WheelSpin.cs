using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelSpin : MonoBehaviour
{
    [SerializeField] int min = 1;
    [SerializeField] int max = 10;
    [SerializeField] TMP_Text number;
    public int numberRolled;

    // Start is called before the first frame update
    // void Start()
    // {

    // }

    // // Update is called once per frame
    // void Update()
    // {

    // }

    public void OnButtonClick()
    {
        numberRolled = Random.Range(min, max);
        number.text = numberRolled.ToString();
    }
}
