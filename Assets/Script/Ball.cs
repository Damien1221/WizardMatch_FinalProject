using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject Effect;
    public GameObject EnemyEffect;
    public AudioClip clip;
    //public AnimationManager camera_Shake;

    public int ballType; // Type of ball (color)
    public float checkRadius = 0.6f; // Detection range for nearby balls
    public bool isTriggerBall = false; // True if this is a trigger ball

    public bool isEnemyBall = false; // True if this is an enemy ball
    public int enemyHealth = 3; // Health for enemy balls
    

    private void Start()
    {
        //camera_Shake = FindObjectOfType<AnimationManager>();
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
                        Destroy(ball.gameObject);
                        //Put the mana here
                        ManaBar.instance.AddMana(1); 

                    }
                }
                Debug.Log(name + " (Trigger Ball) destroyed.");
                Destroy(gameObject); // Destroy the trigger ball after activation
                Instantiate(Effect, transform.position, Quaternion.identity);
                //camera_Shake.CameraShake();
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

        if (enemyHealth <= 0)
        {
            Debug.Log(name + " (Enemy) destroyed!");
            Instantiate(EnemyEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
