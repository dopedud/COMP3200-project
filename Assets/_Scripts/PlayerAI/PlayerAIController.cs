using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAIController : MonoBehaviour {

    private NavMeshAgent navMeshAgent;

    private Animator animator;

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        //if (objectives.Length > 0) navMeshAgent.SetDestination(objectives[0].localPosition);
    }

    public void SetDestination(Vector3 position) {
        navMeshAgent.SetDestination(position);
    }

}
