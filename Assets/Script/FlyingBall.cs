using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBall : MonoBehaviour
{
    public GameObject fire_Ball;
    public GameObject leaf_Ball;
    public GameObject power_Fire;
    public GameObject power_Leaf;


    public float speed = 3f;
    public float changeDirectionTime = 2f;
    public float leftLimit = -5f; // Left boundary
    public float rightLimit = 5f; // Right boundary
    public float bottomLimit = 2f; // Lower boundary
    public float topLimit = 6f; // Upper boundary

    private Vector2 direction;
    private float timer;
    private bool isGrabbed = false;

    void Start()
    {
        ChangeDirection();
    }

    void Update()
    {
        if (!isGrabbed)
        {
            // Move the ball
            transform.Translate(direction * speed * Time.deltaTime);
            timer += Time.deltaTime;

            // Change direction after a set time
            if (timer > changeDirectionTime)
            {
                ChangeDirection();
            }

            // Keep the ball within left, right, top, and bottom limits
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
                Mathf.Clamp(transform.position.y, bottomLimit, topLimit),
                transform.position.z
            );
        }
    }

    void ChangeDirection()
    {
        direction = Random.insideUnitCircle.normalized;
        timer = 0;
    }

    public void GrabBall(Transform handPosition, HandController handController)
    {
        isGrabbed = true;
        GameObject newBall = null;

        if (gameObject.name == "Flying_Fire(Clone)")
        {
            newBall = Instantiate(fire_Ball, handPosition.position, Quaternion.identity);
        }
        else if (gameObject.name == "Flying_Leaf(Clone)")
        {
            newBall = Instantiate(leaf_Ball, handPosition.position, Quaternion.identity);
        }
        else if(gameObject.name == "Fly_PowerFire(Clone)")
        {
            newBall = Instantiate(power_Fire, handPosition.position, Quaternion.identity);
        }
        else if (gameObject.name == "Fly_PowerLeaf(Clone)")
        {
            newBall = Instantiate(power_Leaf, handPosition.position, Quaternion.identity);
        }

        if (newBall != null)
        {
            newBall.transform.parent = handPosition; // Attach to hand so it moves with it

            // Make sure the new ball is grabbed by the hand
            Rigidbody2D newBallRb = newBall.GetComponent<Rigidbody2D>();
            if (newBallRb != null)
            {
                handController.GrabNewObject(newBallRb);
            }
        }

        Destroy(gameObject); // Destroy the current ball
    }

    public void ReleaseBall()
    {
        isGrabbed = false;
    }
}
