using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScene : MonoBehaviour
{
    private LevelTransition _levelTransition;

    // Start is called before the first frame update
    void Start()
    {
        _levelTransition = FindObjectOfType<LevelTransition>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject)
        {
            _levelTransition.ChasingScene();
        }
    }
}
