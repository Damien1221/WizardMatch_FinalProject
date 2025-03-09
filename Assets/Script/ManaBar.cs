using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public static ManaBar instance;

    public Slider manaBar;
    public Slider green_ManaBar;
    public Slider blue_ManaBar;

    public float currentMana = 0f;
    public float currentGreenMana = 0f;
    public float currentBlueMana = 0f;

    public float maxMana = 20f;
    public float maxGreenMana = 20f;
    public float maxBlueMana = 20f;

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

        blue_ManaBar.maxValue = maxBlueMana;
        blue_ManaBar.value = currentBlueMana;
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
        Debug.Log("Green_Mana Added: " + amount + " | Current Green_Mana: " + currentGreenMana);

        if (currentGreenMana > 20)
        {
            currentGreenMana = 20;
        }
    }

    public void AddBlueMana(float amount)
    {
        currentBlueMana += amount;
        currentBlueMana = Mathf.Clamp(currentBlueMana, 0, maxBlueMana); // Prevent overflow
        StartCoroutine(BlueSmoothFill(currentBlueMana)); // Start the smooth animation
        Debug.Log("Blue_Mana Added: " + amount + " | Current Blue_Mana: " + currentBlueMana);

        if (currentBlueMana > 20)
        {
            currentBlueMana = 20;
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
            Debug.Log("Green_Mana Used: " + amount + " | Remaining Green_Mana: " + currentGreenMana);
        }
        else
        {
            Debug.Log("Not enough Green_mana!");
        }
    }

    public void UsedBlueMana(float amount)
    {
        if (currentBlueMana >= amount)
        {
            currentBlueMana -= amount;
            StartCoroutine(BlueSmoothFill(currentBlueMana)); // Smooth decrease
            Debug.Log("Blue_Mana Used: " + amount + " | Remaining Blue_Mana: " + currentBlueMana);
        }
        else
        {
            Debug.Log("Not enough Blue_mana!");
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

    private IEnumerator BlueSmoothFill(float targetValue)
    {
        float startValue = blue_ManaBar.value;
        float elapsedTime = 0f;

        while (elapsedTime < fillSpeed)
        {
            elapsedTime += Time.deltaTime;
            blue_ManaBar.value = Mathf.Lerp(startValue, targetValue, elapsedTime / fillSpeed);
            yield return null;
        }

        blue_ManaBar.value = targetValue;
    }

}
