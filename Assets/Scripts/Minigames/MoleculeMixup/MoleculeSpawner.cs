using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MoleculeSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public float spawnRate = 1.0f;
    public float duration;

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
        float ogSpawnRate = spawnRate;
        bool oneFourth = false;
        bool oneHalf = false;
        bool threeFourths = false;

        Debug.Log("time - startTime: " + (Time.time - startTime));
        Debug.Log("duration: " + duration);
        Debug.Log("duration * 0.75: " + (duration * 0.75));
        Debug.Log("duration * 0.50: " + (duration * 0.50));
        Debug.Log("duration * 0.25: " + (duration * 0.25));

        while (Time.time - startTime < duration)
        {
            //decrease the spawn rate as time goes on
            if (Time.time - startTime < (duration * 0.75) && oneFourth) //if time is 25% completed
            {
                Debug.Log("spawn rate now at 75%");
                spawnRate = ogSpawnRate * 0.75f;
                oneFourth = true;
            }
            if (Time.time - startTime < (duration * 0.50) && oneHalf) //if time is 50% completed
            {
                Debug.Log("spawn rate now at 50%");
                spawnRate = ogSpawnRate * 0.50f;
                oneHalf = true;
            }
            if (Time.time - startTime < (duration * 0.50) && threeFourths) //if time is 75% completed
            {
                Debug.Log("spawn rate now at 25%");
                spawnRate = ogSpawnRate * 0.25f;
                threeFourths = true;
            }

            //wait for spawn rate seconds
            yield return new WaitForSeconds(spawnRate);

            //get random index in the list of elements to spawn
            int spawnIndex = Random.Range(0, elements.Count);

            //spawn that one just above the camera's bounds
            GameObject newElement = Instantiate(elements[spawnIndex], new Vector3(Random.Range(-9, 5.75f), height + 0.5f, -1), Quaternion.identity);

            //lock rotation
            newElement.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

            //change how fast they fall at random
            newElement.GetComponent<Rigidbody>().drag = Random.Range(5, 15);

            yield return null;
        }
    }
}
