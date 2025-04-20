using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComboSystem : MonoBehaviour
{
    public static ComboSystem instance; // Singleton for easy access

    public AudioSource audioSource;
    public AudioClip comboSound;
    public float basePitch = 1f;
    public float pitchStep = 0.1f; // How much pitch increases per combo
    public float maxPitch = 2f;    // Cap pitch to avoid going too high

    public TMP_Text comboText;
    public float comboResetTime = 5f;

    private int comboCount = 0;
    private Coroutine resetComboCoroutine;
    

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateComboUI();
    }

    public int GetComboCount()
    {
        return comboCount;
    }

    public void IncreaseCombo()
    {
        comboCount++;
        UpdateComboUI();

        PlayComboSound();

        if (resetComboCoroutine != null)
        {
            StopCoroutine(resetComboCoroutine);
        }

        resetComboCoroutine = StartCoroutine(ResetComboAfterDelay());
    }

    private void PlayComboSound()
    {
        if (audioSource != null && comboSound != null)
        {
            audioSource.pitch = Mathf.Min(basePitch + comboCount * pitchStep, maxPitch);
            audioSource.PlayOneShot(comboSound);
        }
    }

    private IEnumerator ResetComboAfterDelay()
    {
        yield return new WaitForSeconds(comboResetTime);

        // Reset combo if no new balls are destroyed in 5 seconds
        comboCount = 0;
        UpdateComboUI();
    }

    private void UpdateComboUI()
    {
        if (comboText != null)
        {
            comboText.text = "Combo: " + "x" + comboCount;
        }
        Debug.Log("Current Combo: " + comboCount);
    }
}
