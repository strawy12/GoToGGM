using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoSingleTon<DataManager>
{
    [SerializeField] private int defaultFontsize = 49;
    [SerializeField] private float defaultSound = 0.5f;
    [SerializeField] private Player player;
    public Player CurrentPlayer { get { return player; } }
    string SAVE_PATH = "";
    string SAVE_FILE = "/SaveFile.txt";
    private void Awake()
    {
        SAVE_PATH = Application.persistentDataPath + "/Save";
        DontDestroyOnLoad(gameObject);
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }
        LoadFromJson();
    }
    private void LoadFromJson()
    {
        if (File.Exists(SAVE_PATH + SAVE_FILE))
        {
            string stringJson = File.ReadAllText(SAVE_PATH + SAVE_FILE);
            player = JsonUtility.FromJson<Player>(stringJson);
        }
        else
        {
            player = new Player("고등학생", 0, 0, 0, defaultSound, defaultSound, false, false, defaultFontsize);
            SaveToJson();
        }
    }
    public void SaveToJson()
    {
        string stringJson = JsonUtility.ToJson(player, true);
        File.WriteAllText(SAVE_PATH + SAVE_FILE, stringJson, System.Text.Encoding.UTF8);
    }
    public void ResetData()
    {
        player = new Player("고등학생", 0, 0, 0, defaultSound, defaultSound, false, false, defaultFontsize);
        SaveToJson();
        Application.Quit();
    }
}