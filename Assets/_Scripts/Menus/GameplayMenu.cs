using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayMenu : Menu {

    [SerializeField] private TMP_Text introText, collectionsText;

    [SerializeField] private MLEnvManager mlEnv;

    protected override void Awake() {
        base.Awake();
    }

    private void OnEnable() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mlEnv.OnPlayerCapturedObjective += OnPlayerCapturedObjective;
        mlEnv.OnResetSpawn += OnResetSpawn;
    }

    private void OnDisable() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        mlEnv.OnPlayerCapturedObjective -= OnPlayerCapturedObjective;
        mlEnv.OnResetSpawn -= OnResetSpawn;
    }

    private void OnPlayerCapturedObjective() {
        int orbsLeft = mlEnv.InitialObjectives.Count - mlEnv.ActiveObjectives.Count;

        collectionsText.text = orbsLeft + " / " + mlEnv.InitialObjectives.Count + " orbs left to collect";
    }

    private void OnResetSpawn() {
        int orbsLeft = mlEnv.InitialObjectives.Count - mlEnv.ActiveObjectives.Count;

        introText.text = "COLLECT " + mlEnv.InitialObjectives.Count + " ORBS";
        collectionsText.text = orbsLeft + " / " + mlEnv.InitialObjectives.Count + " orbs left to collect";
    }

}
