using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource sfxSource;

    [Header("Sons del joc")]
    public AudioClip startClip;
    public AudioClip clickClip;
    public AudioClip matchClip;
    public AudioClip mismatchClip;
    public AudioClip newBestClip;
    public AudioClip revealClip;

    void Start()
    {
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayStart() => Play(startClip);
    public void PlayClick() => Play(clickClip);
    public void PlayMatch() => Play(matchClip);
    public void PlayMismatch() => Play(mismatchClip);
    public void PlayNewBest() => Play(newBestClip);
    public void PlayReveal() => Play(revealClip);

    private void Play(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip);
    }
}