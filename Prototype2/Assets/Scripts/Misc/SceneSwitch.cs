using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public void Switch(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }

    public void SwitchWithReturn(int sceneIndex, Vector3 returnPosition) {
        Blackboard.PlayerInfo.position = returnPosition;
        SceneManager.LoadScene(sceneIndex);
    }

    public void Reload() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit() {
        Application.Quit();
    }
}
