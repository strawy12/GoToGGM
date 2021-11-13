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
}
public class AchievementManager : MonoBehaviour
{
    [SerializeField] GameObject panelObject = null;
    [SerializeField] Transform contentTransform = null;
    [SerializeField] ClearNoticeScript clearNotice = null;
    public List<AchievementBase> achievementPanels = new List<AchievementBase>();
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
    public void Test()//실험용 클리어함수
    {
        if (achievementPanels[0].isCleared) return;
        achievementPanels[0].isCleared = true;
        AchievementPanel component = contentTransform.GetChild(0).GetComponent<AchievementPanel>();
        component.Clear();
        contentTransform.GetChild(0).SetSiblingIndex(contentTransform.GetChild(0).GetSiblingIndex() + contentTransform.GetChild(0).childCount);
        clearNotice.ShowNotice(achievementPanels[0].title);
    }
}