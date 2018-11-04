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
    private UnityEvent OnFire = new UnityEvent();

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private float forceImparted = 200.0f;

    public void Fire()
    {
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

        if (rigidbody != null)
        {
            rigidbody.AddForceAtPosition((rigidbody.transform.position - this.transform.position).normalized * forceImparted, hitInfo.point, ForceMode.Force);
        }
    }
}
