using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventStory
{
    public int eventStoryID;

    [Header("���� ����")]
    public EStatType increaseStatType;
    public int increaseStat;

    [Header("��� ������ ����")]
    public int skipRouteCnt;
    public bool isSuccess;

    [Header("���丮")]
    [TextArea(7, 10)] public string eventMainStory;
    [TextArea(3, 5)] public string eventStory;
}

[CreateAssetMenu(fileName = "EventStories", menuName = "Sprictable Object/EventStories")]
public class EventStories : ScriptableObject
{
    public EventStory[] eventStories;
}