﻿using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private CameraControl cameraControl;
    
    private Movement movement;
    private Rewired.Player player;

    private void Awake()
    {
        player = Rewired.ReInput.players.GetPlayer(0);

        movement = GetComponent<Movement>();
    }

    void Update()
    {
        float strafe = player.GetAxis("Strafe");
        float thrust = player.GetAxis("ThrustHorizontal");
        float thrustVertical = player.GetAxis("ThrustVertical");
        float hLook = player.GetAxis("HorizontalLook");
        float vLook = player.GetAxis("VerticalLook");
        bool fire = player.GetButtonDown("Fire");

        movement.SetThrust(new Vector3(strafe, thrustVertical, thrust), hLook);
        cameraControl.Pitch(vLook);

        if (fire)
        {
            movement.DoBigThrust();
        }
    }
}