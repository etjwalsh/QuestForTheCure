using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class FakeSpinner : MonoBehaviour
{
    [SerializeField] private GameObject spinner;
    public bool isRotating;
    [SerializeField] private float speed = 200f;

    public void OnClicked()
    {
        isRotating = !isRotating;
    }

    void Update()
    {
        if (isRotating)
        {
            spinner.transform.Rotate(0f, 0f, speed * Time.deltaTime);
        }
    }
}
