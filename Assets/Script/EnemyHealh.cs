using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealh : MonoBehaviour
{
    public GameObject _hitParticle;
    public GameObject currentSpriteObject;
    public Sprite enemy_DamagedSprite;

    public bool isTutorial = true;

    public float _enemyHealth = 2f;
    public float currentHealth;

    private AnimationManager _enemyAnim;
    private SpriteRenderer spriteRenderer;
    private LevelTransition _levelTransition;

    // Start is called before the first frame update
    void Start()
    {
        _enemyAnim = FindObjectOfType<AnimationManager>();
        _levelTransition = FindObjectOfType<LevelTransition>();

        currentHealth = _enemyHealth;

        if (currentSpriteObject != null)
        {
            spriteRenderer = currentSpriteObject.GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeSprite()
    {
        StartCoroutine(GetHit());
    }

    public void DamageEnemy()
    {
        StartCoroutine(Damage());
    }

    public IEnumerator Damage()
    {
        yield return new WaitForSeconds(3.8f);

        _enemyAnim.ThunderShake();
        currentHealth -= 1;
        CheckEnemyHealth();
    }

    public IEnumerator GetHit()
    {
        yield return new WaitForSeconds(3.8f);
        _enemyAnim.MonsterGetHit();
        Instantiate(_hitParticle, transform.position, Quaternion.identity);

        if (spriteRenderer != null && enemy_DamagedSprite != null)
        {
            spriteRenderer.sprite = enemy_DamagedSprite;
            Debug.Log("Enemy sprite changed!");
        }
        else
        {
            Debug.LogError("SpriteRenderer or damagedSprite is missing!");
        }
    }

    public void CheckEnemyHealth()
    {
        if (isTutorial && currentHealth == 1)
        {
            StartCoroutine(DelayOneHeart());
            //Next Scene
            _levelTransition.BackToForest();
        }
        else if (currentHealth == 1)
        {
            StartCoroutine(DelayOneHeart());
        }
        else if (currentHealth == 0)
        {
            StartCoroutine(DelayDied());
            //Win Scene
            Debug.Log("EnemyDied");
            _levelTransition.TheEnd();
        }
    }

    IEnumerator DelayOneHeart()
    {
        yield return new WaitForSeconds(1.5f);
        _enemyAnim.EnemyOneHeart();
    }

    IEnumerator DelayDied()
    {
        yield return new WaitForSeconds(1.5f);
        _enemyAnim.EnemyDied();
    }
}
