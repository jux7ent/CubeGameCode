using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class PauseDialog : Dialog {
    public PauseDialog(GameObject gameObject) : base(gameObject) {
        soundBtn_ = gameObject.transform.Find("SoundButton").GetComponent<Button>();
        playBtn_ = gameObject.transform.Find("PlayButton").GetComponent<Button>();
        restartBtn_ = gameObject.transform.Find("RestartButton").GetComponent<Button>();

        soundBtn_.onClick.AddListener(OnSoundClick);
        playBtn_.onClick.AddListener(OnPlayClick);
        restartBtn_.onClick.AddListener(OnRestartClick);

        soundOnSprite_ = Resources.Load<Sprite>("Images/SoundOn");
        soundMuteSprite_ = Resources.Load<Sprite>("Images/SoundMute");

        soundState = PlayerPrefs.GetInt(Constants.PlayerPrefs.SOUND_STATE, 1) != 0;

        soundBtn_.gameObject.GetComponent<Image>().sprite =
                        soundState ? soundOnSprite_ : soundMuteSprite_;
    }

    override public void OnEnable() {
        return;
    }

    private void OnSoundClick() {
        soundState = !soundState;
        PlayerPrefs.SetInt(Constants.PlayerPrefs.SOUND_STATE, soundState ? 1 : 0);
        soundBtn_.gameObject.GetComponent<Image>().sprite =
                        soundState ? soundOnSprite_ : soundMuteSprite_;
    }

    private void OnPlayClick() {
        GameSceneManager.GAME.uiManager.UnPause(); // pause ui
        GameSceneManager.GAME.isStarted = true;
    }

    private void OnRestartClick() {
        if (Time.timeSinceLevelLoad + GameSceneManager.prevgameTime > GameSceneManager.GAME.adDelayInSeconds) {
            Advertisement.Show("rewardedVideo");

            GameSceneManager.prevgameTime = 0f;
            PlayerPrefs.SetFloat(Constants.PlayerPrefs.PREVGAME_TIME, 0f);
        } else {
            PlayerPrefs.SetFloat(Constants.PlayerPrefs.PREVGAME_TIME, Time.timeSinceLevelLoad + GameSceneManager.prevgameTime);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool soundState = true; // sound on

    private Button soundBtn_;
    private Button playBtn_;
    private Button restartBtn_;

    private Sprite soundOnSprite_;
    private Sprite soundMuteSprite_;
}
