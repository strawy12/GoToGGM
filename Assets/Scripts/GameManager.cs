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

public class GameManager : MonoSingleTon<GameManager>
{
    public enum btnState { Special, Normal, Good }
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

    public void SetPlayerStat(int stat/*Ω∫≈» ≈∏¿‘*/)
    {
        //player.stat[Ω∫≈»≈∏¿‘] += stat;
    }
}
