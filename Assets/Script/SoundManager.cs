using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip[] _correctSpellSFX;
    [SerializeField] private AudioClip _wrongSpellSFX;
    [SerializeField] private AudioClip _thunderSFX;
    [SerializeField] private AudioClip _shockingSFX;
    [SerializeField] private AudioClip _breakMagicRing;

    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CorrectSpellSFX()
    {
        _audioSource.clip = _correctSpellSFX[Random.Range(0, _correctSpellSFX.Length)];
        _audioSource.Play();
    }

    public void WrongSpellSFX()
    {
        _audioSource.clip = _wrongSpellSFX;
        _audioSource.Play();
    }

    public void PlayThunder()
    {
        _audioSource.clip = _thunderSFX;
        _audioSource.Play();
    }

    public void PlayShocking()
    {
        _audioSource.clip = _shockingSFX;
        _audioSource.Play();
    }

    public void PlayBreakMagicRing()
    {
        _audioSource.clip = _breakMagicRing;
        _audioSource.Play();
    }
}
