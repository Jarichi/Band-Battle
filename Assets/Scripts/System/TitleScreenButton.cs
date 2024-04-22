using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenButton : MonoBehaviour
{
    private Button button;
    [SerializeField]
    private string targetSceneName;
    void Start()
    {
        button= GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(targetSceneName);
        });
    }
}
