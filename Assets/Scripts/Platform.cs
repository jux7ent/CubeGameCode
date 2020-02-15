using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform {
    public Platform(Vector3 startPoint, Vector3 endPoint, float width) {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.width = width;
    }

    public Vector3 startPoint;
    public Vector3 endPoint;
    public float width;
}
