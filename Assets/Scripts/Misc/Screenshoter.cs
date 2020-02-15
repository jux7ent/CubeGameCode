using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshoter : MonoBehaviour {
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            ScreenCapture.CaptureScreenshot("SomeLevel" + Random.Range(0, 10000) + ".png");
        }
    }
}
