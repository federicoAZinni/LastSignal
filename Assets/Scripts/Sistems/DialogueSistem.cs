using System.Collections;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;

public class DialogueSistem : MonoBehaviour
{
    [Header("UI refs")]
    [SerializeField] GameObject panelDialogueUI;
    [SerializeField] TextMeshProUGUI textDialogue;
    [Header("Dialogue refs")]
    [SerializeField] DialogueScriptable dialogue;



    public void StartDialogue()
    {
        LeanTween.moveLocalY(panelDialogueUI, 100, 0.3f).setEaseOutBack();

        StartCoroutine(DialogueRoutine());
    }

    IEnumerator DialogueRoutine()
    {
        foreach (var item in dialogue.dialogues)
        {
            yield return StartCoroutine(WriteTextAnim(item));
        }

        yield return new WaitForSeconds(1f);

        LeanTween.moveLocalY(panelDialogueUI, -150, 0.3f).setEaseInBack();
    }

    IEnumerator WriteTextAnim(Dialogue dia)
    {
        yield return new WaitForSeconds(dia.startDelay);

        textDialogue.text = "";

        if (dia.dialogue.Length == 0)
            yield break;

        float delay = dia.duration / dia.dialogue.Length;

        foreach (char c in dia.dialogue)
        {
            textDialogue.text += c;
            yield return new WaitForSeconds(delay);
        }
    }
}
