using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserInterface : MonoBehaviour
{
    public static UserInterface Instance;
    [SerializeField]
    private GameObject StartScreen;
    private void Start()
    {
        Instance = this;
    }

    public void ToggleStartScreen()
    {
        StartScreen.SetActive(!StartScreen.activeSelf);
    }
}
