using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UserInterface : MonoBehaviour
{
    public static UserInterface Instance;
    [SerializeField]
    private GameObject StartScreen;

    private void OnEnable()
    {
    }

    private void Awake()
    {
        if(Instance== null)
        Instance = this;
    }

}
