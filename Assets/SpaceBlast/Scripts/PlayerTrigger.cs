using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerTrigger : MonoBehaviour
{
    [System.Serializable]
    public class PlayerEvent : UnityEvent<GameObject> { }

    public PlayerEvent OnPlayerEntered = new PlayerEvent();
    public PlayerEvent OnPlayerExited = new PlayerEvent();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            OnPlayerEntered.Invoke(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            OnPlayerExited.Invoke(other.gameObject);
        }
    }
}
