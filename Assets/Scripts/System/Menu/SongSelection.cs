using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Windows;

public class SongSelection : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 500f)]
    private float buttonMarginBottom;
    [SerializeField]
    private GameObject songOptionPrefab;
    [SerializeField]
    private Song[] songs;

    private MenuInputController input;

    private readonly List<GameObject> buttons = new();

    void Start()
    {
        songs.ToList().ForEach(s => s.DeserializeFile());
        var y = 130f;
        foreach (var song in songs)
        {
            var obj = Instantiate(songOptionPrefab, transform);
            var t = obj.GetComponent<RectTransform>();
            var pos = t.localPosition;
            pos.y = y;
            t.localPosition = pos; 
            songOptionPrefab.GetComponentInChildren<TextMeshProUGUI>().text = song.title;
            y -= buttonMarginBottom;
            buttons.Add(obj);
        }

        EventSystem.current.SetSelectedGameObject(buttons[0]);
    }

    private void OnEnable()
    {
        input = PlayerList.Get().First(p => p.data.isBandLeader).GetComponent<MenuInputController>();
        input.ConfirmPressed += (_ => { OnConfirmChoice(); });;
    }

    private void OnDisable()
    {
        input.ConfirmPressed = null;
    }

    public void OnConfirmChoice()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
    }
}


[Serializable]
public class Song
{
    [SerializeField]
    private string beatmapFilePath;
    public EventReference fmodEvent;
    public Beatmap beatmap;
    public string title;
    internal void DeserializeFile()
    {
        beatmap = JsonUtility.FromJson<Beatmap>(System.IO.File.ReadAllText(beatmapFilePath));
    }
}
