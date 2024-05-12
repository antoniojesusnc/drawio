using UnityEngine;
using UnityEngine.SceneManagement;

public class CommonScripts : MonoBehaviour {

    public void doLoadBallDemoScene() {
        SceneManager.LoadScene("BallDemo");
    }

    public void doLoadHapticDemoScene() {
        SceneManager.LoadScene("HapticDemo");
    }

}