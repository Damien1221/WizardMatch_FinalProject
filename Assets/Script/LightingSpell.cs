using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingSpell : MonoBehaviour
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

    public void ActivateLighting()
    {
        animator.Play("LightingRing_Activate");
    }

    public void CloseMagicRing()
    {
        animator.Play("LightingRing_Close");
    }
}
