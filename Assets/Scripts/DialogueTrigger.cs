using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public DialogueManager manager;
    public Animator animator;

    private bool trigger;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.talking)
        {
            if (Input.GetMouseButtonDown(1) && trigger)
            {
                manager.StartDialogue(dialogue);
                manager.DisplayText();
            }
        }

        if(trigger)
        {
            animator.SetBool("trigger", true);
        }
        else
        {
            animator.SetBool("trigger", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        trigger = true;
       
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        trigger = false;

    }
}
