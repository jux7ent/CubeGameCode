using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer {
    public float GetSetTime() {
        float result = curTime;
        curTime = Time.time;
        return result;
    }

    public float curTime;
}
