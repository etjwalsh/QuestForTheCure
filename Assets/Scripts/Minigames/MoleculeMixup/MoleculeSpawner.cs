using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MoleculeSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public float spawnRate = 1.0f;
    public float duration = 30.0f;

    [Header("Element Prefabs")]
    public List<GameObject> elements;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnElement());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator SpawnElement()
    {
        float startTime = Time.time;
        //reference to camera for spawn position
        Camera cam = Camera.main;
        float height = cam.orthographicSize * 2;
        float width = height * cam.aspect;

        while (Time.time - startTime < duration)
        {
            //wait for spawn rate seconds
            yield return new WaitForSeconds(spawnRate);

            //get random index in the list of elements to spawn
            int spawnIndex = Random.Range(0, elements.Count);

            //spawn that one just above the camera's bounds
            Debug.Log("cam x: " + cam.transform.position.x + " and width * 2: " + width * 2);
            Instantiate(elements[spawnIndex], new Vector3(Random.Range(-9, 9), height + 1, -1), Quaternion.identity);

            yield return null;
        }
    }
}
