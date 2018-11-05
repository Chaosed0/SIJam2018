using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class EndTrigger : MonoBehaviour {
	[SerializeField]
	private Health astronaut;

	public void OnEnterTrigger(GameObject player)
	{
		if (astronaut.isDead) {
			player.GetComponent<Player>().Lose ();
		} else {
			player.GetComponent<Player>().Win ();
		}
	}
}
