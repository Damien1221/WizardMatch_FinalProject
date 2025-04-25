using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject FlyingEnemyBall;
    public GameObject enemyBall;
    public GameObject enemyball_effect;

    public bool enableFlyingEnemies = true;

    public float SpawnInterval = 1f;
    public float EnemySpawnInterval = 2f; //flyingball
    public int maxEnemyBalls = 3;

    private bool isAttacking = true;
    private int enemyBallCount = 0;
    private float _spawnTimer = 0f;
    private bool hasTriggeredEvilRing = false;

    private AnimationManager enemy_Attack; // spinning head
    private EnemySpell _enemySpell;
    private ManaBar _manabar;

    protected GameObject currentObj;

    // Start is called before the first frame update
    void Start()
    {
        enemy_Attack = FindObjectOfType<AnimationManager>();
        _enemySpell = FindObjectOfType<EnemySpell>();
        _manabar = FindObjectOfType<ManaBar>();

        if (enableFlyingEnemies)
        {
            StartCoroutine(SpawnEnemyBalls());
        }

        _spawnTimer = SpawnInterval;
    }

    void Update()
    {
        if (!isAttacking) return; // Exit early if attack is disabled

        if (_spawnTimer > 0)
        {
            _spawnTimer -= Time.deltaTime;
        }
        else
        {
            int rand = Random.Range(0, 2); // 0 or 1

            switch (rand)
            {
                case 0:
                    SpawnAttack();
                    enemy_Attack.MonsterAttack();
                    _spawnTimer = SpawnInterval;
                    break;

                case 1:
                    if (_enemySpell != null && !hasTriggeredEvilRing && _manabar.currentGreenMana >= 6f)
                    {
                        hasTriggeredEvilRing = true;

                        _enemySpell.ActivateEvilRing();
                        enemy_Attack.MonsterAttack();
                        _spawnTimer = 20f;
                    }
                    else if (_enemySpell != null && hasTriggeredEvilRing)
                    {
                        _enemySpell.ActivateEvilRing();
                        enemy_Attack.MonsterAttack();
                        _spawnTimer = 20f;
                    }
                    break;
            }
        }
    }

    public void StopEnemyAttack()
    {
        isAttacking = false;
    }

    public void ContinueEnemyAttack()
    {
        isAttacking = true;
    }

    public void SpawnAttack() //enemy dropping ball
    {
        StartCoroutine(DelaySpawn());
    }

    IEnumerator DelaySpawn()
    {
        Vector3 randomSpawnPosition = new Vector3(Random.Range(-7.2f, -4.0f), 3.2f, 0);
        Instantiate(enemyball_effect, randomSpawnPosition, Quaternion.identity);

        yield return new WaitForSeconds(1f);
        GameObject.Instantiate(enemyBall, randomSpawnPosition, Quaternion.identity);
    }

    IEnumerator SpawnEnemyBalls()
    {
        // Initial delay
        yield return new WaitForSeconds(2f);

        while (true)
        {
            if (enemyBallCount < maxEnemyBalls)
            {
                Vector3 randomSpawnPosition = new Vector3(Random.Range(-0.6f, 7.7f), Random.Range(2.6f, 3f), 0);
                currentObj = Instantiate(FlyingEnemyBall, randomSpawnPosition, Quaternion.identity);
                currentObj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
                enemyBallCount++;
            }

            yield return new WaitForSeconds(EnemySpawnInterval); // Wait before checking again
        }
    }

    public void RemoveEnemyBall()
    {
        enemyBallCount = Mathf.Max(0, enemyBallCount - 1); // Decrease count safely
    }
}
