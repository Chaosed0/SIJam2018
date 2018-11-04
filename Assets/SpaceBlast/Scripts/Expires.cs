using UnityEngine;
using System.Collections;

public class Expires : MonoBehaviour
{
    [SerializeField]
    private float lifetime;
    private float spawnTime;

    private void Awake()
    {
        spawnTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - spawnTime > lifetime)
        {
            Destroy(this.gameObject);
        }
    }
}
