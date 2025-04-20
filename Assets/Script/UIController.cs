using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private AudioClip flipingPage_sfx;
    [SerializeField] private AudioClip CloseBook_sfx;

    public GameObject _menuPanel;
    public GameObject _pausePanel;
    public GameObject _tutorialPanel;
    public GameObject _PageOne;
    public GameObject _PageTwo;

    public GameObject _losePanel;

    public AudioSource _music;

    public float changeTime;
    public string sceneName;

    public string _mainMenu = "";
    public string _restartGame = "";

    private PlayableDirector timeline;
    private AudioSource _audioSource;
    private bool isPaused = false;
    private bool introPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        timeline = GetComponent<PlayableDirector>();

        if (_pausePanel == null)
            return;
        if (_tutorialPanel == null)
            return;
        if (_PageOne == null)
            return;
        if (_PageTwo == null)
            return;
        if (_mainMenu == null)
            return;
        if (_restartGame == null)
            return;
        if (_menuPanel == null)
            return;
        if (_losePanel == null)
            return;

        _losePanel.SetActive(false);
        _tutorialPanel.SetActive(false);
        _pausePanel.SetActive(false);
        _menuPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (introPlaying)
        {
            changeTime -= Time.deltaTime;

            if (changeTime <= 0)
            {
                introPlaying = false; // Prevent multiple triggers
                StartCoroutine(FadeOutAudioAndChangeScene(0.1f)); // Duration in seconds
            }
        }

    }

    private IEnumerator FadeOutAudioAndChangeScene(float duration)
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
        SceneManager.LoadScene(sceneName);
    }

    public void StartTutorial()
    {
        _tutorialPanel.SetActive(true);
        PauseGame();
        _audioSource.clip = flipingPage_sfx;
        _audioSource.Play();
    }

    public void EndTutorial()
    {
        _tutorialPanel.SetActive(false);
        ResumeGame();
        _audioSource.clip = CloseBook_sfx;
        _audioSource.Play();
    }

    public void NextPage()
    {
        _PageOne.SetActive(false);
        _PageTwo.SetActive(true);
        _audioSource.clip = flipingPage_sfx;
        _audioSource.Play();
    }

    public void BackPage()
    {
        _PageOne.SetActive(true);
        _PageTwo.SetActive(false);
        _audioSource.clip = flipingPage_sfx;
        _audioSource.Play();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0; // Pause the game
            _pausePanel.SetActive(true);
            Debug.Log("Game Paused");
        }
        else
        {
            Time.timeScale = 1; // Resume the game
            _pausePanel.SetActive(false);
            Debug.Log("Game Resumed");
        }
    }

    public void PlayIntro()
    {
        introPlaying = true;
        _menuPanel.SetActive(false);
        timeline.Play();

    }

    public void PauseGame()
    {
        Time.timeScale = 0; // Pause the game
        Debug.Log("Game Paused");
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1; // Resume the game
        _pausePanel.SetActive(false);
        Debug.Log("Game Resumed");
    }

    public void BackToMenu()
    {
        ResumeGame();
        SceneManager.LoadScene(_mainMenu);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(_restartGame);
        ResumeGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoseScreen()
    {
        Time.timeScale = 0; 
        _losePanel.SetActive(true);
    }
}
