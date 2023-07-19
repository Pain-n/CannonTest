using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosePanel : MonoBehaviour
{
    public Button RestartButton;
    public Button ToMainMenuButton;

    void Start()
    {
        RestartButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        });

        ToMainMenuButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
        });
    }
}
