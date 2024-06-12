using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField]
    private string levelSceneName;
    [SerializeField]
    private int minimumPlayerCount = 1;
    [SerializeField]
    private int maximumPlayerCount = 4;
    void Update()
    {
        var players = PlayerList.Get();
        if (players.Count < minimumPlayerCount) return;
        if (players.All(p => p.isReady))
        {
            SceneManager.LoadScene(levelSceneName);
        }
    }
}
