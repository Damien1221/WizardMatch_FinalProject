using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BalloonSpawner : MonoBehaviour
{
    public GameObject balloonPrefab;
    public Transform spawnArea;
    private List<Balloon> activeBalloons = new List<Balloon>();

    void Start()
    {
        InvokeRepeating("SpawnBalloon", 1f, 2f); // Spawns balloons every 2 seconds
    }

    void SpawnBalloon()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(-5f, 5f),
            spawnArea.position.y,
            0f
        );

        GameObject balloonObject = Instantiate(balloonPrefab, spawnPosition, Quaternion.identity);
        Balloon balloon = balloonObject.GetComponent<Balloon>();
        activeBalloons.Add(balloon);
    }

    public void DestroyBalloonByShape(string shapeName)
    {
        Balloon balloonToDestroy = activeBalloons.Find(balloon => balloon.shapeName == shapeName);

        if (balloonToDestroy != null)
        {
            activeBalloons.Remove(balloonToDestroy);
            Destroy(balloonToDestroy.gameObject);
        }
    }
}
