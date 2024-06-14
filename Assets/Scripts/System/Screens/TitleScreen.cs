using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    [SerializeField]
    private string targetSceneName;

    [Header("GUI")]
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Sprite startButtonPressSprite;
    [SerializeField]
    private Button quitButton;

    public void StartPressed()
    {
        startButton.image.sprite = startButtonPressSprite;
        SceneManager.LoadScene(targetSceneName);
        startButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
    }

    public void QuitPressed()
    {
        Application.Quit();
    }
}
