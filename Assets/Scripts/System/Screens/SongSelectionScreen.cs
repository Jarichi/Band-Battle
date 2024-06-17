using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using UnityEngine.Windows;

public class SongSelectionScreen : Screen
{
    [SerializeField]
    private GameObject songOptionPrefab;

    [Header("GUI Settings")]
    [SerializeField]
    [Range(0f, 500f)]
    private float buttonMarginBottom;

    private Player bandLeader;
    public UnityEvent<Song> songSelectEvent;


    private void SelectSong(Song song)
    {
        var index = EventSystem.current.currentSelectedGameObject.GetComponent<IndexableButton>().Index;
        song.DeserializeFile();
        PlayerList.Get().ForEach(p => p.EnableControls());
        EventSystem.current.SetSelectedGameObject(null);
        songSelectEvent.Invoke(song);
        HideUI();
    }


    public void ShowUI(Song[] songs)
    {
        ShowUI();


        var players = PlayerList.Get();
        players.ForEach(p => {
            p.SwitchActionMap("Player");
            p.DisableAllControls();
        });

        bandLeader = players.FirstOrDefault(p => p.data.isBandLeader);
        if (bandLeader == null)
        {
            var index = UnityEngine.Random.Range(0, players.Count);
            bandLeader = players[index];
            bandLeader.data.isBandLeader = true;
        }
        var y = 130f;
        int i = 0;

        GameObject firstButton = null;
        foreach (var song in songs)
        {
            var obj = Instantiate(songOptionPrefab, transform);
            var t = obj.GetComponent<RectTransform>();
            var pos = t.localPosition;
            pos.y = y;
            t.localPosition = pos;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = song.title;
            y -= buttonMarginBottom;

            var button = obj.GetComponent<Button>();
            button.onClick.AddListener(() => {                           
                SelectSong(song);
                button.onClick.RemoveAllListeners();
            });
            obj.GetComponent<IndexableButton>().SetIndex(i);

            if (i == 0) firstButton = button.gameObject;
            i++;

        }

        GetComponentInChildren<TextMeshProUGUI>().text = GetComponentInChildren<TextMeshProUGUI>().text.Replace("<n>", bandLeader.Index.ToString());
        EventSystem.current.GetComponent<InputSystemUIInputModule>().actionsAsset = bandLeader.Input.actions;
        EventSystem.current.SetSelectedGameObject(firstButton);


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
