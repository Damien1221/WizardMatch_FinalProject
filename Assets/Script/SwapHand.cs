using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapHand : MonoBehaviour
{
    public GameObject hand1;
    public GameObject hand2;

    public GameObject hand1_Shadow;

    private GameObject activeHand;
    private bool canSwap = true; // Set to false to disable swapping

    // Start is called before the first frame update
    void Start()
    {
        activeHand = hand1; // Set the default active hand
        hand1.SetActive(true);
        hand2.SetActive(false);

        hand1_Shadow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (canSwap && Input.GetKeyDown(KeyCode.Space))
        {
            SwapHands();
        }
    }

    public void EnableSwapping()
    {
        canSwap = true;
        FlyingBall.EnableToggle();
    }

    public void DisableSwapping()
    {
        canSwap = false;
        FlyingBall.DisableToggle();
    }

    void SwapHands()
    {
        // Toggle active hand
        if (activeHand == hand1)
        {
            activeHand.SetActive(false);
            hand1_Shadow.SetActive(true);
            activeHand = hand2;
        }
        else
        {
            activeHand.SetActive(false);
            hand1_Shadow.SetActive(false);
            activeHand = hand1;
        }
        activeHand.SetActive(true);
    }
}
