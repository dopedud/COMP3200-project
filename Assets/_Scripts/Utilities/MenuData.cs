using UnityEngine;

[System.Serializable]
public class MenuData {

    [SerializeField] private MenuState menuState;
    public MenuState MenuState => menuState;
    public bool enabled;

    public MenuData(MenuState menuState) => this.menuState = menuState;

}
