
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is the main script for controlling reinforcement learning in the environment. It initialises player and
/// enemy states accordingly, and based on the reset condition, resets accordingly, with player and enemy spawning at
/// their resetting position. It also manages what to do when an episode ends.
/// </summary>
public class MLEnvManager : MonoBehaviour {

    public event Action OnResetSpawn, OnPlayerCapturedObjective;

    private Player player;
    private EnemyAIController enemyAIController;

    [SerializeField] private bool randomiseRotation, isTraining;

    [SerializeField] private Transform[] subMLE;

    [SerializeField] private GameObject objectivePrefab;

    private List<GameObject> initialObjectives, activeObjectives;
    public List<GameObject> InitialObjectives => initialObjectives;
    public List<GameObject> ActiveObjectives => activeObjectives;

    private void Awake() {
        player = GetComponentInChildren<Player>();
        enemyAIController = GetComponentInChildren<EnemyAIController>();

        activeObjectives = new();
        initialObjectives = new();
    }

    public void Initialise() => StartCoroutine(InitialiseCoroutine());

    private IEnumerator InitialiseCoroutine() {
        foreach (var objective in initialObjectives) Destroy(objective); 

        initialObjectives.Clear();
        activeObjectives.Clear();

        int subMLEIndex = UnityEngine.Random.Range(0, subMLE.Length);
        for (int i = 0; i < subMLE.Length; i++) {
            if (i != subMLEIndex) subMLE[i].gameObject.SetActive(false);
            else {
                subMLE[i].gameObject.SetActive(true);

                if (randomiseRotation)
                subMLE[i].rotation = Quaternion.Euler(0, UnityEngine.Random.Range(-180f, 180f), 0);
            } 
        }

        yield return null;

        ResetSpawn(subMLE[subMLEIndex]);
    }

    public void ClearObjective(GameObject objective) {
        if (activeObjectives.Contains(objective)) {
            activeObjectives.Remove(objective);
            objective.SetActive(false);

            OnPlayerCapturedObjective?.Invoke();
        }

        int activeObjectivesIndex = UnityEngine.Random.Range(0, activeObjectives.Count);

        if (activeObjectives.Count > 0) 
        player.SetDestination(activeObjectives[activeObjectivesIndex].transform.position);
        else EndEpisode(false);
    }

    public void EndEpisode(bool hasCapturedPlayer) {
        if (isTraining) {
            if (hasCapturedPlayer) enemyAIController.PlayerCapturedReward();
            else enemyAIController.PlayerWinPunish();

            enemyAIController.EndEpisode();
        } else {
            //TODO: game lose
            InputManager.Instance.ChangeInput(InputState.Menu);

            if (hasCapturedPlayer) {
                MenuManager.Instance.ChangeMenu(MenuState.Lose);
            } else { 
                MenuManager.Instance.ChangeMenu(MenuState.Win);
                enemyAIController.gameObject.SetActive(false);
            }
        }
    }
    
    private void ResetSpawn(Transform subMLE) {
        EnemySpawn[] enemySpawns = subMLE.GetComponentsInChildren<EnemySpawn>();
        PlayerSpawn[] playerSpawns = subMLE.GetComponentsInChildren<PlayerSpawn>();
        ObjectiveSpawn[] objectiveSpawns = subMLE.GetComponentsInChildren<ObjectiveSpawn>();

        int enemySpawnIndex = UnityEngine.Random.Range(0, enemySpawns.Length);
        int playerSpawnIndex = UnityEngine.Random.Range(0, playerSpawns.Length);

        enemyAIController.Respawn(enemySpawns[enemySpawnIndex].transform.position);
        player.Respawn(playerSpawns[playerSpawnIndex].transform.position);

        for (int i = 0; i < objectiveSpawns.Length; i++) {
            var objectiveGO = Instantiate(objectivePrefab, 
            objectiveSpawns[i].transform.position, Quaternion.identity, gameObject.transform);

            initialObjectives.Add(objectiveGO);
            activeObjectives.Add(objectiveGO);
        }

        int activeObjectivesIndex = UnityEngine.Random.Range(0, activeObjectives.Count);
        player.SetDestination(activeObjectives[activeObjectivesIndex].transform.position);

        OnResetSpawn?.Invoke();
    }

}
