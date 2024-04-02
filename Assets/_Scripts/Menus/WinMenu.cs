using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinMenu : Menu {

    [SerializeField] private Button mainMenuButton;

    protected override void Awake() {
        base.Awake();

        mainMenuButton.onClick.AddListener(() => {
            if (SceneLoader.Instance.CurrentSceneState == SceneState.None) return;
            MenuManager.Instance.ChangeMenu(MenuState.Main);
            SceneLoader.Instance.ChangeScene(SceneState.Main);
        });
    }

    protected override void OnDestroy() {
        base.OnDestroy();

        mainMenuButton.onClick.RemoveAllListeners();
    }

}
