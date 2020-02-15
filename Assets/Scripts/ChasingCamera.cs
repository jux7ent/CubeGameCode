using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingCamera : MonoBehaviour {
    private void Awake() {
        camera_ = GetComponent<Camera>();
        SetResolution();
        SetRandomRotation();

        scoreForRatation_ = Random.Range(150, 250);

        startColor = camera_.backgroundColor;
        endColor = Color.black;
    }

    private Vector3 velocity = Vector3.zero;

    public void FixedUpdate() {
        if (!GameSceneManager.GAME.isStarted) {
            return;
        }

        ChangeCameraSize();

        MoveCameraToPosition(GameSceneManager.GAME.cube.transform.position);

        camera_.backgroundColor = Color.Lerp(startColor, endColor, Mathf.Sin(GameSceneManager.GAME.currentScore / (scoreForMaxColor * 10)));
        //print(1 - ((scoreForMaxColor - (GameSceneManager.GAME.currentScore % scoreForMaxColor)) / scoreForMaxColor));
      //  print(Mathf.Sin(GameSceneManager.GAME.currentScore / (scoreForMaxColor * 10)));
        if (GameSceneManager.GAME.currentScore > scoreForRatation_) {
            RotateCamera();
        }
    }

    private void MoveCameraToPosition(Vector3 pos) {
        transform.position =
            Vector3.Scale(Vector3.forward + Vector3.right,
                          Vector3.SmoothDamp(transform.position, pos, ref velocity, maxCameraSmoothCoeff * (1f / 2f) + GameSceneManager.GAME.cube.Area() * maxCameraSmoothCoeff / 2f)
                          ) +
            Vector3.up * transform.position.y;
    }

    private void RotateCamera() {
        transform.rotation = 
            Quaternion.Slerp(
                transform.rotation,
                Quaternion.Euler(Vector3.Scale(Vector3.forward + Vector3.right, transform.rotation.eulerAngles) + rotateAngles),
                0.005f);
    }

    private void ChangeCameraSize() {
        camera_.orthographicSize = 
            Mathf.Lerp(camera_.orthographicSize, maxCameraSize * (2f / 3f) + GameSceneManager.GAME.cube.Area() * maxCameraSize / 3f, 0.08f);
    }

    private void SetRandomRotation() {
        rotateAngles.y = Random.Range(-minMaxRotationAngle_, minMaxRotationAngle_);
        secondsBetweenRotation_ = Random.Range(30, 50);
        Invoke("SetRandomRotation", secondsBetweenRotation_);
    }

    private void SetResolution() {
        setted_width_ = Screen.width;
        setted_height_ = Screen.height;
        float width_size = (float)(w_amount * Screen.height / Screen.width * 0.5);
        float height_size = (float)(h_amount * Screen.width / Screen.height * 0.5) * ((float)Screen.height / Screen.width);
        camera_.orthographicSize = Mathf.Max(height_size, width_size);

        maxCameraSize = camera_.orthographicSize;
    }


    private Camera camera_;

    private float w_amount = 10f; //Minimum units horizontally
    private float h_amount = 5f; //Minimum units vertically
    private float setted_width_, setted_height_;

    private Vector3 rotateAngles = new Vector3();

    private float scoreForRatation_ = 10f;
    private float minMaxRotationAngle_ = 70f;
    private float secondsBetweenRotation_ = 30f;

    private float maxCameraSmoothCoeff = 0.8f;
    private float maxCameraSize;

    private Color startColor;
    private Color endColor;
    private float scoreForMaxColor = 100f;
}
