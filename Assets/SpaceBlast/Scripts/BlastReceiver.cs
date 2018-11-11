using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class BlastReceiver : MonoBehaviour
{
    public UnityEvent OnBlasted = new UnityEvent();

	public bool onceOnly=false;

	private bool blasted=false;

    public void DoBlast()
    {
		if (onceOnly && blasted) {
			return;
		}
		blasted = true;
        OnBlasted.Invoke();
    }
}
