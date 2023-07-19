using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    public Button StartButton;
    public Button CreditsButton;
    public Button RecordsButton;
    public Button ExitButton;
    void Start()
    {
        StartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        });

        CreditsButton.onClick.AddListener(() =>
        {
            Instantiate(Resources.Load<CreditsPanel>("Prefabs/Panels/CreditsPanel"), transform.parent);
        });

        RecordsButton.onClick.AddListener(() =>
        {
            Instantiate(Resources.Load<RecordsPanel>("Prefabs/Panels/RecordsPanel"), transform.parent);
        });

        ExitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
