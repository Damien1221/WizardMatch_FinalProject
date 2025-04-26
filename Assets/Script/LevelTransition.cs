using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public AudioSource _music;

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

    public void FinalFightScene()
    {
        StartCoroutine(FadeOutAudioAndChangeToFinalFight(0.1f));
    }

    public void TutorialScene()
    {
        StartCoroutine(FadeOutAudioAndChangeToTutorialScene(0.5f));
    }

    public void BackToForest()
    {
        StartCoroutine(FadeOutAudioAndChangeToForestScene(0.5f));
    }

    public void TheEnd()
    {
        StartCoroutine(FadeOutAudioAndChangeToEndingScene(0.5f));
    }

    public void ChasingScene()
    {
        _isPlay = true;
    }

    private IEnumerator FadeOutAudioAndChangeToFinalFight(float duration)
    {
        float startVolume = _music.volume;

        float t = 0;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime; // Unscaled time so it works even if game is paused
            _music.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        _music.volume = 0;
        LevelManager.Instance.LoadScene("GamePlay", "CrossFade");
    }

    private IEnumerator FadeOutAudioAndChangeToTutorialScene(float duration)
    {
        float startVolume = _music.volume;

        float t = 0;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime; // Unscaled time so it works even if game is paused
            _music.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        _music.volume = 0;
        LevelManager.Instance.LoadScene("TutorialScene", "CrossFade");
    }

    private IEnumerator FadeOutAudioAndChangeToForestScene(float duration)
    {
        float startVolume = _music.volume;

        float t = 0;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime; // Unscaled time so it works even if game is paused
            _music.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        _music.volume = 0;
        LevelManager.Instance.LoadScene("EnemyEscape", "CrossFade");
    }

    private IEnumerator FadeOutAudioAndChangeToEndingScene(float duration)
    {
        float startVolume = _music.volume;

        float t = 0;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime; // Unscaled time so it works even if game is paused
            _music.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        _music.volume = 0;
        LevelManager.Instance.LoadScene("EndingScene", "CrossFade");
    }
}
