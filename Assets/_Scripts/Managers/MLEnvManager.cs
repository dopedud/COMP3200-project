using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MLEnvManager : MonoBehaviour {

    private PlayerAIController playerAIController;
    private EnemyAIController enemyAIController;

    [SerializeField] private Transform playerSpawn;
    [SerializeField] private Transform enemySpawn;
	[SerializeField] private Transform[] initialObjectives;

	private List<Transform> _activeObjectives;
	public List<Transform> activeObjectives { get { return _activeObjectives; } }

    private void Start() {
		playerAIController = GetComponentInChildren<PlayerAIController>();
		enemyAIController = GetComponentInChildren<EnemyAIController>();
		
		Initialise();
	}

	public void ClearObjective(Transform objective) {
		if (_activeObjectives.Contains(objective)) {
			objective.gameObject.SetActive(false);
			_activeObjectives.Remove(objective);
			Debug.Log("removal successful");
		}

		if (_activeObjectives.Count > 0) playerAIController.SetDestination(_activeObjectives[0].position);
		else EndEpisode();
	}

	public void EndEpisode() {
        enemyAIController.EndEpisode();

		if (_activeObjectives.Count > 0) enemyAIController.Punish();
		else enemyAIController.Reward();

		Initialise();
    }

    private void Initialise() {
        ResetSpawn();

		foreach (var objective in initialObjectives) objective.gameObject.SetActive(true);

        _activeObjectives = initialObjectives.ToList();

		playerAIController.SetDestination(_activeObjectives[0].position);
    }
    
    private void ResetSpawn() {
        playerAIController.Respawn(playerSpawn.position);
        enemyAIController.Respawn(enemySpawn.position);
    }

}
