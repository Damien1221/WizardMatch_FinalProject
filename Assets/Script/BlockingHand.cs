using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingHand : MonoBehaviour
{
    public float timer = 20f;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void HandMoveIn()
    {
        animator.Play("BlockingHand_Move");

        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        animator.Play("BlockingHand_MoveOut");
    }
}
