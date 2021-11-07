using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text storyText = null;
    [SerializeField] private Text jobStatusText = null;
    [SerializeField] private Text statText = null;
    [SerializeField] private GameObject touchScreen = null;
    [SerializeField] private CanvasGroup nicknameInputPanal = null;
    [SerializeField] private SeletingBtnBase[] selectBtns;
    [SerializeField] private Sprite[] selectBtnSprites;
    [SerializeField] private SeletingBtnBase finalSelectBtn = null;

    private InputField nicknameInputField = null;

    private bool isWriting = false;
    private bool isSkip = false;
    private float currentWriteSpeed = 0f;

    private void Awake()
    {
        nicknameInputField = nicknameInputPanal.GetComponentInChildren<InputField>();
    }
    private void Start()
    {

    }

    public void StartWrite(string message, Action action = null, float writeSpeed = 0.03f)
    {
        StartCoroutine(WriteText(message, action, writeSpeed));
    }

    public void StartWrite<T>(string message, Action<T> action, T param, float writeSpeed = 0.03f)
    {
        StartCoroutine(WriteText(message, action, param, writeSpeed));
    }

    public IEnumerator WriteText(string message, Action action, float writeSpeed)
    {
        isWriting = true;
        ActiveTouchScreen(true);
        currentWriteSpeed = writeSpeed;
        float waitTime = currentWriteSpeed * 2f;
        string messageText = "";

        if (isSkip) isSkip = false;

        yield return new WaitForSeconds(0.05f);
        message = message.Replace("&", GameManager.Inst.CurrentPlayer.nickname);
        message = message.Replace("*", GameManager.Inst.CurrentPlayer.playerjob);

        foreach (var c in message)
        {
            if (isSkip) break;

            messageText = string.Format("{0}{1}", messageText, c);
            storyText.text = messageText;
            yield return new WaitForSeconds(currentWriteSpeed);
            if (c == '\n')
            {
                yield return new WaitForSeconds(0.5f);
            }
        }

        if (!isSkip)
        {
            yield return new WaitForSeconds(1.25f);
        }
        else
        {
            storyText.text = message;
            yield return new WaitForSeconds(0.5f);
            isSkip = false;
        }

        if (action == null)
        {
            ShowSelectBtn();
        }

        else
        {
            action();
        }

        isWriting = false;
    }

    public IEnumerator WriteText<T>(string message, Action<T> action, T param, float writeSpeed)
    {
        isWriting = true;
        ActiveTouchScreen(true);
        currentWriteSpeed = writeSpeed;
        float waitTime = currentWriteSpeed * 2f;
        string messageText = "";

        if (isSkip) isSkip = false;

        yield return new WaitForSeconds(0.05f);

        message = message.Replace("&", GameManager.Inst.CurrentPlayer.nickname);
        message = message.Replace("*", GameManager.Inst.CurrentPlayer.playerjob);

        foreach (var c in message)
        {
            if (isSkip) break;

            messageText = string.Format("{0}{1}", messageText, c);
            storyText.text = messageText;
            yield return new WaitForSeconds(currentWriteSpeed);
            if (c == '\n')
            {
                yield return new WaitForSeconds(0.5f);
            }
        }

        if (!isSkip)
        {
            yield return new WaitForSeconds(1.25f);
        }
        else
        {
            storyText.text = message;
            yield return new WaitForSeconds(0.5f);
            isSkip = false;
        }

        if (action == null)
        {
            ShowSelectBtn();
        }

        else
        {
            action(param);
        }

        isWriting = false;
    }

    public void ActiveNameInputField(bool isActive)
    {
        if(isActive)
        {
            nicknameInputPanal.gameObject.SetActive(true);
            nicknameInputPanal.DOFade(1f, 1f);
            GameManager.Inst.Story.EndStory();
        }

        else
        {
            nicknameInputPanal.DOFade(0f, 1f);
            nicknameInputPanal.gameObject.SetActive(false);
        }


    }

    public void OnClickNickNameInput()
    {
        if (nicknameInputField.text == "") return;

        GameManager.Inst.CurrentPlayer.nickname = nicknameInputField.text;
        ActiveNameInputField(false);

        GameManager.Inst.Story.SetStoryNum();
        GameManager.Inst.Story.StartStory();
    }

    public void ShowSelectBtn()
    {
        SelectLine[] selectLines = GameManager.Inst.Story.GetNowStory().selectLines;

        for (int i = 0; i < selectLines.Length; i++)
        {
            selectBtns[i].ActiveBtn(true);
            selectBtns[i].SettingBtn(selectLines[i], i);
        }
    }
    public void UnShowSelectBtn(SeletingBtnBase seletingBtn = null)
    {
        for (int i = 0; i < 3; i++)
        {
            if (seletingBtn == selectBtns[i]) continue;
            selectBtns[i].ActiveBtn(false);
        }
    }

    public void ActiveFinalSelectBtn()
    {
        for (int i = 0; i < 3; i++)
        {
            if (selectBtns[i].SelectType == ESelectType.Final)
            {
                selectBtns[i].ActiveBtn(true);

                GameManager.Inst.Story.EndStory();
                return;
            }
        }
    }

    public void OnClickTouchScreen()
    {
        if (!GameManager.Inst.Story.IsEndStory && isWriting)
        {
            isSkip = true;
            ActiveTouchScreen(false);
        }
        else
        {
            return;
        }
    }

    public void ActiveTouchScreen(bool isActive)
    {
        touchScreen.SetActive(isActive);
    }

    public void SetEventToSelectBtn(bool isRemove)
    {
        for (int i = 0; i < 3; i++)
        {
            selectBtns[i].SetEvent(selectBtns[i].SetPlayerJob, isRemove);
        }
    }

    public void ChangeSelectBtnSprite(Image image, ESelectType type)
    {
        int num = (int)type;
        image.sprite = selectBtnSprites[num];
    }

    public void SetJobText()
    {
        jobStatusText.text = GameManager.Inst.CurrentPlayer.playerjob;
    }

    public void SetStatText()
    {
        int wit = GameManager.Inst.CurrentPlayer.stat_Wit;
        int knowledge = GameManager.Inst.CurrentPlayer.stat_Knowledge;
        int sencetive = GameManager.Inst.CurrentPlayer.stat_Sencetive;

        statText.text = string.Format("재치: {0} / 섬세: {1} / 지식: {2}", wit, sencetive, knowledge);

    }

    private void FadeObject(Image obj)
    {

    }
}
