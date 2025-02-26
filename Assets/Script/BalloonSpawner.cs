using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BalloonSpawner : MonoBehaviour
{
    public GameObject balloonPrefab;
    public Transform[] spawnPoints;
    public Sprite[] shapeSprites; // Array of shape sprites

    private List<Balloon> activeBalloons = new List<Balloon>();
    private string[] shapeNames = { "Circle", "Square", "Triangle", "ArrowUp", "Love" };

    void Start()
    {
        SpawnBalloon();
    }

    void SpawnBalloon()
    {
        ClearBalloons();

        for (int i = 0; i < spawnPoints.Length && i < 5; i++) // Spawn up to 5 balloons
        {
            GameObject balloonObject = Instantiate(balloonPrefab, spawnPoints[i].position, Quaternion.identity);
            Balloon balloon = balloonObject.GetComponent<Balloon>();

            string selectedShape = shapeNames[Random.Range(0, shapeNames.Length)]; // Pick a random shape
            balloon.shapeSprites = shapeSprites; // Assign shape sprites from inspector
            balloon.SetShape(selectedShape); // Set shape & sprite
            activeBalloons.Add(balloon);
        }
    }

    public void DestroyBalloonByShape(string shapeName)
    {
        for (int i = activeBalloons.Count - 1; i >= 0; i--)
        {
            if (activeBalloons[i] != null && activeBalloons[i].shapeName == shapeName)
            {
                Destroy(activeBalloons[i].gameObject);
                activeBalloons.RemoveAt(i);
            }
        }
    }

    private void ClearBalloons()
    {
        foreach (Balloon balloon in activeBalloons)
        {
            if (balloon != null)
            {
                Destroy(balloon.gameObject);
            }
        }
        activeBalloons.Clear();
    }
}
