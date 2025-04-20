using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistBall : MonoBehaviour
{
    public float speed = 2f;           // Speed moving left
    public float waveAmplitude = 0.5f; // How high up/down the wave goes
    public float waveFrequency = 1f;   // How fast it goes up/down

    private BlockingHand _handAnim;
    private BallSpawner _ballspawner;
    private float originalY;           // Remember starting Y position

    
    void Start()
    {
        _handAnim = FindObjectOfType<BlockingHand>();
        _ballspawner = FindObjectOfType<BallSpawner>();

        originalY = transform.position.y;
    }

    void Update()
    {
        // Left movement
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Wave movement
        float newY = originalY + Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Destroy if off screen
        if (transform.position.x < -11f) // Adjust based on your screen size
        {
            _ballspawner.StartCoroutine(_ballspawner.RespawnAfterDelay());
            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        _handAnim.HandMoveIn();
        _ballspawner.StartCoroutine(_ballspawner.RespawnAfterDelay());
        Destroy(gameObject);
    }
}
