using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBall : MonoBehaviour
{
    public GameObject spawnEffectPrefab; // Effect when spell is first cast
    public GameObject clickEffectPrefab; // Effect when clicked

    private GameObject activeClickEffect; // Track click effect
    private GameObject activeSpawnEffect;
    private bool isClicked = false;
    public void SpawnEffect()
    {
        if (spawnEffectPrefab != null)
        {
            activeSpawnEffect = Instantiate(spawnEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    public void OnClick()
    {
        if (!isClicked && clickEffectPrefab != null)
        {
            activeClickEffect = Instantiate(clickEffectPrefab, transform.position, Quaternion.identity);
            isClicked = true; // Mark as clicked so effect happens only once
        }
    }
    public void DestroyEnemy()
    {
        if (activeSpawnEffect != null)
        {
            Destroy(activeSpawnEffect); // Remove spawn effect
        }
        if (activeClickEffect != null)
        {
            Destroy(activeClickEffect); // Remove click effect
        }

        Destroy(gameObject); // Destroy the enemy ball
    }

}
