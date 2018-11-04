using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Health))]
public class EnemyDeathHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject deathPrefab;

    private Health health;

    void Awake()
    {
        health = GetComponent<Health>();

        health.OnDead.AddListener(OnDead);
    }

    void OnDead()
    {
        Instantiate<GameObject>(deathPrefab, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
}
