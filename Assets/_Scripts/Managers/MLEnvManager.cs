using System;
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

	[SerializeField, Min(0)] private float randomPlayerSpawnRadius;

	[SerializeField, Min(0)] private float secondsPerEpisode = 10;
	private float timeLeft;

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
		float randomX = UnityEngine.Random.Range(-1, 1);
		float randomY = UnityEngine.Random.Range(-1, 1);
		Vector3 playerSpawnOffset = new Vector3(randomX, 0 ,randomY) * randomPlayerSpawnRadius;
		if (playerSpawnOffset.magnitude > randomPlayerSpawnRadius) 
		playerSpawnOffset = playerSpawnOffset.normalized;

        playerAIController.Respawn(playerSpawn.position + playerSpawnOffset);
        enemyAIController.Respawn(enemySpawn.position);
    }

}
