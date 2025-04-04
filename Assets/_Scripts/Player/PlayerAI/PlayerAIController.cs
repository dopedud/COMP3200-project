using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is an AI-driven player controller script, that simulates how a real player would behave in a real-world 
/// use case. This is done via deterministic means, specifically via finite state machine. This could help diversify 
/// the actions the enemy AI could take in any given time.
/// </summary>
public class PlayerAIController : Player {

	private NavMeshAgent navMeshAgent;

    private Animator animator;

    protected override void Awake() {
        base.Awake();

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

	public override void SetDestination(Vector3 position) {
        navMeshAgent.SetDestination(position);
    }

    public override void Respawn(Vector3 position) {
        navMeshAgent.Warp(position);
    }

    public bool GetPathStale() {
        return navMeshAgent.hasPath;
    }

}
