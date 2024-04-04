using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text text;
    public Animator text_animator;
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowArea(string area)
    {
        Debug.Log(2);
        text.text = area;
        text_animator.SetTrigger("trigger");
    }
}
