using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator camShake;

    public void CameraShake()
    {
        camShake.SetTrigger("Camera_Shake");
    }
}
