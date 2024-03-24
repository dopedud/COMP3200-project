using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseMenu : Menu {

    [SerializeField] private Button mainMenuButton;

    protected override void Awake() {
        base.Awake();

        mainMenuButton.onClick.AddListener(() => MenuManager.Instance.ChangeMenu(MenuState.Main));
    }

    protected override void OnDestroy() {
        base.OnDestroy();

        mainMenuButton.onClick.RemoveAllListeners();
    }

}
