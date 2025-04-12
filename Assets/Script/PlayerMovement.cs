using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject InteractIcon;
    public float movementSpeed = 3f;

    private Animator _anim;
    private float horizontal = 0.0f;
    private float speed = 0.0f;
    private bool canTalk = false;
    private bool canMove = true;

    private DialogueManager dialogueManager;
    private Vector2 movement;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        dialogueManager = FindObjectOfType<DialogueManager>();

        gameObject.transform.localScale = new Vector3(2, 2, 2);

    }

    void Update()
    {
        if (canTalk && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("RunningDialogue");
            InteractIcon.SetActive(false);
            dialogueManager.DisplayNextDialogueLines();
        }
          
        if (!canMove)
        {
            _anim.SetFloat("Speed", 0);
            return;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        horizontal = movement.x > 0.01f ? movement.x : movement.x < -0.01f ? 1 : 0;
        speed = movement.y > 0.01f ? movement.y : movement.y < -0.01 ? 1 : 0;

        if(movement.x < -0.01f)
        {
            gameObject.transform.localScale = new Vector3(-2, 2, 2);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(2, 2, 2);
        }

        _anim.SetFloat("Horizontal", horizontal);
        _anim.SetFloat("Vertical", movement.y);
        _anim.SetFloat("Speed", speed);

    }

    void FixedUpdate()
    {
        if (!canMove)
            return;

        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
    }

    public void FreezeMovement()
    {
        canMove = false;
        rb.velocity = Vector2.zero; // stop any leftover movement
    }

    public void UnfreezeMovement()
    {
        canMove = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
            InteractIcon.SetActive(true);
            canTalk = true;
        }
        else if(col.gameObject.CompareTag("Intro"))
        {
            canTalk = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            InteractIcon.SetActive(false);
            canTalk = false;
        }
        else if (col.gameObject.CompareTag("Intro"))
        {
            canTalk = false;
        }
    }

}
