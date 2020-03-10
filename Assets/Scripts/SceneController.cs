using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
