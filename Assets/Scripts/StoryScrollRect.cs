using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class StoryScrollRect : ScrollRect
{
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (GameManager.Inst.UI.CheckWriting()) return;
        base.OnBeginDrag(eventData);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (GameManager.Inst.UI.CheckWriting()) return;

        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (GameManager.Inst.UI.CheckWriting()) return;

        base.OnEndDrag(eventData);
    }

    public void SetContentPos()
    {
        content.DOAnchorPosY(0f, 0.3f).SetEase(Ease.OutBack);
    }
}
