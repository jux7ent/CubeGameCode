using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class FinishDialog : Dialog {
    public FinishDialog(GameObject gameObject) : base(gameObject) {
        scoreTxt_ = gameObject.transform.Find("ScoreText").GetComponent<Text>();
        bestTxt_ = gameObject.transform.Find("BestScoreText").GetComponent<Text>();
        playBtn_ = gameObject.transform.Find("PlayBtn").GetComponent<Button>();

        playBtn_.onClick.AddListener(OnPlayClick);
    }

    override public void OnEnable() {
        int score = (int)(GameSceneManager.GAME.currentScore);
        int bestScore = Mathf.Max((int)GameSceneManager.GAME.recordScore, score);
        scoreTxt_.text = score.ToString();
        bestTxt_.text = bestScore.ToString();
    }

    private void OnPlayClick() {
        if (Time.timeSinceLevelLoad + GameSceneManager.prevgameTime > GameSceneManager.GAME.adDelayInSeconds) {
            Advertisement.Show("rewardedVideo");

            GameSceneManager.prevgameTime = 0f;
            PlayerPrefs.SetFloat(Constants.PlayerPrefs.PREVGAME_TIME, 0f);
        //    MonoBehaviour.print("inside time: " + Time.timeSinceLevelLoad + " prevgameTime: " + GameSceneManager.prevgameTime);
        } else {
          //  MonoBehaviour.print("outside time: " + Time.timeSinceLevelLoad + " prevgameTime: " + GameSceneManager.prevgameTime);
            PlayerPrefs.SetFloat(Constants.PlayerPrefs.PREVGAME_TIME, Time.timeSinceLevelLoad + GameSceneManager.prevgameTime);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private Text scoreTxt_;
    private Text bestTxt_;
    private Button playBtn_;
}
