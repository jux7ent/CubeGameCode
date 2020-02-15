using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonVariables {
    public static void Init(GameObject cube) {
        CommonVariables.cube = cube;
    }

    public static float recordScore {
        get {
            return PlayerPrefs.GetFloat(Constants.PlayerPrefs.RECORD_SCORE, 0f);
        }
        set {
            PlayerPrefs.SetFloat(Constants.PlayerPrefs.RECORD_SCORE, value);
        }

    }

    public static GameObject cube;
}
