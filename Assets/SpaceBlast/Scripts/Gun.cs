using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private GameObject facingSource;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private float forceImparted = 2000.0f;

    [HideInInspector]
    public bool canFire = false;

    [HideInInspector]
    public bool dealDamage = false;

    public UnityEvent OnFire = new UnityEvent();

    public void Fire()
    {
        if (!canFire)
        {
            return;
        }

        OnFire.Invoke();

        RaycastHit hitInfo;
        bool hit = Physics.SphereCast(transform.position, 0.5f, facingSource.transform.forward, out hitInfo, 5.0f, layerMask, QueryTriggerInteraction.Ignore);
        if (hit)
        {
            OnHitAction(hitInfo);
        }
    }

    void OnHitAction(RaycastHit hitInfo)
    {
        Rigidbody rigidbody = hitInfo.collider.attachedRigidbody;

        Enemy enemy = rigidbody.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.OnHit(gameObject);
        }
        else if (rigidbody != null)
        {
            rigidbody.AddForceAtPosition((rigidbody.transform.position - this.transform.position).normalized * forceImparted, hitInfo.point, ForceMode.Force);
        }

        BlastableDoor blastableDoor = rigidbody.GetComponent<BlastableDoor>();
        if (blastableDoor != null)
        {
            blastableDoor.OnBlast();
        }

        Health health = rigidbody.GetComponent<Health>();
        if (health != null && dealDamage)
        {
            health.AddHealth(-9999f);
        }
    }
}
