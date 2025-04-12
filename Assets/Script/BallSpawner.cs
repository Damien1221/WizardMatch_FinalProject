using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] ObjPrefab;

    public GameObject _manaTarget;
    public float spawnInterval = 2f;
    public int maxBalls = 5;

    private int ballCount = 0;

    protected GameObject currentObj;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnBalls());

        SpawnManaTarget();
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator SpawnBalls()
    {
        Vector3 randomSpawnPosition = new Vector3(Random.Range(-0.6f, 7.7f), Random.Range(3f, 2.6f), 0);
        while (ballCount < maxBalls)
        {
            currentObj = Instantiate(ObjPrefab[Random.Range(0, ObjPrefab.Length)], randomSpawnPosition, Quaternion.identity);
            currentObj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            ballCount++;

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void SpawningOneBall()
    {
        Vector3 randomSpawnPosition = new Vector3(Random.Range(-0.6f, 7.7f), Random.Range(3f, 2.6f), 0);

        currentObj = Instantiate(ObjPrefab[Random.Range(0, ObjPrefab.Length)], randomSpawnPosition, Quaternion.identity);
        currentObj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
    }

    public void SpawnManaTarget()
    {
        Vector3 Newposition = new Vector3(7.69f, -1.8f, 0);

        Instantiate(_manaTarget, Newposition, Quaternion.identity);
    }   
}

