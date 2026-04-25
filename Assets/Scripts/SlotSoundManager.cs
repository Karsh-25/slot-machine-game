using UnityEngine;

public class SlotSoundManager : MonoBehaviour
{
    #region  Serialized Fields 

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;     // one-shot sounds
    [SerializeField] private AudioSource loopSource;    // looping sounds (reel spin)

    [Header("Sounds")]

    [Header("Handle")]
    [SerializeField] private AudioClip handlePull;

    [Header("Reel")]
    [SerializeField] private AudioClip reelSpinLoop;
    [SerializeField] private AudioClip reelStop;

    [Header("Result")]
    [SerializeField] private AudioClip jackpotSound;
    [SerializeField] private AudioClip StopSound;

    #endregion

    #region  Callable Functions 

    /// <summary>
    /// Play handle pull sound
    /// </summary>
    public void PlayHandle()
    {
        PlayOneShot(handlePull);
    }

    /// <summary>
    /// Start reel spinning loop
    /// </summary>
    public void StartReelLoop()
    {
        if (loopSource == null || reelSpinLoop == null)
            return;

        loopSource.clip = reelSpinLoop;
        loopSource.loop = true;
        loopSource.Play();
    }

    /// <summary>
    /// Stop reel spinning loop
    /// </summary>
    public void StopReelLoop()
    {
        if (loopSource == null)
            return;

        loopSource.Stop();
    }

    /// <summary>
    /// Play reel stop (call per reel)
    /// </summary>
    public void PlayReelStop()
    {
        PlayOneShot(reelStop);
    }

    /// <summary>
    /// Play jackpot sound
    /// </summary>
    public void PlayJackpot()
    {
        PlayOneShot(jackpotSound);
    }

    /// <summary>
    /// Play final Stop sound
    /// </summary>
    public void PlayStopSound()
    {
        PlayOneShot(StopSound);
    }

    #endregion

    #region  Core 

    private void PlayOneShot(AudioClip clip)
    {
        if (sfxSource == null || clip == null)
            return;

        sfxSource.PlayOneShot(clip);
    }

    #endregion
}