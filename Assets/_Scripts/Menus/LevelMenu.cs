using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : Menu {

    [SerializeField] private Button[] levelButtons;

    [SerializeField] private Button mainMenuButton;

    protected override void Awake() {
        base.Awake();

        for (int i = 0; i < levelButtons.Length; i++)
        levelButtons[i].onClick.AddListener(() => Debug.Log("Level " + i + " pressed."));

        mainMenuButton.onClick.AddListener(() => MenuManager.Instance.ChangeMenu(MenuState.Main));
    }

    protected override void OnDestroy() {
        base.OnDestroy();

        for (int i = 0; i < levelButtons.Length; i++)
        levelButtons[i].onClick.RemoveAllListeners();

        mainMenuButton.onClick.RemoveAllListeners();
    }

}
