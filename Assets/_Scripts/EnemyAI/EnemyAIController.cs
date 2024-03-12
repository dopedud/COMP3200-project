using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;

/// <summary>
/// This class is the enemy AI script that holds the reinforcement learning mechanism to learn how to achieve its
/// objective, which is to capture the player, and further optimise its strategy
/// </summary>
public class EnemyAIController : Agent {

	private MLEnvManager academy;

	private new Rigidbody rigidbody;

	private EnemyAILooker looker;

	[SerializeField] private float moveSpeed = 6, rotateSpeed = 2.5f;

	[SerializeField] private LayerMask playerMask;

	[SerializeField] private float
	playerFoundReward = .03f,
	playerLostPunishment = .05f,
	playerCapturedReward = 5;

    [SerializeField] int initialVectorObservationSize = 3;

    private BehaviorParameters behaviorParameters;

    private void Awake() {
		academy = transform.parent.GetComponent<MLEnvManager>();
		rigidbody = GetComponent<Rigidbody>();

		looker = GetComponentInChildren<EnemyAILooker>();

        behaviorParameters = GetComponent<BehaviorParameters>();
        behaviorParameters.BrainParameters.VectorObservationSize = 
        initialVectorObservationSize;
    }
	
    private void OnTriggerEnter(Collider other) {
        if ((1 << other.gameObject.layer | playerMask) != playerMask) return;

		academy.EndEpisode(true);
    }

	public override void OnEpisodeBegin() => academy.Initialise();

	public override void Heuristic(in ActionBuffers actionsOut) {
		ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

		EnemyInput enemyInput = InputManager.Instance.enemyInput;

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
        if (looker.FindPlayer()) AddReward(playerFoundReward);
		else AddReward(-playerLostPunishment);

        sensor.AddObservation(transform.localPosition.x);
		sensor.AddObservation(transform.localPosition.z);

        sensor.AddObservation(transform.localRotation.y);

        // if (academy.InitialObjectives.Length == 0) return;

        // foreach (var objective in academy.InitialObjectives) {
        //     sensor.AddObservation(objective.localPosition);
        //     sensor.AddObservation(objective.gameObject.activeInHierarchy);
        // }
    }

	public void Respawn(Vector3 position) {
		transform.position = position;
	}

	public void Punish() => AddReward(-playerCapturedReward);
	public void Reward() => AddReward(playerCapturedReward);

}
