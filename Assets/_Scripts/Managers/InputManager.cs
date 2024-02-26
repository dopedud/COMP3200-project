using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputFrom { Player, Enemy }

public enum InputState { Menu, Gameplay }

public class InputManager : Singleton<InputManager> {

    public PlayerInput playerInput { get; private set; }
    public EnemyInput enemyInput { get; private set; }

    protected override void Awake() {
        base.Awake();

        playerInput = new PlayerInput();
        enemyInput = new EnemyInput();
    }

    public void ChangeInput(InputFrom inputFrom, InputState inputState) {
        try {
            InputActionMap actionMap = null;

            switch (inputFrom) {
                case InputFrom.Enemy:
                    enemyInput.Disable();
            
                    actionMap = Array.Find(enemyInput.asset.actionMaps.ToArray(), actionMap => 
                    actionMap.name.Equals(inputState.ToString()));
                break;

                case InputFrom.Player:
                    playerInput.Disable();

                    actionMap = Array.Find(playerInput.asset.actionMaps.ToArray(), actionMap => 
                    actionMap.name.Equals(inputState.ToString()));
                break;

                default:
                    Debug.LogError("Cannot find input from given.");
                break;
            }

            actionMap.Enable();
        } catch (ArgumentNullException) {
            Debug.LogError("Cannot find input state given.");
        }
    }

}
