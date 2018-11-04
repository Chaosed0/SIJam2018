using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour
{
    [SerializeField]
    private float period = 0.5f;

    [SerializeField]
    private float damageToDeal = 0.5f;

    private bool isAttacking = false;
    private float lastAttackTime = 0.0f;
    private Health target = null;

    /*
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
    */

    public void SetTarget(Health health)
    {
        target = health;
    }

    public void DoAttack()
    {
        if (!isAttacking)
        {
            Debug.LogWarning("Attacking while not attacking?", this);
        }

        if (target == null)
        {
            Debug.LogError("Attacking with no target", this);
            return;
        }

        lastAttackTime = Time.time;
        target.AddHealth(-damageToDeal);
    }
}
