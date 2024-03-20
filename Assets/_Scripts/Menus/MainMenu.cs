using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Menu {

    [SerializeField] private Button playButton, howToPlayButton, aboutButton, quitButton;

    protected override void Awake() {
        base.Awake();

        playButton.onClick.AddListener(() => MenuManager.Instance.ChangeMenu(MenuState.Levels));
        howToPlayButton.onClick.AddListener(() => MenuManager.Instance.ChangeMenu(MenuState.HowToPlay));
        aboutButton.onClick.AddListener(() => MenuManager.Instance.ChangeMenu(MenuState.About));
        quitButton.onClick.AddListener(() => Application.Quit());
    }

    protected override void OnDestroy() {
        base.OnDestroy();

        playButton.onClick.RemoveAllListeners();
        howToPlayButton.onClick.RemoveAllListeners();
        aboutButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
    }

}
