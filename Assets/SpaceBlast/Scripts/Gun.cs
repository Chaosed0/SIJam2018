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

    public void Fire()
    {
        OnFire.Invoke();

        RaycastHit hitInfo;
        bool hit = Physics.Raycast(transform.position, facingSource.transform.forward, out hitInfo, 5.0f, ~0, QueryTriggerInteraction.Ignore);
        if (hit)
        {
            Enemy enemy = hitInfo.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.OnHit(gameObject);
            }
        }
    }
}
