using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// This class is a player controller script, that lets a real-world user control a player. 
/// </summary>
public class PlayerController : Player {

    private PlayerInput playerInput;

    private Rigidbody rigidbody;

    [SerializeField] private float moveSpeed = 50, runMultiplier = 1.5f, drag = 10, aimSensitivity = .75f;

    private CinemachinePOV cmCameraPOV;

    protected override void Awake() {
        base.Awake();
        
        playerInput = InputManager.Instance.playerInput;

        rigidbody = GetComponent<Rigidbody>();
        cmCameraPOV = GetComponentInChildren<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>();

        cmCameraPOV.m_VerticalAxis.m_MaxSpeed = aimSensitivity / 4;
        cmCameraPOV.m_HorizontalAxis.m_MaxSpeed = 0;
    }

    private void Update() {
        Vector2 aim = playerInput.Gameplay.Aim.ReadValue<Vector2>();
        aim = aim * aimSensitivity / 4;

        transform.Rotate(new Vector3(0, aim.x, 0), Space.World);
    }

    private void FixedUpdate() {
        Vector2 move = playerInput.Gameplay.Movement.ReadValue<Vector2>();

        if (playerInput.Gameplay.Run.IsPressed()) 
        rigidbody.AddRelativeForce(moveSpeed * runMultiplier * new Vector3(move.x, 0, move.y));
        else rigidbody.AddRelativeForce(moveSpeed * new Vector3(move.x, 0, move.y));

        rigidbody.AddForce(-new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z) * drag);
    }

    public override void Respawn(Vector3 position) => transform.position = position;

}
