using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float _playerHealth = 2f;
    public float currentHealth;

    private AnimationManager _playerAnim;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = _playerHealth;

        _playerAnim = FindObjectOfType<AnimationManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DamagePlayer()
    {
        _playerAnim.EnemyFaceAttack();
        currentHealth -= 1;
        CheckPlayerHealth();
    }

    public void CheckPlayerHealth()
    {
        if(currentHealth == 1)
        {
            _playerAnim.PlayerOneHeart();
        }
        else if(currentHealth == 0)
        {
            _playerAnim.PlayerDied();
        }
    }
}
