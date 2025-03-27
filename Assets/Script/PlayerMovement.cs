using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 3f;

    private Animator _anim;
    private float horizontal = 0.0f;
    private float speed = 0.0f;

    private Vector2 movement;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
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
        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
    }

}
