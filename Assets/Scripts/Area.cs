using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Area : MonoBehaviour
{
    // Start is called before the first frame update
    public Image image;
    public Animator animator;

    void Start()
    {
        animator = image.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            animator.SetTrigger("trigger");
        }
    }
}
