using UnityEngine;
using UnityEngine.Events;

public class TriggerEventer : MonoBehaviour
{
    [System.Serializable]
    public class TriggerEnteredEvent : UnityEvent<Collider> { }
    public TriggerEnteredEvent OnTriggerEntered = new TriggerEnteredEvent();

    [System.Serializable]
    public class TriggerExitedEvent : UnityEvent<Collider> { }
    public TriggerExitedEvent OnTriggerExited = new TriggerExitedEvent();

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEntered.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExited.Invoke(other);
    }
}
