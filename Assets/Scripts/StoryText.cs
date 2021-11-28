using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryText : MonoBehaviour
{
    private Text storyText = null;
    private ContentSizeFitter contentSize = null;
    public string text
    {
        get
        {
            if(storyText == null)
            {
                storyText = GetComponent<Text>();
            }

            return storyText.text;
        }

        set
        {
            if (storyText == null)
            {
                storyText = GetComponent<Text>();
            }

            storyText.text = value;
        }
    }


    private void Awake()
    {
        storyText = GetComponent<Text>();
        contentSize = GetComponent<ContentSizeFitter>();
    }


    public void NextText()
    {
        storyText.text = string.Format("{0}\n\n\n\n\n", storyText.text);
        contentSize.enabled = true;
    }
}
