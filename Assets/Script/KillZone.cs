using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KillZone : MonoBehaviour
{
    private float addedMana = 6f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("q"))
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
