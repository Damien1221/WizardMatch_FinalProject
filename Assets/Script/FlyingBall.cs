using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBall : MonoBehaviour
{
    //grabObject
    public GameObject fire_Ball;
    public GameObject leaf_Ball;
    public GameObject lighting_Ball;

    // Glowing forms of flying objects
    public GameObject glowing_Fire;
    public GameObject glowing_Leaf;
    public GameObject glowing_Lighting;

    //Flying object
    public GameObject flying_fire_Ball;
    public GameObject flying_leaf_Ball;
    public GameObject flying_lighting_Ball;

    public float speed = 3f;
    public float changeDirectionTime = 2f;
    public float leftLimit = -5f; // Left boundary
    public float rightLimit = 5f; // Right boundary
    public float bottomLimit = 2f; // Lower boundary
    public float topLimit = 6f; // Upper boundary

    private static bool isTransformed = false; // Tracks whether objects are glowing
    private static List<FlyingBall> allFlyingBalls = new List<FlyingBall>(); // Store all flying balls

    private BallSpawner ball_Spawner;
    private Vector2 direction;
    private float timer;
    private bool isGrabbed = false;

    private float originalSpeed;
    private bool isSlowedDown = false;

    private static bool canToggle = true;

    void Start()
    {
        ball_Spawner = FindObjectOfType<BallSpawner>();
        originalSpeed = speed;

        ChangeDirection();

        if (!allFlyingBalls.Contains(this))
        {
            allFlyingBalls.Add(this); // Add to list for toggling
        }

        DrawingSystem drawingSystem = FindObjectOfType<DrawingSystem>();
        if (drawingSystem != null)
        {
            drawingSystem.flying_Ball = this;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleAllFlyingObjects();
        }

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
                transform.position.z);
        }
    }

    public static void ToggleAllFlyingObjects()
    {
        if (!canToggle)
            return; // If toggling is not allowed, do nothing

        isTransformed = !isTransformed; // Toggle transformation state

        foreach (FlyingBall ball in allFlyingBalls.ToArray())
        {
            if (ball != null)
            {
                ball.ToggleFlyingObject();
            }
        }
    }

    public static void EnableToggle()
    {
        canToggle = true;
    }

    public static void DisableToggle()
    {
        canToggle = false;
    }

    private void ToggleFlyingObject()
    {
        GameObject newObjectPrefab = GetToggledPrefab();

        if (newObjectPrefab == null)
        {
            Debug.LogError("Missing glowing or original prefab!");
            return;
        }

        GameObject newObject = Instantiate(newObjectPrefab, transform.position, Quaternion.identity);
        FlyingBall newFlyingBall = newObject.GetComponent<FlyingBall>();

        if (newFlyingBall != null)
        {
            newFlyingBall.CopyFlyingBallData(this); // Copy movement and settings
            newFlyingBall.RestartMovement(); // Ensures movement is reset properly
        }
        else
        {
            Debug.LogError("The instantiated object does not have a FlyingBall component!");
        }

        Destroy(gameObject); // Remove the old version
    }

    private GameObject GetToggledPrefab()
    {
        if (isTransformed)
        {
            // Transform to glowing version
            if (gameObject.name.Contains("Flying_Fire")) return glowing_Fire;
            if (gameObject.name.Contains("Flying_Leaf")) return glowing_Leaf;
            if (gameObject.name.Contains("Flying_Lighting")) return glowing_Lighting;
        }
        else
        {
            // Transform back to original
            if (gameObject.name.Contains("Glowing_Fire")) return flying_fire_Ball;
            if (gameObject.name.Contains("Glowing_Leaf")) return flying_leaf_Ball;
            if (gameObject.name.Contains("Glowing_Lighting")) return flying_lighting_Ball;
        }

        return null; // No matching object found
    }

    private void CopyFlyingBallData(FlyingBall oldBall)
    {
        speed = oldBall.speed;
        changeDirectionTime = oldBall.changeDirectionTime;
        leftLimit = oldBall.leftLimit;
        rightLimit = oldBall.rightLimit;
        bottomLimit = oldBall.bottomLimit;
        topLimit = oldBall.topLimit;
        direction = oldBall.direction;
        timer = oldBall.timer;
        isGrabbed = oldBall.isGrabbed; 
    }

    private void RestartMovement()
    {
        Debug.Log("Restarting movement for " + gameObject.name);
        direction = Random.insideUnitCircle.normalized; // Ensure a new movement direction

        // Prevent falling due to gravity
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0; // Disable gravity
            rb.velocity = Vector2.zero; // Reset any downward movement
        }

        transform.position = new Vector3(transform.position.x, Mathf.Max(transform.position.y, bottomLimit + 1f), transform.position.z); // Lift slightly above bottom limit
        timer = 0;
    }

    void ChangeDirection()
    {
        direction = Random.insideUnitCircle.normalized;
        timer = 0;
    }

    public void GrabBall(Transform handPosition, HandController handController)
    {
        isGrabbed = true;
        ball_Spawner.SpawningOneBall();

        GameObject newBall = null;

        if (gameObject.CompareTag("BadFlyingBall")) // Check if it's a bad ball
        {
            handController.StartCoroutine(handController.UncontrollableHand());
        }
        else
        {
            if (gameObject.name == "Flying_Fire(Clone)")
            {
                newBall = Instantiate(fire_Ball, handPosition.position, Quaternion.identity);
            }
            else if (gameObject.name == "Flying_Leaf(Clone)")
            {
                newBall = Instantiate(leaf_Ball, handPosition.position, Quaternion.identity);
            }
            else if (gameObject.name == "Flying_Lighting(Clone)")
            {
                newBall = Instantiate(lighting_Ball, handPosition.position, Quaternion.identity);
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

        }
        
        Destroy(gameObject); // Destroy the current ball
    }

    public void ReleaseBall()
    {
        isGrabbed = false;
    }

    public void SetChangeDirectionTime(float newTime)
    {
        changeDirectionTime = newTime;
        Debug.Log("changeDirectionTime set to: " + changeDirectionTime);
    }

    public void SlowDownForDrawing()
    {
        if (!isSlowedDown)
        {
            Debug.Log("Calling SlowDownBall()");
            StartCoroutine(SlowDownBall());
            StartCoroutine(ResetChangeDirectionTime());
        }
        else
        {
            Debug.Log("Already slowed down, ignoring...");
        }
    }

    private System.Collections.IEnumerator SlowDownBall()
    {
        Debug.Log("SlowDownBall() started!"); // Check if function is triggered

        isSlowedDown = true;
        speed = 0.5f; // Reduce speed by half
        Debug.Log("Speed reduced to: " + speed);

        yield return new WaitForSeconds(5f); // Wait for 5 seconds

        speed = originalSpeed; // Restore original speed
        Debug.Log("Speed restored to: " + speed);

        isSlowedDown = false;
    }
    IEnumerator ResetChangeDirectionTime()
    {
        yield return new WaitForSeconds(5f);
        changeDirectionTime = 2f; // Reset to default
        Debug.Log("changeDirectionTime reset to default: " + changeDirectionTime);
    }

    private void OnDestroy()
    {
        allFlyingBalls.Remove(this); // Remove from list when destroyed
    }
}
