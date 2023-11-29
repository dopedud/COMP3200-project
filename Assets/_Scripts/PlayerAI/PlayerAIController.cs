using UnityEngine;
using UnityEngine.AI;

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
		if ((int)Mathf.Pow(2, other.gameObject.layer) != objectiveMask) return;
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
