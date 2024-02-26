using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour {

    private PlayerInput playerInput;

    private Rigidbody rigidbody;

    [SerializeField] private float moveSpeed = 50, drag = 10, aimSensitivity = .75f;

    private CinemachinePOV cmCameraPOV;

    private void Awake() {
        playerInput = InputManager.Instance.playerInput;

        rigidbody = GetComponent<Rigidbody>();
        cmCameraPOV = GetComponentInChildren<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>();

        cmCameraPOV.m_VerticalAxis.m_MaxSpeed = aimSensitivity / 4;
        cmCameraPOV.m_HorizontalAxis.m_MaxSpeed = 0;
    }

    private void LateUpdate() {
        Vector2 aim = playerInput.Gameplay.Aim.ReadValue<Vector2>();
        aim = aim * aimSensitivity / 4;

        transform.Rotate(new Vector3(0, aim.x, 0), Space.World);
    }

    private void FixedUpdate() {
        Vector2 move = playerInput.Gameplay.Movement.ReadValue<Vector2>();

        rigidbody.AddRelativeForce(new Vector3(move.x, 0, move.y) * moveSpeed);
        rigidbody.AddForce(-new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z) * drag);
    }

}
