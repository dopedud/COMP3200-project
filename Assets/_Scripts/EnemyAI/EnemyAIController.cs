using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// This class is the enemy AI script that holds the reinforcement learning mechanism to learn how to achieve its
/// objective, which is to capture the player, and further optimise its strategy
/// </summary>
public class EnemyAIController : Agent {

    private MLEnvManager academy;

    private new Rigidbody rigidbody;

    [SerializeField] private GameObject collider;
    private CapsuleCollider colliderComponent;

    private EnemyAILooker looker;

    private EnemyInput enemyInput;

    private Vector3 previousPosition;

    [SerializeField] private float moveSpeed = 6, rotateSpeed = 2.5f;

    [SerializeField] private LayerMask playerMask;
    [SerializeField] private string EnemyNavMeshLayerName = "Enemy";

    [SerializeField] private float
    playerFoundReward = .03f,
    playerLostPunishment = .05f,
    playerCapturedReward = 5;

    [SerializeField] private float
    explorationRadius = 1.8f,
    explorationReward = .15f;

    [SerializeField] private int explorationStepSize = 10;
    private int explorationStep;

    private BufferSensorComponent bufferSensor;

    private void Awake() {
        academy = transform.parent.GetComponent<MLEnvManager>();
        rigidbody = GetComponent<Rigidbody>();
        colliderComponent = collider.GetComponent<CapsuleCollider>();

        looker = GetComponentInChildren<EnemyAILooker>();
        enemyInput = InputManager.Instance.enemyInput;

        bufferSensor = GetComponent<BufferSensorComponent>();
    }
    
    private void OnTriggerEnter(Collider other) {
        if ((1 << other.gameObject.layer | playerMask) != playerMask) return;

        academy.EndEpisode(true);
    }

    public override void OnEpisodeBegin() { 
        academy.Initialise();
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        discreteActions[0] = (int)enemyInput.Gameplay.MovementY.ReadValue<float>() + 1;
        discreteActions[1] = (int)enemyInput.Gameplay.Rotate.ReadValue<float>() + 1;
    }

    public override void OnActionReceived(ActionBuffers actions) {
        Vector3 move = transform.forward * (actions.DiscreteActions[0] - 1);
        Vector3 angle = transform.up * (actions.DiscreteActions[1] - 1);
        
        rigidbody.velocity = move * moveSpeed;
        rigidbody.angularVelocity =  angle * rotateSpeed;
    }

    public override void CollectObservations(VectorSensor sensor) {
        var rayOutputs = looker.RayPerceptionSensor.RaySensor.RayPerceptionOutput.RayOutputs;

        try {
            if (rayOutputs.Any(rayOutput => rayOutput.HitTagIndex == 0)) AddReward(playerFoundReward);
            else AddReward(-playerLostPunishment);
        } catch (ArgumentNullException) {}

        explorationStep++;
        if (explorationStep >= explorationStepSize) {
            Vector3 raycastDirection = transform.position - previousPosition;
            Vector3 startingPosition = previousPosition + Vector3.up * 0.5f;

            if (raycastDirection.sqrMagnitude > 2 * colliderComponent.radius) {
                if (!Physics.Raycast(startingPosition, raycastDirection,
                out RaycastHit hit, explorationRadius, ~0, QueryTriggerInteraction.Ignore)) {
                    AddReward(explorationReward);
                } else {
                    var hitGO = hit.collider.gameObject;
                    if ((1 << hitGO.layer | 1 << collider.layer) != 1 << collider.layer) {
                        AddReward(explorationReward);
                    }
                }
            }
            
            previousPosition = transform.position;

            explorationStep = 0;
        }

        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.z);

        sensor.AddObservation(transform.rotation.y);

        if (academy.InitialObjectives.Count == 0) return;

        foreach (var objective in academy.InitialObjectives) {
            NavMeshPath path = new();

            if (!NavMesh.CalculatePath(transform.position, objective.transform.position, 
            NavMesh.GetAreaFromName(EnemyNavMeshLayerName), path)) continue;

            try {
                var navDirection = path.corners[1] - transform.position;
                Debug.DrawRay(transform.position, navDirection, Color.cyan, .1f);

                bufferSensor.AppendObservation(new float[] { 
                    navDirection.x,
                    navDirection.y,
                    navDirection.z,
                    objective.activeInHierarchy ? 1 : 0
                });
            } catch (IndexOutOfRangeException) {}
        }
    }

    public void Respawn(Vector3 position) {
        transform.position = position;
        previousPosition = position;
    }

    public void PlayerWinPunish() => AddReward(-playerCapturedReward);
    public void PlayerCapturedReward() => AddReward(playerCapturedReward);

}
