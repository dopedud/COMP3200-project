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

    private Player player;
    private EnemyAIController enemyAIController;

    [SerializeField] private Transform[] subMLE;

	private Transform[] m_initialObjectives;
    private List<Transform> m_activeObjectives;
	public Transform[] InitialObjectives => m_initialObjectives;
	public List<Transform> ActiveObjectives => m_activeObjectives;

    private void Start() {
		player = GetComponentInChildren<Player>();
		enemyAIController = GetComponentInChildren<EnemyAIController>();
		
		Initialise();
	}

    public void Initialise() {
        int subMLEIndex = UnityEngine.Random.Range(0, subMLE.Length);
        for (int i = 0; i < subMLE.Length; i++) {
            if (i == subMLEIndex) subMLE[i].gameObject.SetActive(true);
            else subMLE[i].gameObject.SetActive(false);
        }

        ResetSpawn(subMLE[subMLEIndex]);

        // TODO: do objectives on seperate floor plans
        // if (m_initialObjectives.Length == 0) return;

        // m_activeObjectives = m_initialObjectives.ToList();

        // foreach (var objective in m_initialObjectives) objective.gameObject.SetActive(true);

        // player.SetDestination(m_activeObjectives[0].position);
    }

	public void ClearObjective(Transform objective) {
		if (m_activeObjectives.Contains(objective)) {
			objective.gameObject.SetActive(false);
			m_activeObjectives.Remove(objective);
		}

		if (m_activeObjectives.Count > 0) 
		player.SetDestination(m_activeObjectives[0].position);
		else EndEpisode(false);
	}

	public void EndEpisode(bool hasCapturedPlayer) {
		if (!hasCapturedPlayer) enemyAIController.Punish();
		else enemyAIController.Reward();

		enemyAIController.EndEpisode();
    }
    
    private void ResetSpawn(Transform subMLE) {
        EnemySpawn enemySpawn = subMLE.GetComponentInChildren<EnemySpawn>();
        PlayerSpawn[] playerSpawns = subMLE.GetComponentsInChildren<PlayerSpawn>();

		int playerSpawnIndex = UnityEngine.Random.Range(0, playerSpawns.Length);

        player.Respawn(playerSpawns[playerSpawnIndex].transform.position);
        enemyAIController.Respawn(enemySpawn.transform.position);
    }

}
