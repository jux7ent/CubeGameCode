using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemManager {
    public ParticleSystemManager(ParticleSystem ps) {
        particleSystem_ = ps;
        Disable();
    }

    public void SetOnPosition(Vector3 moveDirection, GameObject cubeGO, float startSpeed = 1f) {
        if (Active()) {
            return;
        }

        Vector3 psPos = cubeGO.transform.position;
        float edgeLength = 0f;

        if (moveDirection == Vector3.forward) {
            psPos.z += cubeGO.transform.localScale.z / 2f;
            edgeLength = cubeGO.transform.localScale.x / 2f;
            particleSystem_.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        } else if (moveDirection == Vector3.right) {
            psPos.x += cubeGO.transform.localScale.x / 2f;
            edgeLength = cubeGO.transform.localScale.z / 2f;
            particleSystem_.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
        } else if (moveDirection == Vector3.back) {
            psPos.z -= cubeGO.transform.localScale.z / 2f;
            edgeLength = cubeGO.transform.localScale.x / 2f;
            particleSystem_.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        } else if (moveDirection == Vector3.left) {
            psPos.x -= cubeGO.transform.localScale.x / 2f;
            edgeLength = cubeGO.transform.localScale.z / 2f;
            particleSystem_.transform.rotation = Quaternion.Euler(90f, -90f, 0f);
        }

        particleSystem_.transform.position = psPos;

        var shape = particleSystem_.shape;
        shape.radius = edgeLength;

        Enable(startSpeed);
    }

    public void Disable() {
        particleSystem_.gameObject.SetActive(false);
    }

    public void Diactivate() {
        particleSystem_.enableEmission = false;
    }

    public void Enable(float startSpeed = 1f) {
        particleSystem_.enableEmission = true;
        particleSystem_.startSpeed = startSpeed;
        particleSystem_.gameObject.SetActive(true);
        GameSceneManager.GAME.StartCoroutine(DisableCoroutine());
    }

    IEnumerator DisableCoroutine() {
        yield return new WaitForSeconds(1.5f);
        Disable();
    }

    public bool Active() {
        return particleSystem_.gameObject.active;
    }

    private ParticleSystem particleSystem_;
}
