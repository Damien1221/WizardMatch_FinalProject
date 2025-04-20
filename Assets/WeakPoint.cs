using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WeakPointActivate()
    {
        animator.Play("WeakPoint_Activate");
    }

    public void WeakPointClose()
    {
        animator.Play("WeakPoint_Close");
    }

    public void WeakPointCorrectSpell()
    {
        animator.Play("WeakPoint_CorrectSpell");
    }

    public void WeakPointDestroy()
    {
        animator.Play("WeakPoint_CorrectClose");
    }

}
