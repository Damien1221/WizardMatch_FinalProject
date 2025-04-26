using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject _comboText;
    public GameObject Effect;
    public Transform manaBarTransform; // Drag this in from Inspector
  
    public AnimationManager camera_Shake;
    public CircleCollider2D ballCollider;

    public GameObject currentSpriteObject;

    public int ballType; // Type of ball (color)
    public float checkRadius = 0.6f; // Detection range for nearby balls

    public float addedMana = 0.5f;

    // Define the area where the collider should turn back on
    public Vector2 areaMin = new Vector2(-3f, 1f);  // Bottom-left corner
    public Vector2 areaMax = new Vector2(3f, 5f);   // Top-right corner

    void Start()
    {
        camera_Shake = FindObjectOfType<AnimationManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float comboMultiplier = 1f + (ComboSystem.instance.GetComboCount() * 0.1f); // 10% more mana per combo
        float scaledMana = addedMana * comboMultiplier;

        if (collision.gameObject.CompareTag("Ball") || collision.gameObject.CompareTag("Ground"))
        {
            List<Ball> connectedBalls = new List<Ball>();
            FindConnectedBalls(this, connectedBalls);

          if (connectedBalls.Count >= 3)
            {
                foreach (Ball ball in connectedBalls)
                {
                    Debug.Log(ball.name + " is destroyed.");
                    if (ball.name == "Fire(Clone)")
                    {
                        ManaBar.instance.AddMana(scaledMana);
                    }
                    else if (ball.name == "Leaf(Clone)")
                    {
                        ManaBar.instance.AddGreenMana(scaledMana);
                    }
                    else if (ball.name == "Lighting(Clone)")
                    {
                        ManaBar.instance.AddBlueMana(scaledMana);
                    }

                    SpawnManaEffect(ball.transform.position);
                    Destroy(ball.gameObject);
                }
                camera_Shake.CameraShake();
                ComboSystem.instance.IncreaseCombo();
                GameObject combo = Instantiate(_comboText, transform.position, Quaternion.identity);
                Destroy(combo, 2f);
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
}
