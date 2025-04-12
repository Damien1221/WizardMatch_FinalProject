using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBadGuy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float waitTime = 2f; // Time to stop at a location
    public float moveTime = 3f; // Time to move before stopping

    public float leftLimit = -5f; // Movement boundary (left)
    public float rightLimit = 5f; // Movement boundary (right)
    public float bottomLimit = 2f; // Movement boundary (bottom)
    public float topLimit = 6f; // Movement boundary (top)

    private EnemySpawner _enemySpawner;
    private HandController _handController;
    private Vector3 targetPosition;
    private bool isMoving = true;

    void Start()
    {
        _handController = FindObjectOfType<HandController>();
        _enemySpawner = FindObjectOfType<EnemySpawner>();

        StartCoroutine(MoveAndStopRoutine());
    }

    IEnumerator MoveAndStopRoutine()
    {
        while (true)
        {
            // Move for some time
            isMoving = true;
            SetNewTargetPosition();

            yield return new WaitForSeconds(moveTime);

            // Stop for a while
            isMoving = false;
            yield return new WaitForSeconds(waitTime);
        }
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                SetNewTargetPosition();
            }
        }
    }
    private void OnMouseDown()
    {
        _handController.StartCoroutine(_handController.UncontrollableHand());
        Destroy(gameObject);
        _enemySpawner.RemoveEnemyBall();
    }

    void SetNewTargetPosition()
    {
        targetPosition = new Vector3(
            Random.Range(leftLimit, rightLimit),
            Random.Range(bottomLimit, topLimit), 0);
    }
}
