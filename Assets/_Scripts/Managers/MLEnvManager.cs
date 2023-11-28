using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MLEnvManager : MonoBehaviour {

    private PlayerAIController playerAIController;
    private EnemyAIController enemyAIController;

    [SerializeField] private Transform playerSpawn;
    [SerializeField] private Transform enemySpawn;

	[SerializeField] private Transform[] _initialObjectives;
	public Transform[] initialObjectives { get { return _initialObjectives; } }

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
		}

		if (_activeObjectives.Count > 0) 
		playerAIController.SetDestination(_activeObjectives[0].position);
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

		_activeObjectives = _initialObjectives.ToList();
		
		if (_initialObjectives.Length == 0) return;

		foreach (var objective in _initialObjectives) objective.gameObject.SetActive(true);

		playerAIController.SetDestination(_activeObjectives[0].position);
    }
    
    private void ResetSpawn() {
        playerAIController.Respawn(playerSpawn.position);
        enemyAIController.Respawn(enemySpawn.position);
    }

}
