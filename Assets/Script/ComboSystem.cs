using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComboSystem : MonoBehaviour
{
    public static ComboSystem instance; // Singleton for easy access

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

    public void IncreaseCombo()
    {
        comboCount++;
        UpdateComboUI();

        // Reset the previous reset timer
        if (resetComboCoroutine != null)
        {
            StopCoroutine(resetComboCoroutine);
        }

        // Start a new reset timer
        resetComboCoroutine = StartCoroutine(ResetComboAfterDelay());
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
