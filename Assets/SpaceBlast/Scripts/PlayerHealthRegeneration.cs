using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Health))]
public class PlayerHealthRegeneration : MonoBehaviour
{
    [SerializeField]
    private float healTime = 2.5f;

    [SerializeField]
    private float regeneration = 1.0f;

    private Health health;
    private float lastHealTime = 0f;

    private void Awake()
    {
        health = GetComponent<Health>();

        health.OnHealthChanged.AddListener(OnHealthChanged);
    }

    void OnHealthChanged(float newHealth, float oldHealth)
    {
        if (oldHealth < newHealth)
        {
            lastHealTime = Time.time;
        }
    }

    void Update()
    {
        if (Time.time - lastHealTime > healTime && !health.isDead)
        {
            lastHealTime = Time.time;
            health.AddHealth(regeneration);
            Debug.Log("Regenerate");
        }
    }
}
