using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelSpin : MonoBehaviour
{
    //variables for number rolled 1-10
    [SerializeField] private RectTransform wheel; //reference to the wheel ui object's transform - set in inspector
    [SerializeField] private int numberOfSlices = 10;
    public TMP_Text number;
    public int numberRolled;

    public event System.Action<int> OnRolled;

    //variables for spinning animation
    private bool spinning = false;
    private float spinTime;
    private float elapsedTime;
    private float startSpeed;
    private float angle;

    public void OnButtonClick()
    {

        if (!spinning)
        {
            spinTime = Random.Range(3f, 5f); //how long the spinning will last, random between 0 and 5
            startSpeed = 1080f; //how fast it will spin to start, degrees per second
            elapsedTime = 0f;
            spinning = true;
        }
    }

    private void Update()
    {
        if (!spinning) return; //quit this function if it isn't spinning

        //smooth slow down
        float t = elapsedTime / spinTime;
        float currentSpeed = Mathf.Lerp(startSpeed, 0, t * t);

        angle += currentSpeed * Time.deltaTime;
        wheel.localEulerAngles = new Vector3(0, 0, -angle);
        elapsedTime += Time.deltaTime;

        Debug.Log("elapsed time = " + elapsedTime);
        Debug.Log("spin time = " + spinTime);

        if (elapsedTime >= spinTime)
        {
            spinning = false;
            Debug.Log("STOPPING SPIN");

            //calculate where the wheel stopped
            float finalAngle = wheel.localEulerAngles.z;
            numberRolled = GetResult(finalAngle);

            number.text = numberRolled.ToString();

            Debug.Log("number rolled = " + numberRolled);

            OnRolled?.Invoke(numberRolled);

        }

    }

    private int GetResult(float zRotation)
    {
        float normalized = (360 - zRotation % 360) % 360;
        float sliceSize = 360f / numberOfSlices;
        int result = Mathf.FloorToInt(normalized / sliceSize) + 1;
        return Mathf.Clamp(result, 1, numberOfSlices);
    }
}
