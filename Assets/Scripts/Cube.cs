using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Cube : MonoBehaviour {
    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start() {
        ChangeMoveDirection();

        GetComponent<Renderer>().enabled = false;

        for (int i = 0; i < 3; ++i) {
            supportCubes.Add(Instantiate(supportCubePrefab).GetComponent<SupportCube>());
            supportCubes[i].name = "supportCube_" + i.ToString();
            supportCubeRenderers.Add(supportCubes[i].GetComponent<Renderer>());
        }

        startColor = supportCubeRenderers[0].material.color;
        endColor = Color.red;
    }

    private void FixedUpdate() {
        if (!GameSceneManager.GAME.isStarted) {
            return;
        }

        TransformSupportCubes();

        int activeCubes = 0;
        int activeCubeIndex = -1;
        for (int i = 0; i < 3; ++i) {
            if (supportCubes[i].active) {
                activeCubeIndex = i;
                ++activeCubes;
                supportCubeRenderers[i].enabled = true;
                supportCubeRenderers[i].material.color = Color.Lerp(startColor, endColor, 1 - Area());
            } else {
                supportCubeRenderers[i].enabled = false;
            }
        }

        if (activeCubes == 1) {
            transform.position = supportCubes[activeCubeIndex].transform.position;
            transform.localScale = supportCubes[activeCubeIndex].transform.localScale;
        }

        if (activeCubes == 0) {
            GameSceneManager.GAME.OnGameOver();
        }
    }

    private void TransformSupportCubes() {
        if (curSegmentIndex == 0) {
            supportCubes[2].active = false;

            supportCubes[0].TransformAs(
                                    Misc.GetRectIntersection(gameObject, GameSceneManager.GAME.platformObjects[0])
                                );

            supportCubes[1].TransformAs(
                                    Misc.GetRectIntersection(gameObject, GameSceneManager.GAME.platformObjects[1])
                                );
        } else {

            for (int i = 0; i < 3; ++i) {
                supportCubes[i].TransformAs(
                                            Misc.GetRectIntersection(gameObject, GameSceneManager.GAME.platformObjects[curSegmentIndex + i - 1])
                                        );
            }
        }
    }

    private void ChangeMoveDirection() {
        ++curSegmentIndex;
        SetDirection(curSegmentIndex);

        GameSceneManager.GAME.GeneratePlatformsObjectByCubeIndex(curSegmentIndex + GameSceneManager.GAME.startPlatformCount);
    }

    private void Update() {
        if (!GameSceneManager.GAME.isStarted) {
            return;
        }

        if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
            return;
        }

        if (Input.GetMouseButtonDown(0) && Time.time - GameSceneManager.GAME.pauseTime > 1f) {
            if (GameSceneManager.GAME.uiManager.pauseDialog.soundState) {
                GameSceneManager.GAME.tapSound.Play();
            }
            ChangeMoveDirection();
        }

        rigidbody.velocity = Vector3.Scale(
                moveDirection * GameSceneManager.GAME.cubeSpeed,
                (Vector3.forward + Vector3.right));
    }

    public float Area() {
        return Misc.CubeArea(gameObject);
    }

    private void SetDirection(int segmentIndex) {
        moveDirection =
            GameSceneManager.GAME.platformPoints[segmentIndex + 1] -
            GameSceneManager.GAME.platformPoints[segmentIndex];
        moveDirection.Normalize();

        posY =
            GameSceneManager.GAME.platformPoints[segmentIndex].y + transform.localScale.y / 2f;
    }

    public int curSegmentIndex = -1; // init = -1
    public float posY = 0;
    public Vector3 moveDirection = new Vector3();

    public List<SupportCube> supportCubes = new List<SupportCube>();
    public List<Renderer> supportCubeRenderers = new List<Renderer>();

    public Rigidbody rigidbody;

    public Color startColor;
    public Color endColor;

    [Header("Set in Ispector")]
    public GameObject supportCubePrefab;
}
