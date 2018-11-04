using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToSpawn;

    [SerializeField]
    private float minTime = 5.0f;

    [SerializeField]
    private float maxTime = 15.0f;

    private float lastSpawnTime = 0f;
    private float currentSpawnTime = 0f;

    private void OnEnable()
    {
        lastSpawnTime = Time.time;
        currentSpawnTime = Random.Range(minTime, maxTime);
    }

    void Update()
    {
        if ((Time.time - lastSpawnTime) >= currentSpawnTime)
        {
            GameObject go = Instantiate(objectToSpawn, transform.position, transform.rotation);
            go.SetActive(true);

            lastSpawnTime = Time.time;
            currentSpawnTime = Random.Range(minTime, maxTime);
        }
    }
}
