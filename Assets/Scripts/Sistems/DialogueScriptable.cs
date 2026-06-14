using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "dialogue", menuName = "Dialogues")]
public class DialogueScriptable : ScriptableObject
{
    public List<Dialogue> dialogues;
}
[Serializable]
public class Dialogue
{
    public float startDelay;
    public float duration;
    [TextArea(5,5)]
    public string dialogue;
}