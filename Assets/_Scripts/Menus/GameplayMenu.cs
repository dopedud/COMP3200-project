using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMenu : Menu {

    protected override void Awake() {
        base.Awake();
    }

    private void OnEnable() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
