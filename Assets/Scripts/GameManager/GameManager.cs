using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private string SAVE_PATH = "";
    private string SAVE_FILE = "/SaveFile.txt";
    public Player player { get; private set; }
    private void Awake()
    {
        SAVE_PATH = Application.persistentDataPath;
        if (Directory.Exists(SAVE_PATH) == false)
        {
            Directory.CreateDirectory(SAVE_PATH);
        }
        LoadFromJson();
    }

    private void LoadFromJson()
    {
        if(File.Exists(string.Concat(SAVE_PATH, SAVE_FILE)))
        {
            string stringJson = File.ReadAllText(string.Concat(SAVE_PATH, SAVE_FILE));
            player = JsonUtility.FromJson<Player>(stringJson);
        }
        else
        {
            player = new Player();
            player.stat_Sencetive = 0;
            player.stat_Knowledge = 0;
            player.stat_Wit = 0;
            SaveToJson();
        }
    }

    private void SaveToJson()
    {
        string stringJson = JsonUtility.ToJson(player, true);
        File.WriteAllText(string.Concat(SAVE_PATH, SAVE_FILE), stringJson, System.Text.Encoding.UTF8);
    }
}
