using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GameData;

public class Character : MonoBehaviour
{
    private SpriteRenderer spriteRenderer = null;
    [SerializeField] private SpriteRenderer background = null;
    [SerializeField] private Sprite normalCharaCterSprite = null;
    private bool isTouch = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalCharaCterSprite;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.currentSelectedGameObject == null && !EventSystem.current.IsPointerOverGameObject())
        {
            isTouch = true;
            transform.Rotate(0f, 0f, -10f);
        }
        else
        {
            isTouch = false;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    private void OnMouseUp()
    {
        if (isTouch)
        {
        }
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

    }

}

