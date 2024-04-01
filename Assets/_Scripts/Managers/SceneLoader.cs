using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneState {
    None,
    Initialisation,
    Main,
    Level1,
    Level2,
    Level3
}

public class SceneLoader : Singleton<SceneLoader> {

    public event Action<SceneState> OnSceneChanged;

    [SerializeField] private SceneState currentSceneState;
    public SceneState CurrentSceneState => currentSceneState;

    protected override void Awake() {
        base.Awake();

        currentSceneState = SceneState.None;

        if (!Enum.TryParse(SceneManager.GetActiveScene().name, false, out currentSceneState)) 
        Debug.LogWarning("Unable to parse current scene.");
    }
    
    public void ChangeScene(SceneState sceneState) {
        StopAllCoroutines();
        StartCoroutine(ChangeSceneCoroutine(sceneState));
    }

    private IEnumerator ChangeSceneCoroutine(SceneState sceneState) {
        Debug.Log(sceneState);

        if (sceneState != SceneState.None) {
            yield return SceneManager.LoadSceneAsync(sceneState.ToString());
            OnSceneChanged?.Invoke(sceneState);
        }
    }

}
