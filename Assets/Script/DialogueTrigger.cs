using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueCharecter
{
    public string name;
    public Sprite icon;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharecter charecter;
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject IntroDialogue;

    public UnityEvent OnFinishDialogue;

    void Start()
    {
        if (IntroDialogue == null)
            return;
    }

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue, this);

        IntroDialogue.SetActive(false);
    }

    public void EndDialogue()
    {
        OnFinishDialogue?.Invoke();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            Debug.Log("wizard Talking");
            TriggerDialogue();
        }
    }
}
