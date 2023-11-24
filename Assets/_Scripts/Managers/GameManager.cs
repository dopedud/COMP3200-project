using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager> {

    protected override void Awake() {
		InputManager.Instance.ChangeActionMap(InputState.Gameplay);
	}

}
