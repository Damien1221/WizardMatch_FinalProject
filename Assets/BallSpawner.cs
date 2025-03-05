using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] ObjPrefab;

    public float SpawnInterval = 1f;
    private float _spawnTimer = 0f;

    protected GameObject currentObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_spawnTimer > 0)
        {
            _spawnTimer -= Time.deltaTime;
        }
        else
        {
            Spawning();
            _spawnTimer = SpawnInterval;
        }

    }
    public void Spawning()
    {
        Vector3 randomSpawnPosition = new Vector3(Random.Range(-0.6f, 7.7f), Random.Range(3f, 2.6f), 0);

        currentObj = Instantiate(ObjPrefab[Random.Range(0, ObjPrefab.Length)], randomSpawnPosition, Quaternion.identity);
        currentObj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
    }
}
