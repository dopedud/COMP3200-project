using UnityEngine;
using UnityEngine.AI;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class EnemyAIController : Agent {

	private MLEnvManager academy;

	private NavMeshAgent navMeshAgent;

    private EnemyAILooker enemyAILooker;

	[SerializeField] private float speed = 5, rotateMultiplier = 5;

	[SerializeField] private LayerMask playerMask;

	[SerializeField] private int playerFoundReward = 1;

    [SerializeField] private int playerLoseReward = 5;
    [SerializeField] private int playerWinPunishment = 5;

    private void Awake() {
		academy = transform.root.GetComponent<MLEnvManager>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAILooker = GetComponentInChildren<EnemyAILooker>();
    }
	
    private void OnTriggerEnter(Collider other) {
        if ((int)Mathf.Pow(2, other.gameObject.layer) != playerMask) return;

		academy.EndEpisode();
    }

	public void Respawn(Vector3 position) {
		navMeshAgent.enabled = false;
		transform.position = position;
		navMeshAgent.enabled = true;
	}

	public void Punish() => SetReward(-playerWinPunishment);
	public void Reward() => SetReward(playerLoseReward);

	public override void Heuristic(in ActionBuffers actionsOut) {
		ActionSegment<float> continuousActions = actionsOut.ContinuousActions;

		UserInput userInput = InputManager.Instance.userInput;

		continuousActions[0] = userInput.Gameplay.Movement.ReadValue<Vector2>().x;
		continuousActions[1] = userInput.Gameplay.Movement.ReadValue<Vector2>().y;
		continuousActions[2] = userInput.Gameplay.Rotate.ReadValue<float>();
	}

	public override void OnActionReceived(ActionBuffers actions) {
        Vector3 move = new Vector3(actions.ContinuousActions[0], 0, actions.ContinuousActions[1]);
        float angle = actions.ContinuousActions[2] * rotateMultiplier;

        if (enemyAILooker.player != null) {
			navMeshAgent.SetDestination(enemyAILooker.player.position);
			SetReward(playerFoundReward);
		} else {
            transform.Translate(move * speed * Time.fixedDeltaTime);
            transform.RotateAround(transform.position, transform.up, angle);
        }
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localEulerAngles);
        sensor.AddObservation(academy.activeObjectives.Count);
		foreach (var objective in academy.initialObjectives) {
			sensor.AddObservation(objective.position);
			sensor.AddObservation(objective.gameObject.activeInHierarchy);
		}
    }

}
