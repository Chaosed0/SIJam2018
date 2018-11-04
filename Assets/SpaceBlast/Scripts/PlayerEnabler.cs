using UnityEngine;
using System.Collections;

public class PlayerEnabler : MonoBehaviour
{
    public void EnableGun(GameObject player)
    {
        player.GetComponent<Player>().SetGunEnabled(true);
    }

    public void EnableScope(GameObject player)
    {
        player.GetComponent<Player>().SetScopeEnabled(true);
    }

    public void EnableKill(GameObject player)
    {
        player.GetComponent<Player>().SetKillEnabled(true);
    }
}
