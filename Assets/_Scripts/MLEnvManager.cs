using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLEnvManager : Singleton<MLEnvManager> {

    [SerializeField] private PlayerAIController playerAIController;
    [SerializeField] private EnemyAIController enemyAIController;

    [SerializeField] private Transform _playerSpawn;
    public Transform playerSpawn { get { return _playerSpawn; } }
    [SerializeField] private Transform _enemySpawn;
    public Transform enemySpawn { get { return _enemySpawn; } }

    [SerializeField] private Transform[] _objectives;
    public Transform[] objectives { get { return _objectives; } }

    private void Start() => Initialise();

    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.name);
    }

    public void EndEpisode() {
        Initialise();

        enemyAIController.EndEpisode();
    }

    private void Initialise() {
        ResetSpawn();

        enemyAIController.objectives = _objectives;
        playerAIController.SetDestination(_objectives[0].position);
    }
    
    private void ResetSpawn() {
        playerAIController.transform.localPosition = _playerSpawn.localPosition;
        enemyAIController.transform.localPosition = _enemySpawn.localPosition;
    }

}
