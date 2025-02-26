using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator camShake;
    public Animator Ultimate_Effect;
    public void CameraShake()
    {
        camShake.SetTrigger("Camera_Shake");
    }

    public void PlayUltimate()
    {
        Ultimate_Effect.SetTrigger("PlayUltimate");
    }
}
