using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager> {

    [SerializeField] InputState InputInitialiser;
    [SerializeField] MenuState MenuInitialiser;

    private void Start() {
        InputManager.Instance.ChangeInput(InputFrom.Enemy, InputState.Gameplay);

        InputManager.Instance.ChangeInput(InputFrom.Player, InputInitialiser);
        MenuManager.Instance.ChangeMenu(MenuInitialiser);

        if (SceneLoader.Instance.CurrentSceneState == SceneState.Initialisation)
        SceneLoader.Instance.ChangeScene(SceneState.Main);
	}

}
