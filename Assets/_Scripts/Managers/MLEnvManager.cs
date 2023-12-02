using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.AI;

public class MLEnvManager : MonoBehaviour {

    private PlayerAIController playerAIController;
    private EnemyAIController enemyAIController;

    [SerializeField] private Transform[] playerSpawns;
    [SerializeField] private Transform enemySpawn;

	[SerializeField] private Transform[] m_initialObjectives;
	public Transform[] initialObjectives => m_initialObjectives;

	private List<Transform> m_activeObjectives;
	public List<Transform> activeObjectives => m_activeObjectives;

    private void Start() {
		playerAIController = GetComponentInChildren<PlayerAIController>();
		enemyAIController = GetComponentInChildren<EnemyAIController>();
		
		Initialise();
	}

	public void ClearObjective(Transform objective) {
		if (m_activeObjectives.Contains(objective)) {
			objective.gameObject.SetActive(false);
			m_activeObjectives.Remove(objective);
		}

		if (m_activeObjectives.Count > 0) 
		playerAIController.SetDestination(m_activeObjectives[0].position);
		else EndEpisode(false);
	}

	public void EndEpisode(bool hasCapturedPlayer) {
		if (!hasCapturedPlayer) enemyAIController.Punish();
		else enemyAIController.Reward();

		enemyAIController.EndEpisode();
    }

    public void Initialise() {
        ResetSpawn();

		m_activeObjectives = m_initialObjectives.ToList();

		if (m_initialObjectives.Length == 0) return;

		foreach (var objective in m_initialObjectives) objective.gameObject.SetActive(true);

		playerAIController.SetDestination(m_activeObjectives[0].position);
    }
    
    private void ResetSpawn() {
		int playerSpawnIndex = UnityEngine.Random.Range(0, playerSpawns.Length);

        playerAIController.Respawn(playerSpawns[playerSpawnIndex].position);
        enemyAIController.Respawn(enemySpawn.position);
    }

}
