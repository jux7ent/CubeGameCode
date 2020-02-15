using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportCube : MonoBehaviour {
    private void Start() {
        chasedCube = GameSceneManager.GAME.cube;
    }

    public void TransformAs(Vector3[] points) {
        active = Misc.TransformCubeFromEdges(gameObject, points);
    }

    public float Area() {
        if (!active)
            return 0f;

        return Misc.CubeArea(gameObject);
    }

    public bool active = true;
    private Cube chasedCube;
}
