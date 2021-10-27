using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class Scenario
{
    public string sceneName;
    public int sceneNum;
    public int[] storyOrder;

}




[CreateAssetMenu(fileName = "StoryLine", menuName = "Sprictable Object/StoryLine")]
public class StoryLine : ScriptableObject
{
    public string storyName;
    public Scenario[] story;
}

