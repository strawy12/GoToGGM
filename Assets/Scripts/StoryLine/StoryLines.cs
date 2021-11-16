using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[System.Serializable]
public class StoryLine
{
    public string sceneName;
    public int sceneNum;
    public EStoryOrder[] storyOrder;
}




[CreateAssetMenu(fileName = "StoryLines", menuName = "Sprictable Object/StoryLines")]
public class StoryLines : ScriptableObject
{
    public StoryLine[] storyLines;
}

