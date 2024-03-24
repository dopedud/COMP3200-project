using UnityEngine;

[System.Serializable]
public class MenuData {

    [SerializeField] private MenuState menuState;
    public MenuState MenuState => menuState;
    public bool enabled;
    private int v;

    public MenuData(MenuState menuState) => this.menuState = menuState;
    public MenuData(int menuState) => this.menuState = (MenuState)menuState;
}
