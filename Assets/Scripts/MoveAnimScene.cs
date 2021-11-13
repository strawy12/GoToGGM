using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MoveAnimScene : MonoBehaviour
{
    [SerializeField] RectTransform playerobj;
    [SerializeField] RectTransform stageObjTemp;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Text currentStageText;


    public void StartMoveAnim()
    {
        canvasGroup.DOFade(1f, 1f).OnComplete(() => StartCoroutine(SpawnStage()));
    }

    IEnumerator SpawnStage()
    {
        Scenario scenario = GameManager.Inst.Story.GetNowScenario();
        currentStageText.text = scenario.scenarioName;
        currentStageText.DOFade(0f, 0f);

        currentStageText.DOFade(1f, 1f);

        yield return new WaitForSeconds(1f);

        int objCnt = scenario.stories.Length;
        float[] objLerps = new float[objCnt];
        float interval = 1f / (objCnt - 1);

        RectTransform[] rects = new RectTransform[objCnt];

        for (int i = 0; i < objCnt; i++)
        {
            objLerps[i] = interval * i;
            rects[i] = Instantiate(stageObjTemp, stageObjTemp.transform.parent);
        }

        for (int i = 0; i < objCnt; i++)
        {
            float targetPosX = Mathf.Lerp(-550f, 550f, objLerps[i]);

            Vector2 targetPos = rects[i].anchoredPosition;
            targetPos.x = targetPosX;
            rects[i].anchoredPosition = targetPos;//new Vector2(-550f, rects[i].anchoredPosition.y);
            rects[i].gameObject.SetActive(true);
            rects[i].localScale = Vector3.zero;
            rects[i].DOScale(Vector3.one, 0.3f);
            // rects[i].DOAnchorPosX(targetPos.x, 0.5f);

            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(1f);

        playerobj.anchoredPosition = rects[0].anchoredPosition;
        playerobj.gameObject.SetActive(true);
        playerobj.localScale = Vector3.zero;
        playerobj.DOScale(Vector3.one, 0.5f);
        yield return new WaitForSeconds(1f);

        playerobj.DOAnchorPosX(rects[GameManager.Inst.CurrentPlayer.crtStoryNum].anchoredPosition.x, 1f);

        yield return new WaitForSeconds(1.3f);
        canvasGroup.DOFade(0f, 1f);
        yield return new WaitForSeconds(0.7f);

        GameManager.Inst.Story.StartStory();
        gameObject.SetActive(false);
    }
}

