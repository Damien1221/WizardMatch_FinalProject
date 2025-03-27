using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyBall;

    public float SpawnInterval = 1f;
    private float _spawnTimer = 0f;
    private AnimationManager enemy_Attack;

    // Start is called before the first frame update
    void Start()
    {
        enemy_Attack = FindObjectOfType<AnimationManager>();

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
}
