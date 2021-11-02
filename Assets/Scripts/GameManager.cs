using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESelectType
{
    Normal,
    Gread,
    Special
}

public enum EStatType
{
    None,
    Sencetive,
    Knowledge,
    Wit
}

public enum EStoryOrder
{
    Prologue,
    Bus,
    Subway
}

//public enum btnState { Special, Normal, Good }

public class GameManager : MonoSingleTon<GameManager>
{

    private UIManager uiManager;
    private StoryManager storyManager;
    public UIManager UI { get { return uiManager; } }
    public StoryManager Story { get { return storyManager; } }
    public int stat;
    void Awake()
    {
        uiManager = GetComponent<UIManager>();
        storyManager = GetComponent<StoryManager>();
    }

    public void SetPlayerStat(int stat/*���� Ÿ��*/)
    {
        //player.stat[����Ÿ��] += stat;
    }
}
