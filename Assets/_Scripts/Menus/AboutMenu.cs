using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutMenu : Menu {

    public void MainMenu() {
        MenuManager.Instance.ChangeMenu(MenuState.Main);
    }

}
