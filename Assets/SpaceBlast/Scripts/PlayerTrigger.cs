using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerTrigger : MonoBehaviour
{
    [System.Serializable]
    public class PlayerEvent : UnityEvent<GameObject> { }

    public PlayerEvent OnPlayerEntered = new PlayerEvent();
    public PlayerEvent OnPlayerExited = new PlayerEvent();

    public bool enterOnceOnly = true;
    public bool exitOnceOnly = true;

    private bool enterTriggered = false;
    private bool exitTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (enterOnceOnly && enterTriggered)
        {
            return;
        }

        if (other.tag == "Player")
        {
            enterTriggered = true;
            OnPlayerEntered.Invoke(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (exitOnceOnly && exitTriggered)
        {
            return;
        }

        if (other.tag == "Player")
        {
            exitTriggered = true;
            OnPlayerExited.Invoke(other.gameObject);
        }
    }
}
