using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : Menu {

    [SerializeField] private Button[] levelButtons;

    [SerializeField] private Button mainMenuButton;

    protected override void Awake() {
        base.Awake();

        for (int i = 0; i < levelButtons.Length; i++) {
            int index = i;
            levelButtons[index].onClick.AddListener(() => {
                SceneLoader.Instance.ChangeScene(SceneState.Level1 + index);
                InputManager.Instance.ChangeInput(InputState.Gameplay);
                MenuManager.Instance.ChangeMenu(MenuState.Gameplay);
            });
        }

        mainMenuButton.onClick.AddListener(() => MenuManager.Instance.ChangeMenu(MenuState.Main));
    }

    protected override void OnDestroy() {
        base.OnDestroy();

        for (int i = 0; i < levelButtons.Length; i++)
        levelButtons[i].onClick.RemoveAllListeners();

        mainMenuButton.onClick.RemoveAllListeners();
    }

}
