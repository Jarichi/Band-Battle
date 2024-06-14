using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerJoinScreen : MonoBehaviour
{
    [SerializeField]
    private string levelSceneName;
    [SerializeField]
    private int minimumPlayerCount = 1;
    [SerializeField]
    private int maximumPlayerCount = 4;
    [SerializeField]
    private GameObject[] playerSelectionFields;
    [Header("GUI Options")]
    private Color selectedColor = Color.white;

    void Update()
    {
        var players = PlayerList.Get();
        for (int i = 0; i < players.Count; i++)
        {
            playerSelectionFields[i].SetActive(true);
            var playerText = playerSelectionFields[i].GetComponentInChildren<TextMeshProUGUI>();
            playerText.color = selectedColor;
            var readyText = playerText.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            readyText.text = "press A";
            if (players[i].isReady)
            {
                readyText.text = "ready!";
                readyText.color = Color.green;
            }
            else readyText.color = selectedColor;
        }

        if (players.Count < minimumPlayerCount) return;

        if (players.All(p => p.isReady))
        {
            PlayerList.Instance.inputManager.DisableJoining();
            SceneManager.LoadScene(levelSceneName);
        }
    }
}
