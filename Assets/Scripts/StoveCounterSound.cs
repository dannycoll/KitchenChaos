using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{

  [SerializeField]
  private StoveCounter stoveCounter;
  private AudioSource audioSource;
  private float warningSoundTimer;
  private bool playWarningSound;

  private void Awake()
  {
    audioSource = GetComponent<AudioSource>();
  }

  private void Start()
  {
    stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
  }

  private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
  {
    float playSoundAmount = .5f;
    playWarningSound = stoveCounter.IsFried() && e.progressNormalized >= playSoundAmount;
  }

  private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
  {
    if (e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried)
    {
      audioSource.Play();
    }
    else
    {
      audioSource.Pause();
    }
  }

  private void Update()
  {
    if (playWarningSound)
    {

      warningSoundTimer -= Time.deltaTime;
      if (warningSoundTimer <= 0)
      {
        warningSoundTimer = .2f;
        SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
      }
    }
  }
}
