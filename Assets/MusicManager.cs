using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip[] sounds;
    [SerializeField] string stopMusicInSceneName; // Set this in Inspector

    AudioSource myAudioSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        Music();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == stopMusicInSceneName)
        {
            Destroy(gameObject); // Or: myAudioSource.Stop();
        }

    }

    public void Music()
    {
        AudioClip clip = sounds[Random.Range(0, sounds.Length)];
        myAudioSource.PlayOneShot(clip);
    }
}
