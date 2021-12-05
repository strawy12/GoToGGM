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
    [SerializeField] private HelpBox helpBox = null;
    [SerializeField] private GameObject tutoMsgBox = null;

    [Header("업적 패널 관련")]
    [SerializeField] private GameObject panelObject = null;
    [SerializeField] private Transform contentTransform = null;
    public List<AchievementBase> achievementPanels = new List<AchievementBase>();

    [SerializeField] GameObject msgBox = null;

    private List<GameObject> activePanals = new List<GameObject>();


    private void Start()
    {
        SetSettingPanel();
        CreatePanels();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (activePanals.Count != 0)
            {
                UnActivePanal(activePanals[activePanals.Count - 1]);
                return;
            }
            else
            {
                ActivePanal(msgBox);
                return;
            }

        }
    }

    public void StartButtonOnClick()
    {
        if (!DataManager.Inst.CurrentPlayer.isTuto)
        {
            ShowTutoMsgBox();
            return;
        }

        SceneManager.LoadScene("Main");
    }

    public void DataReset()
    {
        DataManager.Inst.DataReset();
    }

    public void OnClickHelpBtn()
    {
        if(!DataManager.Inst.CurrentPlayer.isTuto)
        {
            DataManager.Inst.CurrentPlayer.isTuto = true;
        }
    }

    public void ShowTutoMsgBox()
    {
        tutoMsgBox.SetActive(true);
        tutoMsgBox.transform.DOScale(Vector3.one, 0.5f);
        tutoMsgBox.transform.DOScale(Vector3.zero, 0.3f).SetDelay(4f).OnComplete(() => tutoMsgBox.SetActive(false));
    }

    public void ActivePanal(GameObject panal)
    {
        activePanals.Add(panal);
        panal.SetActive(true);
        panal.transform.DOKill();
        panal.transform.DOScale(Vector3.one, 0.5f);
    }

    public void UnActivePanal(GameObject panal)
    {
        activePanals.Remove(panal);
        panal.transform.DOKill();
        panal.SetActive(true);
        panal.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() => panal.SetActive(false));
    }

    #region SettingPanel
    public void SetSettingPanel()
    {
        effectSlider.value = DataManager.Inst.CurrentPlayer.effectVolume;
        bgmSlider.value = DataManager.Inst.CurrentPlayer.bgmVolume;
        fontSlider.value = DataManager.Inst.CurrentPlayer.fontSize;
        testText.fontSize = DataManager.Inst.CurrentPlayer.fontSize;
        if (DataManager.Inst.CurrentPlayer.bgmMute)
            bgmMute.isOn = true;
        if (DataManager.Inst.CurrentPlayer.effectMute)
            effectMute.isOn = true;
    }
    public void OnClickHelpBtn(int helpID)
    {
        HelpTexts helpTexts = DataManager.Inst.GetHelpTexts();
        helpBox.SetHelpText(helpTexts.helpTextList[helpID], helpID + 1);
    }
    public void BGMVolume(float value)
    {
        SoundManager.Inst.BGMVolume(value);
        DataManager.Inst.SaveToJson();
    }

    public void BGMMute(bool isMute)
    {
        SoundManager.Inst.BGMMute(isMute);
        DataManager.Inst.SaveToJson();
    }

    public void EffectMute(bool isMute)
    {
        SoundManager.Inst.EffectMute(isMute);
        DataManager.Inst.SaveToJson();
    }

    public void EffectVolume(float value)
    {
       SoundManager.Inst.EffectVolume(value);
        DataManager.Inst.SaveToJson();
    }
    public void SetFontSize(float value)
    {
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
