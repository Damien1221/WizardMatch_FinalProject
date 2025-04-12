using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KillZone : MonoBehaviour
{
    private ManaBar _manabar;
    private UIController _uiController;
    private float addedMana = 6f;
    private bool hasTutorialStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        _manabar = FindObjectOfType<ManaBar>();
        _uiController = FindObjectOfType<UIController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasTutorialStarted && (_manabar.currentMana >= 6 || _manabar.currentGreenMana >= 6 || _manabar.currentBlueMana >= 6))
        {
            _uiController.StartTutorial();
            hasTutorialStarted = true;
        }

        if (Input.GetKeyDown("q"))
        {
            ManaBar.instance.AddMana(addedMana);
        }
        else if(Input.GetKeyDown("w"))
        {
            ManaBar.instance.AddGreenMana(addedMana);
        }
        else if (Input.GetKeyDown("e"))
        {
            ManaBar.instance.AddBlueMana(addedMana);
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(col.gameObject);

    }
}
