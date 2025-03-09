using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject Effect;
    public GameObject EnemyEffect;
    public AudioClip clip;
    public AnimationManager camera_Shake;

    public GameObject currentSpriteObject;
    public Sprite enemy_DamagedSprite;

    public int ballType; // Type of ball (color)
    public float checkRadius = 0.6f; // Detection range for nearby balls
    public bool isTriggerBall = false; // True if this is a trigger ball

    public bool isEnemyBall = false; // True if this is an enemy ball
    public int enemyHealth = 3; // Health for enemy balls

    public float addedMana = 0.5f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        if (currentSpriteObject == null)
        {
            currentSpriteObject = transform.Find("Icon").gameObject; // Replace "ChildSprite" with the actual name
        }

        if (currentSpriteObject != null)
        {
            spriteRenderer = currentSpriteObject.GetComponent<SpriteRenderer>(); // Get the SpriteRenderer
        }
        else
        {
            Debug.LogError("No sprite object found! Assign it in the Inspector or check the name.");
        }

        camera_Shake = FindObjectOfType<AnimationManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isTriggerBall) return; // Trigger balls don't form connections

        if (collision.gameObject.CompareTag("Ball") || collision.gameObject.CompareTag("Ground"))
        {
            List<Ball> connectedBalls = new List<Ball>();
            FindConnectedBalls(this, connectedBalls);

            // Count only normal balls (ignore enemy balls)
            int normalBallCount = connectedBalls.FindAll(b => !b.isEnemyBall).Count;

            if (normalBallCount < 3)
            {
                connectedBalls.Clear(); // Reset list if not enough normal balls
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Ball otherBall = other.GetComponent<Ball>();

        if (isTriggerBall && otherBall != null && otherBall.ballType == this.ballType)
        {
            List<Ball> destroyGroup = new List<Ball>();
            FindConnectedBalls(otherBall, destroyGroup);

            int normalBallCount = destroyGroup.FindAll(b => !b.isEnemyBall).Count;
            Debug.Log("Trigger Ball activated! Normal Ball Count: " + normalBallCount);

            if (normalBallCount >= 4)
            {
                Debug.Log("Destroy Group contains:");
                foreach (Ball ball in destroyGroup)
                {
                    Debug.Log(ball.name + " | Enemy: " + ball.isEnemyBall);

                    if (ball.isEnemyBall)
                    {
                        Debug.Log(ball.name + " is an enemy and should take damage!");
                        ball.TakeDamage(1);
                    }
                    else
                    {
                        Debug.Log(ball.name + " is destroyed.");
                        if (ball.name == "Fire(Clone)")
                        {
                            ManaBar.instance.AddMana(addedMana);
                        }
                        else if (ball.name == "Leaf(Clone)")
                        {
                            ManaBar.instance.AddGreenMana(addedMana);
                        }
                        else if(ball.name == "Lighting(Clone)")
                        {
                            ManaBar.instance.AddBlueMana(addedMana);
                        }

                        Destroy(ball.gameObject);
                    }
                }
                Debug.Log(name + " (Trigger Ball) destroyed.");
                ComboSystem.instance.IncreaseCombo();
                Destroy(gameObject); // Destroy the trigger ball after activation
                Instantiate(Effect, transform.position, Quaternion.identity);
                camera_Shake.CameraShake();
                AudioSource.PlayClipAtPoint(clip, this.gameObject.transform.position);
            }
        }
    }

    void FindConnectedBalls(Ball ball, List<Ball> connectedBalls)
    {
        if (connectedBalls.Contains(ball)) return;

        connectedBalls.Add(ball);
        Debug.Log(ball.name + " added to list. IsEnemy: " + ball.isEnemyBall);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(ball.transform.position, checkRadius);
        foreach (Collider2D col in colliders)
        {
            Ball nearbyBall = col.GetComponent<Ball>();
            if (nearbyBall != null && nearbyBall.ballType == ball.ballType)
            {
                FindConnectedBalls(nearbyBall, connectedBalls);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(name + " (Enemy) took damage! Current Health: " + enemyHealth);
        enemyHealth -= damage;

        if(enemyHealth == 1)
        {
            ChangeSprite();
        }
        else if (enemyHealth <= 0)
        {
            Debug.Log(name + " (Enemy) destroyed!");
            Instantiate(EnemyEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    void ChangeSprite()
    {
        if (spriteRenderer != null && enemy_DamagedSprite != null)
        {
            spriteRenderer.enabled = false; // Hide the current sprite
            GameObject newSprite = new GameObject("DamagedSprite"); // Create a new GameObject
            SpriteRenderer newRenderer = newSprite.AddComponent<SpriteRenderer>(); // Add SpriteRenderer

            newRenderer.sprite = enemy_DamagedSprite; // Set new sprite
            newRenderer.sortingOrder = 5;
            newSprite.transform.position = currentSpriteObject.transform.position; // Match position
            newSprite.transform.parent = transform; // Parent it to the enemy

            Debug.Log("Enemy sprite changed!");
        }
        else
        {
            Debug.LogError("SpriteRenderer or damagedSprite is missing!");
        }
    }
}
