using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float _playerHealth = 2f;
    public float currentHealth;

    private AnimationManager _playerAnim;
    private UIController _uiController;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = _playerHealth;

        _playerAnim = FindObjectOfType<AnimationManager>();
        _uiController = FindObjectOfType<UIController>();
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
            StartCoroutine(DelayOneHeart());
        }
        else if(currentHealth == 0)
        {
            StartCoroutine(DelayDied());
        }
    }

    IEnumerator DelayOneHeart()
    {
        yield return new WaitForSeconds(1.5f);
        _playerAnim.PlayerOneHeart();
    }

    IEnumerator DelayDied()
    {
        yield return new WaitForSeconds(1.5f);
        _playerAnim.PlayerDied();
        _uiController.LoseScreen();
    }
}
