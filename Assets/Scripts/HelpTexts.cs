using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HelpTexts", menuName = "Sprictable Object/HelpTexts")]
public class HelpTexts : ScriptableObject
{
    [TextArea] public List<string> helpTextList;
}