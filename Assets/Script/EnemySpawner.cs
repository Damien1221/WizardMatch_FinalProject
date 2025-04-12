using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject FlyingEnemyBall;
    public GameObject enemyBall;

    public float SpawnInterval = 1f;
    public float EnemySpawnInterval = 2f;
    public int maxEnemyBalls = 3;

    private int enemyBallCount = 0;
    private float _spawnTimer = 0f;
    private AnimationManager enemy_Attack;

    protected GameObject currentObj;

    // Start is called before the first frame update
    void Start()
    {
        enemy_Attack = FindObjectOfType<AnimationManager>();

        StartCoroutine(SpawnEnemyBalls());
        _spawnTimer = SpawnInterval;
    }

    void Update()
    {
        if (_spawnTimer > 0)
        {
            _spawnTimer -= Time.deltaTime;
        }
        else
        {
            SpawnAttack();
            enemy_Attack.MonsterAttack();
            _spawnTimer = SpawnInterval;
        }
    }

    public void SpawnAttack()
    {
        Vector3 randomSpawnPosition = new Vector3(Random.Range(-7.2f, -4.0f), 3.2f, 0);
        GameObject.Instantiate(enemyBall, randomSpawnPosition, Quaternion.identity);
    }

    IEnumerator SpawnEnemyBalls()
    {
        Vector3 randomSpawnPosition = new Vector3(Random.Range(-0.6f, 7.7f), Random.Range(3f, 2.6f), 0);
        while (enemyBallCount < maxEnemyBalls)
        {
            currentObj = Instantiate(FlyingEnemyBall, randomSpawnPosition, Quaternion.identity);
            currentObj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            enemyBallCount++;

            yield return new WaitForSeconds(EnemySpawnInterval);
        }
    }

    public void SpawningOneEnemyBall()
    {
        Vector3 randomSpawnPosition = new Vector3(Random.Range(-0.6f, 7.7f), Random.Range(3f, 2.6f), 0);

        currentObj = Instantiate(FlyingEnemyBall, randomSpawnPosition, Quaternion.identity);
        currentObj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
    }

    public void RemoveEnemyBall()
    {
        enemyBallCount = Mathf.Max(0, enemyBallCount - 1); // Decrease count safely
    }
}
