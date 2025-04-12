using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject Effect;
    public Transform manaBarTransform; // Drag this in from Inspector

    public GameObject EnemyEffect;
    public AudioClip clip;
    public AnimationManager camera_Shake;
    public CircleCollider2D ballCollider;

    public GameObject currentSpriteObject;
    public Sprite enemy_DamagedSprite;

    public int ballType; // Type of ball (color)
    public float checkRadius = 0.6f; // Detection range for nearby balls

    public bool isEnemyBall = false; // True if this is an enemy ball
    public int enemyHealth = 3; // Health for enemy balls

    public float addedMana = 0.5f;

    // Define the area where the collider should turn back on
    public Vector2 areaMin = new Vector2(-3f, 1f);  // Bottom-left corner
    public Vector2 areaMax = new Vector2(3f, 5f);   // Top-right corner

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
        if (collision.gameObject.CompareTag("Ball") || collision.gameObject.CompareTag("Ground"))
        {
            List<Ball> connectedBalls = new List<Ball>();
            FindConnectedBalls(this, connectedBalls);

            int normalBallCount = connectedBalls.FindAll(b => !b.isEnemyBall).Count;

            if (normalBallCount >= 3)
            {
                foreach (Ball ball in connectedBalls)
                {
                    if (ball.isEnemyBall)
                    {
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
                        else if (ball.name == "Lighting(Clone)")
                        {
                            ManaBar.instance.AddBlueMana(addedMana);
                        }

                        SpawnManaEffect(ball.transform.position);
                        Destroy(ball.gameObject);
                    }
                }

                //spawn ball
                camera_Shake.CameraShake();
                AudioSource.PlayClipAtPoint(clip, this.gameObject.transform.position);
                ComboSystem.instance.IncreaseCombo();
            }
        }
    }

    void SpawnManaEffect(Vector3 spawnPos)
    {
        // Add a small random offset
        Vector3 offset = new Vector3(
            Random.Range(-0.3f, 0.3f),
            Random.Range(-0.3f, 0.3f),
            0f
        );

        Vector3 finalPos = spawnPos + offset;

        GameObject particle = Instantiate(Effect, spawnPos, Quaternion.identity);
        ManaParticle mp = particle.GetComponent<ManaParticle>();
        mp.target = manaBarTransform;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (ballCollider.isTrigger && other.CompareTag("CupZone"))
        {
            ballCollider.isTrigger = false;
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
