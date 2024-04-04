using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    public TMP_Text name;
    public TMP_Text dialogueText;

    public Animator animator;
    public bool talking = false;
    public float textSpeed;
    private int sentencesCount;
    private string sentenceType;
    private bool complete;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        sentencesCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogueText.text.Length == sentenceType.Length)
        {
            complete = true;
        }
        else
        {
            complete = false;
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        sentencesCount = 0;
        sentences.Clear();
        talking = true;
        name.text = dialogue.name;
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
            sentencesCount++;
        }


    }

    public void DisplayText()
    {
        if(sentences.Count == 0 && complete)
        {
            EndDialogue();
            return;
        }

        if(sentences.Count == sentencesCount)
        {
            sentenceType = sentences.Dequeue();
            StartCoroutine(TypeSentence(sentenceType));
        }
        else if (dialogueText.text.Length < sentenceType.Length && sentences.Count < sentencesCount)
        {
            StopAllCoroutines();
            dialogueText.text = sentenceType;
        }
        else if (dialogueText.text.Length == sentenceType.Length)
        {
            sentenceType = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentenceType));
        }
            
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void EndDialogue()
    {
        talking = false;
        animator.SetBool("IsOpen", false);
        sentences.Clear();
        StopAllCoroutines();
    }
}
