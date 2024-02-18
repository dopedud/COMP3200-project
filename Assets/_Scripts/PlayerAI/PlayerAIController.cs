using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is a player controller script, that simulates how a real player would behave in a real-world use case.
/// This is done via deterministic means, specifically via finite state machine. This could help diversify the actions
/// the enemy AI could take in any given time.
/// </summary>
public class PlayerAIController : MonoBehaviour {

	private MLEnvManager academy;

	private NavMeshAgent navMeshAgent;

	[SerializeField] private LayerMask objectiveMask;

    private Animator animator;

    private void Awake() {
		academy = transform.root.GetComponent<MLEnvManager>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

	private void OnTriggerEnter(Collider other) {
		if ((1 << other.gameObject.layer | objectiveMask) != objectiveMask) return;
        
		academy.ClearObjective(other.transform);
	}

	public void SetDestination(Vector3 position) {
        navMeshAgent.SetDestination(position);
    }

	public void Respawn(Vector3 position) {
		navMeshAgent.enabled = false;
		transform.position = position;
		navMeshAgent.enabled = true;
	}

}
