using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants {

    // for random generation
    public static float[] availableHeights = { 2.5f, 3f, 3.5f, 4f, 4.5f, 5f, 5.5f };
    public static float[] availableAngles = { -90f, 90f };

    public static float eps = 1e-3f;

    public static bool DEBUG = false;

    public static class PlayerPrefs {
        public static string RECORD_SCORE = "##RECORD_SCORE##";
        public static string SOUND_STATE = "##SOUND_STATE##";
        public static string PREVGAME_TIME = "##PREVGAME_TIME##";
        public static string FIRST_LAUNCH = "##FIRST_LAUNCH##";
    }

    public static class Advertisement {
        public static string GAME_ID = "2190564";
        public static bool TEST_MODE = true;

        public static float DEFAULT_AD_DELAY_IN_SECONDS = 120f;
        public static float FIRST_LAUNCH_AD_DELAY_IN_SECONDS = 240f;
    }
}
