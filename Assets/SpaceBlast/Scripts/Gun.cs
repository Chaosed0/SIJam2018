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

    [SerializeField]
    private float spread = 10.0f;

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

        List<RaycastHit> raycastHits = new List<RaycastHit>();

        for (int i = 0; i < 6; i++)
        {
            RaycastHit hitInfo;
            Vector3 axis = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward) * Vector3.right;
            Quaternion rotation = Quaternion.AngleAxis(Random.Range(-spread, spread), axis);
            bool localHit = Physics.Raycast(transform.position, rotation * facingSource.transform.forward, out hitInfo, 5.0f, layerMask, QueryTriggerInteraction.Ignore);

            Debug.DrawLine(transform.position, transform.position + rotation * facingSource.transform.forward * 5.0f);

            if (localHit)
            {
                raycastHits.Add(hitInfo);
            }
        }

        HashSet<Collider> collidersProcessed = new HashSet<Collider>();
        foreach (RaycastHit hit in raycastHits)
        {
            if (!collidersProcessed.Contains(hit.collider))
            {
                collidersProcessed.Add(hit.collider);
                OnHitAction(hit);
            }
        }
    }

    void OnHitAction(RaycastHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        if (rigidbody == null)
        {
            return;
        }

        Enemy enemy = rigidbody.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.OnHit(gameObject);
        }
        else if (rigidbody != null)
        {
            rigidbody.AddForceAtPosition((rigidbody.transform.position - this.transform.position).normalized * forceImparted, hit.point, ForceMode.Force);
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
