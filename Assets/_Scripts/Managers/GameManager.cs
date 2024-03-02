using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager> {

    private void Start() {
        InputManager.Instance.ChangeInput(InputFrom.Enemy, InputState.Gameplay);

        InputManager.Instance.ChangeInput(InputFrom.Player, InputState.Gameplay);
        MenuManager.Instance.ChangeMenu(MenuState.Gameplay);
	}

}
