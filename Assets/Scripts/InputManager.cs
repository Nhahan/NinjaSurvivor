using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    TouchControls touchControls;

    void Awake() {
        touchControls = new TouchControls();
    }

    void OnEnable() {
        touchControls.Enable();
    }

    void OnDisable() {
        touchControls.Disable();
    }

    void Start() {
        touchControls.Touch.TouchPress.started += context => StartTouch(context);
        touchControls.Touch.TouchPress.cancelled += context => EndTouch(context);
    }
}
