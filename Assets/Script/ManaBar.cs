using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public static ManaBar instance;

    public Slider manaBar;
    public Slider green_ManaBar;

    public float currentMana = 0f;
    public float currentGreenMana = 0f;

    public float maxMana = 20f;
    public float maxGreenMana = 20f;
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

        green_ManaBar.maxValue = maxGreenMana;
        green_ManaBar.value = currentGreenMana;
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    public void AddMana(float amount)
    {
        currentMana += amount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana); // Prevent overflow
        StartCoroutine(SmoothFill(currentMana)); // Start the smooth animation
        Debug.Log("Mana Added: " + amount + " | Current Mana: " + currentMana);

        if(currentMana > 20)
        {
            currentMana = 20;
        }
    }

    public void AddGreenMana(float amount)
    {
        currentGreenMana += amount;
        currentGreenMana = Mathf.Clamp(currentGreenMana, 0, maxGreenMana); // Prevent overflow
        StartCoroutine(GreenSmoothFill(currentGreenMana)); // Start the smooth animation
        Debug.Log("Mana Added: " + amount + " | Current Mana: " + currentGreenMana);

        if (currentGreenMana > 20)
        {
            currentGreenMana = 20;
        }
    }

    public void UsedMana(float amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            StartCoroutine(SmoothFill(currentMana)); // Smooth decrease
            Debug.Log("Mana Used: " + amount + " | Remaining Mana: " + currentMana);
        }
        else
        {
            Debug.Log("Not enough mana!");
        }
    }

    public void UsedGreenMana(float amount)
    {
        if (currentGreenMana >= amount)
        {
            currentGreenMana -= amount;
            StartCoroutine(GreenSmoothFill(currentGreenMana)); // Smooth decrease
            Debug.Log("Mana Used: " + amount + " | Remaining Mana: " + currentGreenMana);
        }
        else
        {
            Debug.Log("Not enough mana!");
        }
    }


    private IEnumerator SmoothFill(float targetValue)
    {
        float startValue = manaBar.value;
        float elapsedTime = 0f;

        while (elapsedTime < fillSpeed)
        {
            elapsedTime += Time.deltaTime;
            manaBar.value = Mathf.Lerp(startValue, targetValue, elapsedTime / fillSpeed);
            yield return null;
        }

        manaBar.value = targetValue;
    }

    private IEnumerator GreenSmoothFill(float targetValue)
    {
        float startValue = green_ManaBar.value;
        float elapsedTime = 0f;

        while (elapsedTime < fillSpeed)
        {
            elapsedTime += Time.deltaTime;
            green_ManaBar.value = Mathf.Lerp(startValue, targetValue, elapsedTime / fillSpeed);
            yield return null;
        }

        green_ManaBar.value = targetValue;
    }

}
