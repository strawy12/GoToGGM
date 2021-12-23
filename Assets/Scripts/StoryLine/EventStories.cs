using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventStory
{
    public int eventStoryID;

    [Header("���� �ð�")]
    public int increaseTime;

    [Header("���� ����")]
    public IncreaseStats[] increaseStats;

    [Header("��� ������ ����")]
    public bool isSuccess;

    public bool usedEffect
    {
        get
        {
            return effectSettings != null && effectSettings.Length != 0;
        }
    }

    public EffectSetting[] effectSettings;

    [Header("���丮")]
    [TextArea(6, 10)] public string eventMainStory;

    public string[] eventFinalStory;

    public bool ExistIncreaseStats { get { return increaseStats.Length != 0; } }
    public bool ExistIncreaseStat_ArrivalTime
    { 
        get 
        { 
            foreach(var increaseStat in increaseStats)
            {
                if(increaseStat.increaseStatType == EStatType.ArrivalTime)
                {
                    return true;
                }
            }
            return false; 
        } 
    }
}

[System.Serializable]
public class EventStoryLine
{
    public string storyLineName;
    public EventStory[] eventStories;
}

[System.Serializable]
public class IncreaseStats
{
    public EStatType increaseStatType;
    public int increaseStat;
}

[System.Serializable]
public class EventScenario
{
    public string scenarioName;
    public EventStoryLine[] eventStoryLines;
}

[CreateAssetMenu(fileName = "EventStories", menuName = "Sprictable Object/EventStories")]
public class EventStories : ScriptableObject
{
    public EventScenario[] eventScenarios;
}
