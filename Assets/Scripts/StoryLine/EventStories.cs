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
    public EStatType increaseStatType;
    public int increaseStat;

    [Header("��� ������ ����")]
    public bool isSuccess;

    public bool usedEffect
    {
        get
        {
            return effectSettings.Length != 0;
        }
    }

    public EffectSetting[] effectSettings;

    [Header("���丮")]
    [TextArea(6, 10)] public string eventMainStory;

    public string[] eventFinalStory;
}

[System.Serializable]
public class EventStoryLine
{
    public string storyLineName;
    public EventStory[] eventStories;
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
