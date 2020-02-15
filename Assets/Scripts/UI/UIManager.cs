using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager {
    public UIManager(GameObject canvasGO) {
        canvasGO_ = canvasGO;

        textTapToStart = canvasGO.transform.Find("TapToStart").GetComponent<Text>();
        bestText = canvasGO.transform.Find("FpsText").GetComponent<Text>();
        scoreText = canvasGO.transform.Find("ScoreText").GetComponent<Text>();
        pauseButton = canvasGO.transform.Find("PauseButton").GetComponent<Button>();

        finishDialog = new FinishDialog(canvasGO.transform.Find("FinishingDialog").gameObject);
        pauseDialog = new PauseDialog(canvasGO.transform.Find("PauseDialog").gameObject);

        scaleParameter = minMaxScaleParameter[1];

        pauseButton.onClick.AddListener(OnPauseClick);

        ChangeScaleParameter();

        finishDialog.Disable();
        pauseDialog.Disable();
        Prepare();
        scoreText.gameObject.SetActive(false);
        bestText.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);
    }

    public void Update() {
        ScaleStartText();
    }

    public void OnPauseClick() {
        Pause(); // pause ui
        GameSceneManager.GAME.isStarted = false;
    }

    public void ScaleStartText() {
        if (Mathf.Abs(textTapToStart.transform.localScale.x - scaleParameter) < Constants.eps * 100f) {
            ChangeScaleParameter();
        }

        textTapToStart.transform.localScale =
                Vector3.Lerp(textTapToStart.transform.localScale,
                    Vector3.forward + Vector3.up * scaleParameter + Vector3.right * scaleParameter,
                    2.5f * Time.deltaTime);     
    }

    public void Prepare() {
        textTapToStart.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(false);
        bestText.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);
    }

    public void UnPrepare() {
        textTapToStart.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        bestText.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(true);
    }

    public void Pause() {
        textTapToStart.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        bestText.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        finishDialog.Disable();
        pauseDialog.Enable();
    }

    public void UnPause() {
        Time.timeScale = 1;
        textTapToStart.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        bestText.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(true);
        finishDialog.Disable();
        pauseDialog.Disable();
    }

    private void ChangeScaleParameter() {
        scaleParameter =
            scaleParameter == minMaxScaleParameter[0] ?
            minMaxScaleParameter[1] :
            minMaxScaleParameter[0];
    }

    private void print(string str) {
        MonoBehaviour.print(str);
    }

    private float prevTime = Time.time;

    /* text for tap to start */
    public Text textTapToStart;
    private Vector2 minMaxScaleParameter = new Vector2(0.5f, 1.5f);
    public float scaleParameter;

    public Text bestText;
    public Text scoreText;

    public Button pauseButton;

    public FinishDialog finishDialog;
    public PauseDialog pauseDialog;

    private GameObject canvasGO_;
}
