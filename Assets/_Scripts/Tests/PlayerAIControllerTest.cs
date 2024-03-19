using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAIControllerTest : MonoBehaviour {

    private PlayerAIController playerAIController;

    private bool switchDestination = true;

    [SerializeField] private Transform[] destinations;

    private void Awake() {
        playerAIController = GetComponentInChildren<PlayerAIController>();
    }

    private void Update() {
        if (!playerAIController.GetPathStale()) {
            if (switchDestination) playerAIController.SetDestination(destinations[0].position);
            else playerAIController.SetDestination(destinations[1].position);

            switchDestination = !switchDestination;
        }
    }

}
