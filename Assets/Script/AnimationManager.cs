using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AnimationManager : MonoBehaviour
{
    public GameObject _EnemyFace;

    public Animator camShake;
    public Animator camShakingHard;
    public Animator Ultimate_Effect;
    public Animator Hand;
    public Animator _ThunderEffect;
    public Animator _monster_Attack;
    public Animator _monster_Hit;
    public Animator _flyingEnemy;

    public Animator _playerHealth;
    public Animator _enemyHealth;
    public Animator _enemyFace;

    public PlayableDirector _enemyEsacpe;

    private LevelTransition _nextScene;

    void Start()
    {
        _EnemyFace.SetActive(false);

        _nextScene = FindObjectOfType<LevelTransition>(); //  this will have bug later

        if(_flyingEnemy == null)
        {
            //_flyingEnemy = .Find("Icon").gameObject;
        }
    }

    public void PlayerOneHeart()
    {
        _playerHealth.Play("PlayerHeart_LeftOneHeart");
    }

    public void PlayerDied()
    {
        _playerHealth.Play("PlayerHeart_NoMoreHeart");
    }

    public void EnemyOneHeart()
    {
        _enemyHealth.Play("EnemyHeart_LeftOne");
    }

    public void EnemyDied()
    {
        _enemyHealth.Play("EnemyHeart_NoMoreLeft");
    }

    public void EnemyFaceAttack()
    {
        _EnemyFace.SetActive(true);
        _enemyFace.Play("EnemyFace_Activate");
        StartCoroutine(ActiveTrigger());
    }

    public IEnumerator ActiveTrigger()
    {
        yield return new WaitForSeconds(2f);
        _EnemyFace.SetActive(false);

    }

    public void FlyingEnemyLaugh()
    {
        _flyingEnemy.SetTrigger("Enemy_Laughing");
    }

    public void TriggerScene()
    {
        _enemyEsacpe.Play();
        _nextScene.ChasingScene();
    }

    public void CameraShake()
    {
        camShake.SetTrigger("Camera_Shake");
    }

    public void ThunderShake()
    {
        camShake.SetTrigger("Thunder_Shake");
    }

    public void CameraShakingHard()
    {
        camShakingHard.SetBool("Camera_ShakingHard", true);
    }

    public void CameraStopShaking()
    {
        camShakingHard.SetBool("Camera_ShakingHard", false);
    }

    public void PlayUltimate()
    {
        Ultimate_Effect.SetTrigger("PlayUltimate");
    }

    public void OpeningHand()
    {
        Hand.SetBool("Closing_Hand", false);
    }

    public void OpeningWideHand()
    {
        Hand.SetBool("Opening_Wide", true);
    }

    public void ClosingWideHand()
    {
        Hand.SetBool("Opening_Wide", false);
    }

    public void ClosingHand()
    {
        Hand.SetBool("Closing_Hand", true);
    }

    public void MonsterAttack()
    {
        _monster_Attack.SetTrigger("Monster_Attacking");
    }
    public void MonsterGetHit()
    {
        _monster_Hit.SetTrigger("Monster_GetHit");
    }

    public void ThunderEffect()
    {
        _ThunderEffect.SetTrigger("Thunder");
    }
}
