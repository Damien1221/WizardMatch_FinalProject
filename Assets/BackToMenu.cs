using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    public Animator _animator;

    private bool _Isbutton = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_Isbutton)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject)
        {
            Debug.Log("Enter Liao");
            //start countdown
            StartCountdown();
        }
        
    }

    public void StartCountdown()
    {
        StartCoroutine(GoBackToMenu());
    }

    IEnumerator GoBackToMenu()
    {
        yield return new WaitForSeconds(5f);
        _animator.SetTrigger("Appear");
        _Isbutton = true;

        yield return new WaitForSeconds(30f);
        SceneManager.LoadScene("MainMenu");

    }
}
