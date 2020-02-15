using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class GameSceneManager : MonoBehaviour {
    private void Awake() {
        GAME = this;
        platformPoints = Misc.RandomGenerationPlatformPoints(Vector3.zero, platformPointsCount);

        recordScore = CommonVariables.recordScore;

        prevgameTime = PlayerPrefs.GetFloat(Constants.PlayerPrefs.PREVGAME_TIME, 0f);

        if (firstLaunch == 0) {
            firstLaunch = PlayerPrefs.GetInt(Constants.PlayerPrefs.FIRST_LAUNCH, 1);
            prevgameTime = 0f;
        }
        
        if (firstLaunch == 1) {
            PlayerPrefs.SetInt(Constants.PlayerPrefs.FIRST_LAUNCH, -1);
        }

        adDelayInSeconds = firstLaunch == 1 ?
            Constants.Advertisement.FIRST_LAUNCH_AD_DELAY_IN_SECONDS :
            Constants.Advertisement.DEFAULT_AD_DELAY_IN_SECONDS;
    }

    private void Start() {
        Advertisement.Initialize(Constants.Advertisement.GAME_ID, Constants.Advertisement.TEST_MODE);

        tapSound = GameObject.Find("Audio").GetComponent<AudioSource>();

        uiManager = new UIManager(GameObject.Find("UICanvas").gameObject);
        uiManager.Prepare();

        for (int i = 0; i < startPlatformCount; ++i) {
            GeneratePlatformsObjectByCubeIndex(i);
        }
    }

    private void FixedUpdate() { // 0.02 secx
        if (!isStarted) {
            return;
        }

        currentScore += scoreAdditionByFixedFrame;

        /*
         * 60 * scoreAdditionByFixedFrame / 0.02f - it is score per minute 
         */
         if (cubeSpeed < maxCubeSpeed) {
            cubeSpeed = startCubeSpeed * (1 + currentScore / (60 * scoreAdditionByFixedFrame / 0.02f));
        }
    }

    private void Update() {
        uiManager.Update();

        if (!isStarted && !uiManager.finishDialog.Active()) {

            if (!uiManager.pauseDialog.Active() && Input.GetMouseButton(0)) {
                pauseTime = Time.time;
                isStarted = true;
                uiManager.UnPrepare();
            }

            return;
        }

        uiManager.scoreText.text = ((int)(currentScore)).ToString();
        uiManager.bestText.text = ((int)(recordScore)).ToString();

        /*
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        uiManager.bestText.text = Mathf.Ceil(fps).ToString(); */
    }

    public void GeneratePlatformsObjectByCubeIndex(int pointIndex, float widthMultiplier=1.4f) {
        platformObjects.Add(
                Misc.GeneratePlatformObject(platformPoints[pointIndex], platformPoints[pointIndex + 1],
                                                      pointIndex == 0 ? 0 : platformObjects[pointIndex - 1].transform.localScale.x, // because margin = 1/2 * margin
                                                      Mathf.Max(Mathf.Min(cube.transform.localScale.x, cube.transform.localScale.z) * widthMultiplier, Mathf.Max(cube.transform.localScale.x, cube.transform.localScale.z) * 1.1f),
                                                      platformPrefab)
            );
    }

    public void OnGameOver() {
        isStarted = false;

        Misc.platformIndex = 0;

        if (currentScore > recordScore) {
            CommonVariables.recordScore = currentScore;
        }

        uiManager.finishDialog.Enable();
    }

    public static GameSceneManager GAME;

    private float scoreAdditionByFixedFrame = 0.2f;

    [Header("Set Dynamically")]
    public float startCubeSpeed = 2f;
    public float cubeSpeed = 0.01f;
    public float currentScore = 0;
    public float recordScore;
    public float maxCubeSpeed = 6f;

    public UIManager uiManager;

    public List<Vector3> platformPoints = new List<Vector3>();
    public List<GameObject> platformObjects = new List<GameObject>();

    public int startPlatformCount = 10;
    public int platformPointsCount = 1000;

    public float pauseTime;

    public Timer timer = new Timer();

    public AudioSource tapSound;

    [Header("Set in Inspector")]
    public Cube cube;
    public GameObject platformPrefab;
    public float deltaTime;

    public bool isStarted {
        get {
            return isStarted_;
        }
        set {
            if (value == true) {
                cube.rigidbody.velocity = prevCubeVelocity;
            } else {
                prevCubeVelocity = cube.rigidbody.velocity;
                cube.rigidbody.velocity = Vector3.zero;
            }
            prevCubeVelocity = cube.rigidbody.velocity;
            isStarted_ = value;
        }
    }

    private bool isStarted_ = false;
    private bool rotated = false;
    private Vector3 prevCubeVelocity;

    public static int firstLaunch = 0; // -1 = false, 1 = true, 0 - not stated
    public float adDelayInSeconds;

    public static float prevgameTime = 0f;
}
