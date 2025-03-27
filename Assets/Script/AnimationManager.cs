using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator camShake;
    public Animator camShakingHard;
    public Animator Ultimate_Effect;
    public Animator Hand;
    public Animator _monster_Attack;
    public void CameraShake()
    {
        camShake.SetTrigger("Camera_Shake");
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
}
