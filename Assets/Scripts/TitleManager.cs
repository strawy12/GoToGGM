using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
public class TitleManager : MonoBehaviour
{
    [SerializeField] private Slider effectSlider = null;
    [SerializeField] private Slider bgmSlider = null;
    [SerializeField] private Toggle effectMute = null;
    [SerializeField] private Toggle bgmMute = null;
    [SerializeField] private Slider fontSlider = null;

    [SerializeField] GameObject panelObject = null;
    [SerializeField] Transform contentTransform = null;
    public List<AchievementBase> achievementPanels = new List<AchievementBase>();


    private void Start()
    {
        SetSettingPanel();
        CreatePanels();
    }

    public void ShowPanels(GameObject gameObject)
    {
        gameObject.transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
        gameObject.transform.DOScale(Vector3.one, 0.4f);
    }

    public void StartButtonOnClick()
    {
        SceneManager.LoadScene("Main");
    }

    #region SettingPanel
    public void SetSettingPanel()
    {
        effectSlider.value = DataManager.Inst.CurrentPlayer.effectVolume;
        bgmSlider.value = DataManager.Inst.CurrentPlayer.bgmVolume;
        fontSlider.value = DataManager.Inst.CurrentPlayer.fontSize;
        if (DataManager.Inst.CurrentPlayer.bgmMute)
            bgmMute.isOn = true;
        if (DataManager.Inst.CurrentPlayer.effectMute)
            effectMute.isOn = true;
    }
    public void BGMVolume(float value)
    {
        DataManager.Inst.CurrentPlayer.bgmVolume = value;
        DataManager.Inst.SaveToJson();
    }

    public void BGMMute(bool isMute)
    {
        DataManager.Inst.CurrentPlayer.bgmMute = isMute;
        DataManager.Inst.SaveToJson();
    }

    public void EffectMute(bool isMute)
    {
        DataManager.Inst.CurrentPlayer.effectMute = isMute;
        DataManager.Inst.SaveToJson();
    }

    public void EffectVolume(float value)
    {
        DataManager.Inst.CurrentPlayer.effectVolume = value;
        DataManager.Inst.SaveToJson();
    }
    public void SetFontSize(float value)
    {
        DataManager.Inst.CurrentPlayer.fontSize = (int)value;
        DataManager.Inst.SaveToJson();
    }
    #endregion
    #region AchivementPanel
    private void CreatePanels()
    {
        for (int i = 0; i < 15; i++)
        {
            GameObject newObject = Instantiate(panelObject, contentTransform);
            achievementPanels[i].achievementPanel = newObject.GetComponent<AchievementPanel>();
            achievementPanels[i].achievementPanel.SetValue(achievementPanels[i].title, achievementPanels[i].explanation);
            newObject.SetActive(true);

            achievementPanels[i].isCleared = DataManager.Inst.CurrentPlayer.clears[i];
        }
        for (int i = 0; i < 15; i++)
        {
            if (achievementPanels[i].isCleared == true)
                achievementPanels[i].achievementPanel.Clear(i);
        }
    }
    #endregion
}
