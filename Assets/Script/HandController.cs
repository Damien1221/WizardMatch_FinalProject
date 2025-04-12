using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public Transform hand_Position;
    public Transform target;

    public float topScreenLimit = 3.5f; // Adjust this based on your screen setup
    public float bottomScreenLimit = 2.0f; // Lower boundary for hand movement
    public float leftLimit = -7f; // Left boundary
    public float rightLimit = 7f; // Right boundary
    public float moveSpeed = 10f;
    public float StunTime = 2f;

    public float throwForceMultiplier = 2f; // Adjust for stronger/weaker throws

    public bool isHandUncontrollable = false; // Track uncontrollable state

    private Ball _ball;
    private AnimationManager grab_Hand;
    private Rigidbody2D grabbedObject = null;
    private Vector3 mousePosition;

    private Vector3 lastPosition;
    private Vector3 velocity;

    void Start()
    {
        grab_Hand = FindObjectOfType<AnimationManager>();
        _ball = FindObjectOfType<Ball>();
    }

    void Update()
    {
        if (target != null)
        {
            target.position = transform.position;
        }

        if (!isHandUncontrollable)
        {
            FollowMouse();
        }

        // Handle grabbing and releasing objects
        if (Input.GetMouseButtonDown(0))
        {
            TryGrabObject();
            grab_Hand.ClosingHand();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ReleaseObject();
            grab_Hand.OpeningHand();
        }
    }

    public void FollowMouse()
    {
        // Calculate the hand velocity
        velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

        // Move the hand with the mouse, but limit Y position
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        mousePosition.y = Mathf.Clamp(mousePosition.y, bottomScreenLimit, topScreenLimit); // Restrict vertical movement
        mousePosition.x = Mathf.Clamp(mousePosition.x, leftLimit, rightLimit); // Restrict left/right movement
        transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed * Time.deltaTime);
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
            // Remove freeze constraint so the ball can move freely
            grabbedObject.constraints = RigidbodyConstraints2D.None;

            // Apply throwing force
            grabbedObject.velocity = velocity * throwForceMultiplier;

            // Detach from hand
            grabbedObject.gravityScale = 1f;
            grabbedObject.gameObject.GetComponent<Ball>().ballCollider.isTrigger = true;
            grabbedObject.transform.parent = null;
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

    public IEnumerator UncontrollableHand()
    {
        Debug.Log("Hand is uncontrollable!");
        grab_Hand.CameraShakingHard();

        isHandUncontrollable = true; // Disable normal mouse control
        float duration = StunTime; // Effect lasts
        float timer = 0f;

        while (timer < duration)
        {
            float randomX = Random.Range(-1f, 1f);
            float randomY = Random.Range(-1f, 1f);
            transform.position += new Vector3(randomX, randomY, 0) * Time.deltaTime * 5f; // Random movement

            timer += Time.deltaTime;
            yield return null;
        }

        isHandUncontrollable = false; // Restore mouse control
        Debug.Log("Hand control restored!");
        grab_Hand.CameraStopShaking();
    }

}
