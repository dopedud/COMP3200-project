using System;
using UnityEngine;
using System.Linq;

public enum MenuState { None, Main, Gameplay, Dialogue, Win, Lose, }

public class MenuManager : Singleton<MenuManager> {

    public event Action OnMenuChanged;

    [SerializeField] private MenuState currentMenuState;
    public MenuState CurrentMenuState => currentMenuState;

    [SerializeField] private MenuData[] menus;
    public MenuData[] Menus => menus;

    protected override void Awake() {
        base.Awake();

        menus = new MenuData[] {
            new(MenuState.Main),
            new(MenuState.Gameplay),
            new(MenuState.Dialogue),
            new(MenuState.Win),
            new(MenuState.Lose),
        };
    }

    public void ChangeMenu(string menuName) {
        if (Enum.TryParse(menuName, false, out MenuState menuState)) ChangeMenu(menuState);
        else Debug.LogWarning("Unable to parse given menu.");
    }

    public void ChangeMenu(MenuState menuState) {
        foreach (var menu in menus) {
            if (menu.MenuState == menuState) {
                menu.enabled = true;
            } else menu.enabled = false;
        }

        currentMenuState = menuState;
        OnMenuChanged?.Invoke();
    }

    public Canvas FindMenu(MenuState menuState) {
        Canvas[] menus = FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        return menus.Single(menu => menu.gameObject.name.Equals(menuState.ToString() + "Menu"));
    }

    public void EnableMenu(MenuState menuState) {
        Array.Find(menus, menu => menu.MenuState == menuState).enabled = true;
        OnMenuChanged?.Invoke();
    }

    public void DisableMenu(MenuState menuState) {
        Array.Find(menus, menu => menu.MenuState == menuState).enabled = false;
        OnMenuChanged?.Invoke();
    }

}
