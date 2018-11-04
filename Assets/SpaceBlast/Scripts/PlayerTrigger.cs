using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnPlayerEntered = new UnityEvent();

    [SerializeField]
    private UnityEvent OnPlayerExited = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            OnPlayerEntered.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            OnPlayerExited.Invoke();
        }
    }
}
