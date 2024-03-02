using System;
using UnityEngine;

public abstract class Menu : MonoBehaviour {

    [SerializeField] private MenuState menuState;

    protected virtual void Awake() {
        MenuManager.Instance.OnMenuChanged += OnMenuChanged;
    }

    protected virtual void OnDestroy() {
        MenuManager.Instance.OnMenuChanged -= OnMenuChanged;
    }

    protected virtual void Start() => OnMenuChanged();

    private void OnMenuChanged() {
        bool enabled = Array.Find(MenuManager.Instance.Menus, menu => menuState == menu.MenuState).enabled;
        gameObject.SetActive(enabled);
    }

}