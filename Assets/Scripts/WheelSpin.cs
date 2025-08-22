using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelSpin : MonoBehaviour
{
    [SerializeField] private int min = 1;
    [SerializeField] private int max = 11;
    public TMP_Text number;
    public int numberRolled;

    public event System.Action<int> OnRolled;

    public void OnButtonClick()
    {
        numberRolled = Random.Range(min, max);
        number.text = numberRolled.ToString();

        Debug.Log("number rolled = " + numberRolled);

        if (OnRolled != null)
        {
            DebugOnRolled();
            OnRolled.Invoke(numberRolled);
            DebugOnRolled();
        }
    }

    private void DebugOnRolled()
    {
        if (OnRolled != null)
        {
            foreach (var d in OnRolled.GetInvocationList())
            {
                Debug.Log("OnRolled subscriber: " + d.Method.Name + 
                        " on " + d.Target);
            }
        }
        else
        {
            Debug.Log("OnRolled has no subscribers.");
        }
    }

    // public int GetNumberRolled()
    // {
    //     return numberRolled;
    // }
}
