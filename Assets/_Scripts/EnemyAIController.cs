using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class EnemyAIController : Agent {

    private NavMeshAgent navMeshAgent;

    private EnemyAILooker looker;

    private Transform[] _objectives;
    public Transform[] objectives { set { _objectives = value; } }

    [SerializeField] private int _playerLoseReward;
    public int playerLoseReward { get { return _playerLoseReward; } }
    [SerializeField] private int _playerWinPunishment;
    public int playerWinPunishment { get { return _playerWinPunishment; } }

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        looker = GetComponentInChildren<EnemyAILooker>();
    }

    // private void Update() {
    //     if (looker.player == null) return;
    //     navMeshAgent.SetDestination(looker.player.position);
    // }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        Debug.Log("Player captured.");
    }

    public override void OnActionReceived(ActionBuffers actions) {
        Vector3 move = new Vector3(actions.ContinuousActions[0], 0, actions.ContinuousActions[1]);
        Vector3 rotate = Vector3.up * ((actions.ContinuousActions[2] + 1) / 2) * 360;

        if (looker.player != null) navMeshAgent.SetDestination(looker.player.position);
        else {
            navMeshAgent.velocity = move;
            transform.eulerAngles = rotate;
        }
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localEulerAngles);
        foreach (var objective in _objectives) sensor.AddObservation(objective.localPosition);
    }

}
