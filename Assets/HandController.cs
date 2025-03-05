using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public Transform hand_Position;

    public float topScreenLimit = 3.5f; // Adjust this based on your screen setup
    public float bottomScreenLimit = 2.0f; // Lower boundary for hand movement
    public float leftLimit = -7f; // Left boundary
    public float rightLimit = 7f; // Right boundary
    public float moveSpeed = 10f;

    private Rigidbody2D grabbedObject = null;
    private Vector3 mousePosition;

    void Update()
    {
        // Move the hand with the mouse, but limit Y position
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        mousePosition.y = Mathf.Clamp(mousePosition.y, bottomScreenLimit, topScreenLimit); // Restrict vertical movement
        mousePosition.x = Mathf.Clamp(mousePosition.x, leftLimit, rightLimit); // Restrict left/right movement
        transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed * Time.deltaTime);


        // Handle grabbing and releasing objects
        if (Input.GetMouseButtonDown(0))
        {
            TryGrabObject();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ReleaseObject();
        }
    }

    void TryGrabObject()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
        if (hit.collider != null && hit.collider.attachedRigidbody != null)
        {
            grabbedObject = hit.collider.attachedRigidbody;

            // Check if the grabbed object is a FlyingBall
            FlyingBall flyingBall = grabbedObject.GetComponent<FlyingBall>();
            if (flyingBall != null)
            {
                flyingBall.GrabBall(hand_Position, this); // Pass hand reference
            }

            grabbedObject.gravityScale = 0f;
            grabbedObject.velocity = Vector2.zero;
            grabbedObject.transform.parent = transform; // Attach to hand
        }
    }

    void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.transform.parent = null; // Detach from hand

            FlyingBall flyingBall = grabbedObject.GetComponent<FlyingBall>();
            if (flyingBall != null)
            {
                flyingBall.ReleaseBall(); // Resume movement
            }

            grabbedObject.gravityScale = 1f;
            grabbedObject = null;
        }
    }

    public void GrabNewObject(Rigidbody2D newObject)
    {
        grabbedObject = newObject;
        grabbedObject.gravityScale = 0f;
        grabbedObject.velocity = Vector2.zero;
        grabbedObject.transform.parent = transform; // Attach to hand
    }
}
