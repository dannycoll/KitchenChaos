using UnityEngine;

public class MusicManager : MonoBehaviour
{
  private const string PlayerPrefsMusicVolume = "MusicVolume";

  public static MusicManager Instance;
  private AudioSource _audioSource;
  private float _volume = .3f;

  private void Awake()
  {
    Instance = this;
    _audioSource = GetComponent<AudioSource>();
    _volume = PlayerPrefs.GetFloat(PlayerPrefsMusicVolume, _volume);
    _audioSource.volume = _volume;
  }

  public void ChangeVolume()
  {
    _volume = _volume + .1f;
    if (_volume > 1f)
    {
      _volume = 0;
    }
    _audioSource.volume = _volume;
    PlayerPrefs.SetFloat(PlayerPrefsMusicVolume, _volume);
    PlayerPrefs.Save();
  }
  public float GetVolume()
  {
    return _volume;
  }
}
