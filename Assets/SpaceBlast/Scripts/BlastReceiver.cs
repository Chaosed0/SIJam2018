using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class BlastReceiver : MonoBehaviour
{
    public UnityEvent OnBlasted = new UnityEvent();

    public void DoBlast()
    {
        OnBlasted.Invoke();
    }
}
