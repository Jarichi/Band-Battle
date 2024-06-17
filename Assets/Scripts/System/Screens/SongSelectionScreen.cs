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
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using UnityEngine.Windows;

public class SongSelectionScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject songOptionPrefab;
    [SerializeField]
    private Song[] songs;
    [Header("GUI Settings")]
    [SerializeField]
    [Range(0f, 500f)]
    private float buttonMarginBottom;

    private Player bandLeader;

    private readonly List<GameObject> buttons = new();
    private void OnEnable()
    {

        //retrieve song array from Game


        var players = PlayerList.Get();
        players.ForEach(p => {
            p.SwitchActionMap("Player");
            p.DisableAllControls();
        });

        bandLeader = players.FirstOrDefault(p => p.data.isBandLeader);
        if (bandLeader == null)
        {
            var index  = UnityEngine.Random.Range(0, players.Count);
            bandLeader = players[index];
            bandLeader.data.isBandLeader = true;
        }
        var y = 130f;
        int i = 0;
        foreach (var song in songs)
        {
            var obj = Instantiate(songOptionPrefab, transform);
            var t = obj.GetComponent<RectTransform>();
            var pos = t.localPosition;
            pos.y = y;
            t.localPosition = pos; 
            obj.GetComponentInChildren<TextMeshProUGUI>().text = song.title;
            y -= buttonMarginBottom;
            obj.GetComponent<Button>().onClick.AddListener(() => SelectSong());
            obj.GetComponent<IndexableButton>().SetIndex(i);
            buttons.Add(obj);
            i++;
        }
        GetComponentInChildren<TextMeshProUGUI>().text = GetComponentInChildren<TextMeshProUGUI>().text.Replace("<n>", bandLeader.Index.ToString());
        EventSystem.current.GetComponent<InputSystemUIInputModule>().actionsAsset = bandLeader.Input.actions;
        EventSystem.current.SetSelectedGameObject(buttons[0]);
    }

    public void SelectSong()
    {
        var index = EventSystem.current.currentSelectedGameObject.GetComponent<IndexableButton>().Index;
        var song = songs[index];
        song.DeserializeFile();
        PlayerList.Get().ForEach(p => p.EnableControls());
        buttons.ForEach(btn => btn.GetComponent<Button>().onClick.RemoveAllListeners());
        EventSystem.current.SetSelectedGameObject(null);
        Game.Instance.OnSongSelect(song);
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
