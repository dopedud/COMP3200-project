using UnityEngine;
using UnityEngine.AI;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Linq;

public class EnemyAIController : Agent {

	private MLEnvManager academy;

	private new Rigidbody rigidbody;

	private EnemyAILooker looker;

	[SerializeField] private float moveSpeed = 2.5f, rotateSpeed = 2.5f;

	[SerializeField] private LayerMask playerMask;

	[SerializeField] private float playerFoundReward = .05f, playerLoseReward = 5;

    private void Awake() {
		academy = transform.root.GetComponent<MLEnvManager>();
		rigidbody = GetComponent<Rigidbody>();

		looker = GetComponentInChildren<EnemyAILooker>();
    }
	
    private void OnTriggerEnter(Collider other) {
        if ((int)Mathf.Pow(2, other.gameObject.layer) != playerMask) return;

		academy.EndEpisode(true);
    }

	public override void OnEpisodeBegin() => academy.Initialise();

	public override void Heuristic(in ActionBuffers actionsOut) {
		ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

		UserInput userInput = InputManager.Instance.userInput;

		discreteActions[0] = (int)userInput.Gameplay.MovementY.ReadValue<float>() + 1;
		discreteActions[1] = (int)userInput.Gameplay.Rotate.ReadValue<float>() + 1;
	}

	public override void OnActionReceived(ActionBuffers actions) {
		if (looker.FindPlayer()) SetReward(playerFoundReward * 5);
		else SetReward(-playerFoundReward);

        Vector3 move = transform.forward * (actions.DiscreteActions[0] - 1);
        Vector3 angle = transform.up * (actions.DiscreteActions[1] - 1);
		
		rigidbody.velocity = move * moveSpeed;
		rigidbody.angularVelocity =  angle * rotateSpeed;
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.localPosition.x);
		sensor.AddObservation(transform.localPosition.z);

		if (academy.initialObjectives.Length == 0) return;

		foreach (var objective in academy.initialObjectives) {
			sensor.AddObservation(objective.localPosition);
			sensor.AddObservation(objective.gameObject.activeInHierarchy);
		}
    }

	public void Respawn(Vector3 position) {
		transform.position = position;
	}

	public void Punish() => SetReward(-playerLoseReward);
	public void Reward() => SetReward(playerLoseReward);

}