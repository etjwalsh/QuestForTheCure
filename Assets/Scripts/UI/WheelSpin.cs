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
    // private float angle;

    public void OnButtonClick()
    {
        if (!spinning)
        {
            spinTime = Random.Range(3f, 6f); //how long the spinning will last, random between 0 and 5
            if (Random.value < 0.5f)
            {
                startSpeed = Random.Range(-1440f, -720f); //how fast it will spin to start, degrees per second
            }
            else
            {
                startSpeed = Random.Range(720f, 1440f);
            }

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

        wheel.Rotate(0, 0, -currentSpeed * Time.deltaTime);

        // angle += currentSpeed * Time.deltaTime;
        // wheel.localEulerAngles = new Vector3(0, 0, -angle);
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= spinTime)
        {
            spinning = false;

            //calculate where the wheel stopped
            float finalAngle = wheel.localEulerAngles.z;
            numberRolled = GetResult(finalAngle);

            Debug.Log("number rolled = " + numberRolled);

            OnRolled?.Invoke(numberRolled);
        }
    }

    //buncha weird math to return the top of the wheel based on how much it rotates
    private int GetResult(float zRotation)
    {
        Debug.Log("zRotation = " + zRotation);
        float normalized = zRotation % 360;
        if (normalized < 0)
        {
            normalized += 360f;
        }

        Debug.Log("normalized = " + normalized);

        float offset = 0f;
        normalized = (normalized + offset) % 360;

        float sliceSize = 360f / numberOfSlices;
        int result = Mathf.FloorToInt(normalized / sliceSize) + 1;
        return Mathf.Clamp(result, 1, numberOfSlices);
    }
}
