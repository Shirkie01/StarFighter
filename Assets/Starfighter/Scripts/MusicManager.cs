using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private AudioClip[] clips;
    private int currentIndex;

    private AudioSource audioSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        clips = Resources.LoadAll<AudioClip>("Music");
        if (clips == null)
        {
            Debug.Log("No music found! Add some clips to the Resources/Music folder.");
            enabled = false;
            return;
        }

        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayTrack());
    }

    public void NextTrack()
    {
        currentIndex++;
        if (currentIndex > clips.Length - 1)
        {
            currentIndex = 0;
        }

        audioSource.clip = clips[currentIndex];
        audioSource.Play();
    }

    public void PreviousTrack()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = clips.Length - 1;
        }

        audioSource.clip = clips[currentIndex];
        audioSource.Play();
    }

    private IEnumerator PlayTrack()
    {
        currentIndex = Random.Range(0, clips.Length - 1);
        audioSource.clip = clips[currentIndex];
        audioSource.Play();

        while (true)
        {
            yield return new WaitForSeconds(audioSource.clip.length);
            NextTrack();
        }
    }
}