using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ManaParticle : MonoBehaviour
{
    public Transform target; // The mana bar
    public float speed = 5f;
    public float delay = 1f;

    private float timer = 0f;
    private bool canMove = false;
    private bool hasStartedScaling = false;

    void Update()
    {
        if (target == null) return;

        // Wait for delay
        if (!canMove)
        {
            timer += Time.deltaTime;
            if (timer >= delay)
            {
                canMove = true;
            }
            return;
        }

        // Move toward the target
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Shrink and destroy when close
        if (!hasStartedScaling && Vector3.Distance(transform.position, target.position) < 2f)
        {
            hasStartedScaling = true;

            transform
                .DOScale(Vector3.zero, 1f)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() => Destroy(gameObject));
        }
    }
}
