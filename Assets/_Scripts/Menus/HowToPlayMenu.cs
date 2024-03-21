using UnityEngine;
using UnityEngine.UI;

public class HowToPlayMenu : Menu {

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
