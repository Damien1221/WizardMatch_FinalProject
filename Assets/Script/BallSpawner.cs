using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] ObjPrefab;

    public GameObject _assistBall;
    public GameObject _blockingHand;

    public int maxBalls = 5;

    public float _respawnAssistTime = 10f;

    private int ballCount = 0;

    protected GameObject currentObj;

    // Start is called before the first frame update
    void Start()
    {
        SpawnHand();

        SpawnAssistBall();

        SpawnAllBallsAtStart();
    }

    public void SpawnHand()
    {
        Vector3 Newposition = new Vector3(-11.72f, 3.46f, 0f);

        Instantiate(_blockingHand, Newposition, Quaternion.identity);
    }

    void SpawnAllBallsAtStart()
    {
        for (int i = 0; i < maxBalls; i++)
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-0.6f, 7.7f), Random.Range(2.6f, 3f), 0);
            currentObj = Instantiate(ObjPrefab[Random.Range(0, ObjPrefab.Length)], randomSpawnPosition, Quaternion.identity);
            currentObj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            ballCount++;
        }
    }

    public void SpawningOneBall()
    {
        Vector3 randomSpawnPosition = new Vector3(Random.Range(-0.6f, 7.7f), Random.Range(3f, 2.6f), 0);

        currentObj = Instantiate(ObjPrefab[Random.Range(0, ObjPrefab.Length)], randomSpawnPosition, Quaternion.identity);
        currentObj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
    }

    public void SpawnAssistBall()
    {
        float ySpawn = Random.Range(-2f, 2f); // Optional: Random vertical range
        Vector3 spawnPosition = new Vector3(11f, ySpawn, 0f); // Adjust X for screen size
        Instantiate(_assistBall, spawnPosition, Quaternion.identity);
    }

    public IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(_respawnAssistTime);

        // Re-spawn the ball from the right side
        float ySpawn = Random.Range(-2f, 2f); // Optional: Random vertical range
        Vector3 spawnPosition = new Vector3(11f, ySpawn, 0f); // Adjust X for screen size
        Instantiate(_assistBall, spawnPosition, Quaternion.identity);
    }
}

