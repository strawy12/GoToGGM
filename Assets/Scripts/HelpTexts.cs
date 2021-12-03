using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HelpTexts", menuName = "Sprictable Object/HelpTexts")]
public class HelpTexts : ScriptableObject
{
    [TextArea(3, 10)] public List<string> helpTextList;
}