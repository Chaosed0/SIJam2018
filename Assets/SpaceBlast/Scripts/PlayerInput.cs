using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private CameraControl cameraControl;

    [SerializeField]
    private Gun gun;
    
    private Movement movement;
    private Rewired.Player player;

    private void Awake()
    {
        player = Rewired.ReInput.players.GetPlayer(0);

        movement = GetComponent<Movement>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float strafe = player.GetAxis("Strafe");
        float thrust = player.GetAxis("ThrustHorizontal");
        float thrustVertical = player.GetAxis("ThrustVertical");
        float hLook = player.GetAxis("HorizontalLook");
        float vLook = player.GetAxis("VerticalLook");
        bool fire = player.GetButtonDown("Fire");
        bool escape = player.GetButtonDown("Escape");
        bool ads = player.GetButton("Aim");

        movement.SetThrust(new Vector3(strafe, thrustVertical, thrust), hLook);
        cameraControl.Pitch(vLook);
        cameraControl.SetAds(ads);

        if (fire)
        {
            movement.DoBigThrust();
            gun.Fire();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (escape)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
