using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealh : MonoBehaviour
{
    public GameObject currentSpriteObject;
    public Sprite enemy_DamagedSprite;

    public float _enemyHealth = 2f;
    public float currentHealth;

    private AnimationManager _enemyAnim;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _enemyAnim = FindObjectOfType<AnimationManager>();

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
        if (currentHealth == 1)
        {
            Debug.Log("Enemy Health is 1");
            _enemyAnim.EnemyOneHeart();
        }
        else if (currentHealth == 0)
        {
            _enemyAnim.EnemyDied();
        }
    }
}
