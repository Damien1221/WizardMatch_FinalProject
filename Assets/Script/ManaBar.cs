using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public static ManaBar instance;

    public Slider manaBar;
    public float currentMana = 0f;
    public float maxMana = 20f;
    public float fillSpeed = 0.5f;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        manaBar.maxValue = maxMana; 
        manaBar.value = currentMana; 
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    public void AddMana(float amount)
    {
        currentMana += amount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana); // Prevent overflow
        StartCoroutine(SmoothFill()); // Start the smooth animation
        Debug.Log("Mana Added: " + amount + " | Current Mana: " + currentMana);
    }
    private IEnumerator SmoothFill()
    {
        float startValue = manaBar.value;
        float targetValue = currentMana;
        float elapsedTime = 0f;

        while (elapsedTime < fillSpeed)
        {
            elapsedTime += Time.deltaTime;
            manaBar.value = Mathf.Lerp(startValue, targetValue, elapsedTime / fillSpeed);
            yield return null; // Wait for the next frame
        }

        manaBar.value = targetValue; // Ensure it reaches the exact value
    }


}
