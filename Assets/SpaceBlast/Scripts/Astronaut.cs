using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Health))]
public class Astronaut : MonoBehaviour
{
    [SerializeField]
    private GameObject replace;

    [SerializeField]
    private GameObject replacement;

    [SerializeField]
    private new Collider collider;

    Health health;

    private void Awake()
    {
        health = GetComponent<Health>();

        health.OnDead.AddListener(OnDead);
    }

    void OnDead()
    {
        replace.SetActive(false);
        collider.enabled = false;
        replacement.SetActive(true);
    }
}
