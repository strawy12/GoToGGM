using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
public class TitleManager : MonoBehaviour
{
    [Header("설정 패널 관련")]
    [SerializeField] private Slider effectSlider = null;
    [SerializeField] private Slider bgmSlider = null;
    [SerializeField] private Toggle effectMute = null;
    [SerializeField] private Toggle bgmMute = null;
    [SerializeField] private Slider fontSlider = null;
    [SerializeField] private Text testText = null;

    [Header("업적 패널 관련")]
    [SerializeField] GameObject panelObject = null;
    [SerializeField] Transform contentTransform = null;
    public List<AchievementBase> achievementPanels = new List<AchievementBase>();

    [SerializeField] MSGBoxScript msgBox = null;


    private void Start()
    {
        SetSettingPanel();
        CreatePanels();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            msgBox.ShowMSGBox();
        }
    }
    public void ShowPanels(RectTransform gameObject)
    {
        Vector2 Scale = new Vector2(gameObject.localScale.x, gameObject.localScale.y);
        gameObject.transform.localScale = Vector3.zero;
        gameObject.gameObject.SetActive(true);
        gameObject.transform.DOScale(Scale, 0.4f);
    }

    public void StartButtonOnClick()
    {
        SceneManager.LoadScene("Main");
    }

    public void DataReset()
    {
        DataManager.Inst.DataReset();
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
        //SoundManager.Inst.BGMVolume(value);
        DataManager.Inst.SaveToJson();
    }

    public void BGMMute(bool isMute)
    {
        //SoundManager.Inst.BGMMute(isMute);
        DataManager.Inst.SaveToJson();
    }

    public void EffectMute(bool isMute)
    {
        //SoundManager.Inst.EffectMute(isMute);
        DataManager.Inst.SaveToJson();
    }

    public void EffectVolume(float value)
    {
        //SoundManager.Inst.EffectVolume(value);
        DataManager.Inst.SaveToJson();
    }
    public void SetFontSize(float value)
    {
        Debug.Log(value);
        testText.fontSize = (int)value;
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
            //achievementPanels[i].isCleared = DataManager.Inst.CurrentPlayer.clears[i];
        }
        for (int i = 0; i < 15; i++)
        {
            if (achievementPanels[i].isCleared == true)
                achievementPanels[i].achievementPanel.Clear(i);
        }
    }
    #endregion
}
