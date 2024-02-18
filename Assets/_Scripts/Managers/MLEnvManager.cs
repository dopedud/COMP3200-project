using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is the main script for controlling reinforcement learning in the environment. It initialises player and
/// enemy states accordingly, and based on the reset condition, resets accordingly, with player and enemy spawning at
/// their resetting position. It also manages what to do when an episode ends.
/// </summary>
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

    public void Initialise() {
        ResetSpawn();

		m_activeObjectives = m_initialObjectives.ToList();

		if (m_initialObjectives.Length == 0) return;

		foreach (var objective in m_initialObjectives) objective.gameObject.SetActive(true);

		playerAIController.SetDestination(m_activeObjectives[0].position);
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
    
    private void ResetSpawn() {
		int playerSpawnIndex = UnityEngine.Random.Range(0, playerSpawns.Length);

        playerAIController.Respawn(playerSpawns[playerSpawnIndex].position);
        enemyAIController.Respawn(enemySpawn.position);
    }

}
