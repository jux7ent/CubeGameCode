using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dialog {
    public Dialog(GameObject gameObject) {
        this.gameObject = gameObject;
    }

    public void Enable() {
        gameObject.SetActive(true);
        OnEnable();
    }

    public void Disable() {
        gameObject.SetActive(false);
    }

    public bool Active() {
        return gameObject.active;
    }

    abstract public void OnEnable();

    public GameObject gameObject;
}
