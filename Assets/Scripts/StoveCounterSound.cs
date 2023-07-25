using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{

  [SerializeField]
  private StoveCounter stoveCounter;
  private AudioSource _audioSource;
  private float _warningSoundTimer;
  private bool _playWarningSound;

  private void Awake()
  {
    _audioSource = GetComponent<AudioSource>();
  }

  private void Start()
  {
    stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
  }

  private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
  {
    float playSoundAmount = .5f;
    _playWarningSound = stoveCounter.IsFried() && e.ProgressNormalized >= playSoundAmount;
  }

  private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
  {
    if (e.State == StoveCounter.State.Frying || e.State == StoveCounter.State.Fried)
    {
      _audioSource.Play();
    }
    else
    {
      _audioSource.Pause();
    }
  }

  private void Update()
  {
    if (_playWarningSound)
    {

      _warningSoundTimer -= Time.deltaTime;
      if (_warningSoundTimer <= 0)
      {
        _warningSoundTimer = .2f;
        SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
      }
    }
  }
}
