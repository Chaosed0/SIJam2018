using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour
{
    [SerializeField]
    private float period = 1.0f;

    [SerializeField]
    private float damageToDeal = 1.0f;

    private bool isAttacking = false;
    private float lastAttackTime = 0.0f;
    private Health target = null;

    void Update()
    {
        if (target == null)
        {
            return;
        }

        if (Time.time - lastAttackTime > period)
        {
            lastAttackTime = Time.time;
            target.AddHealth(-damageToDeal);
        }
    }

    public void SetTarget(Health health)
    {
        target = health;
    }
}
