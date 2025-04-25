using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandController : MonoBehaviour
{
    public Transform hand_Position;
    public Transform target;
    public GameObject sparks_effect;

    public float topScreenLimit = 3.5f; // Adjust this based on your screen setup
    public float bottomScreenLimit = 2.0f; // Lower boundary for hand movement
    public float leftLimit = -7f; // Left boundary
    public float rightLimit = 7f; // Right boundary
    public float moveSpeed = 10f;

    public bool isHandUncontrollable = false;

    private Vector3 mousePosition;
    private Vector3 lastPosition;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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

    public void StopAction()
    {
        isHandUncontrollable = true;
    }
}
