using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputState { Menu, Gameplay }

public class InputManager : Singleton<InputManager> {

    public UserInput userInput { get; private set; }

    protected override void Awake() {
        base.Awake();

        userInput = new UserInput();
    }

    public void ChangeActionMap(InputState inputState) {
        userInput.Disable();

        InputActionMap actionMap = 
        Array.Find(userInput.asset.actionMaps.ToArray(), actionMap => actionMap.name.Equals(inputState.ToString()));

        actionMap.Enable();
    }

}
