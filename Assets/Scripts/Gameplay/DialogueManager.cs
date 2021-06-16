using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;
    [SerializeField] Text dialogueText;
    [SerializeField] int lettersPerSecond;
    [SerializeField] GameObject imageBox;

    public event Action OnShowDialogue;
    public event Action onCloseDialogue;

    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;   
    }

    Dialogue dialogue;
    Sprite image;
    
    Action onDialogueFinished;

    int currentLine = 0;
    bool isTyping;

    public bool isShowing { get; private set; }

    public IEnumerator ShowDialogue(Dialogue dialogue, Action onFinished=null)
    {
        yield return new WaitForEndOfFrame();

        OnShowDialogue?.Invoke();

        isShowing = true;
        this.dialogue = dialogue;
        onDialogueFinished = onFinished;

        dialogueBox.SetActive(true);
        StartCoroutine(TypeDialogue(dialogue.Lines[0]));
    }

    public IEnumerator ShowDialogue(Dialogue dialogue, Sprite image, Action onFinished=null)
    {
        yield return new WaitForEndOfFrame();

        OnShowDialogue?.Invoke();

        isShowing = true;
        this.dialogue = dialogue;
        onDialogueFinished = onFinished;

        this.image = image;
        imageBox.GetComponent<Image>().sprite = image;

        dialogueBox.SetActive(true);
        imageBox.SetActive(true);
        StartCoroutine(TypeDialogue(dialogue.Lines[0]));
    }

    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTyping)
        {
            ++currentLine;
            if (currentLine < dialogue.Lines.Count)
            {
                StartCoroutine(TypeDialogue(dialogue.Lines[currentLine]));
            }
            else
            {
                currentLine = 0;
                isShowing = false;
                dialogueBox.SetActive(false);
                imageBox.SetActive(false);
                onDialogueFinished?.Invoke();
                onCloseDialogue?.Invoke();
            }
        }
    }

    public IEnumerator TypeDialogue(string dialogue)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (var letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f/lettersPerSecond);
        }
        isTyping = false;
    }
}
