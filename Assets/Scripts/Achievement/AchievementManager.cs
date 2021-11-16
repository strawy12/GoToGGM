using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AchievementBase
{
    public string title;
    public string explanation;
    public bool isCleared;
    public int ID;
}
public class AchievementManager : MonoBehaviour
{
    [SerializeField] GameObject panelObject = null;
    [SerializeField] Transform contentTransform = null;
    [SerializeField] ClearNoticeScript clearNotice = null;
    public List<AchievementBase> achievementPanels = new List<AchievementBase>();
    private int testID = 0;
    private void Start()
    {
        foreach (AchievementBase achievement in achievementPanels)
        {
            GameObject newObject = Instantiate(panelObject, contentTransform);
            AchievementPanel newComponent = newObject.GetComponent<AchievementPanel>();
            newComponent.SetValue(achievement.title, achievement.explanation);
            newObject.SetActive(true);
        }
    }
    private void Clear(int ID)
    {
        if (achievementPanels[ID].isCleared) return;
        achievementPanels[ID].isCleared = true;
        AchievementPanel component = contentTransform.GetChild(ID).GetComponent<AchievementPanel>();
        component.Clear();
        contentTransform.GetChild(0).SetSiblingIndex(contentTransform.GetChild(ID).GetSiblingIndex() + contentTransform.GetChild(0).childCount);
        clearNotice.ShowNotice(achievementPanels[ID].title);
    }
    public void Test()//실험용 클리어함수
    {
        Clear(testID);
        testID++;
    }
}