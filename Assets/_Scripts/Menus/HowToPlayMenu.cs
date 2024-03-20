using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayMenu : Menu {

    public void MainMenu() {
        MenuManager.Instance.ChangeMenu(MenuState.Main);
    }

}
