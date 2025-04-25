using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    internal Queue<DialogueLine> lines;
    private Animator animator;
    private PlayerMovement _playerMovement;
    private DialogueTrigger currentDialogueTrigger;

    public bool isDialogueActive = false;
    public float typingSpeed = 0.2f;   

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        _playerMovement = FindObjectOfType<PlayerMovement>();

        if(Instance == null)
            Instance = this;

        lines = new Queue<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue, DialogueTrigger dialogueTrigger)
    {
        currentDialogueTrigger = dialogueTrigger;
        isDialogueActive = true;

        animator.Play("Dialogue_Start");

        if (_playerMovement != null)
            _playerMovement.FreezeMovement();


        lines.Clear();

        foreach (DialogueLine dialogueline in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueline);
        }

        DisplayNextDialogueLines();
    }

    public void DisplayNextDialogueLines()
    {
        if(lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        characterIcon.sprite = currentLine.charecter.icon;
        characterName.text = currentLine.charecter.name;

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueline)
    {
        dialogueArea.text = "";
        foreach(char letter in dialogueline.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void EndDialogue()
    {
        if (currentDialogueTrigger != null)
        {
            currentDialogueTrigger.EndDialogue();
        }   

        isDialogueActive = false;
        animator.Play("Dialogue_Out");
        if (_playerMovement != null)
            _playerMovement.UnfreezeMovement();
    }
}
