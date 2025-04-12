using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBall : MonoBehaviour
{
    public GameObject BlinkingEffect;

    void Start()
    {
        BlinkingEffect.SetActive(false);
    }

    public void SpawnEffect()
    {
        StartCoroutine(ActivateAndDeactivate());
    }

    IEnumerator ActivateAndDeactivate()
    {
        BlinkingEffect.SetActive(true); // Activate the object
        yield return new WaitForSeconds(5f); // Wait for 5 seconds
        BlinkingEffect.SetActive(false); // Deactivate the object
    }

}
