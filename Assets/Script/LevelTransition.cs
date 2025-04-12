using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string To_Scene;
    public float changeTime;

    private bool _isPlay = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlay == true)
        {
            changeTime -= Time.deltaTime;

            if (changeTime <= 0)
            {
                SceneManager.LoadScene(To_Scene);
            }
        }

    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(To_Scene);
        }
    }

    public void FightScene()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void ChasingScene()
    {
        _isPlay = true;
    }
}
