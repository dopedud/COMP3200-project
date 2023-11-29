using UnityEngine;
using UnityEngine.AI;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Linq;

public class EnemyAIController : Agent {

	private MLEnvManager academy;

	private new Rigidbody rigidbody;

	private RayPerceptionSensorComponentBase rayPerceptionSensor;

	[SerializeField] private float speed = 5, rotateMultiplier = 5;

	[SerializeField] private LayerMask playerMask;

	[SerializeField] private int playerFoundReward = 1;

    [SerializeField] private int playerLoseReward = 5;
    [SerializeField] private int playerWinPunishment = 5;

    private void Awake() {
		academy = transform.root.GetComponent<MLEnvManager>();
		rigidbody = GetComponent<Rigidbody>();
		
		rayPerceptionSensor = GetComponentInChildren<RayPerceptionSensorComponent3D>();
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
        Vector3 move = transform.forward * (actions.DiscreteActions[0] - 1);
        float angle = rotateMultiplier * (actions.DiscreteActions[1] - 1);
		
		rigidbody.velocity = move * speed;
		rigidbody.angularVelocity = transform.up * angle;
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localEulerAngles);

		if (academy.initialObjectives.Length == 0) return;

		foreach (var objective in academy.initialObjectives) {
			sensor.AddObservation(objective.localPosition);
			sensor.AddObservation(objective.gameObject.activeInHierarchy);
		}
    }

	public void Respawn(Vector3 position) {
		transform.position = position;
	}

	public void Punish() => SetReward(-playerWinPunishment);
	public void Reward() => SetReward(playerLoseReward);

}
