using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator camShake;
    public Animator Ultimate_Effect;
    public Animator Hand;
    public void CameraShake()
    {
        camShake.SetTrigger("Camera_Shake");
    }

    public void PlayUltimate()
    {
        Ultimate_Effect.SetTrigger("PlayUltimate");
    }

    public void OpeningHand()
    {
        Hand.SetBool("Closing_Hand", false);
    }

    public void ClosingHand()
    {
        Hand.SetBool("Closing_Hand", true);
    }
}
