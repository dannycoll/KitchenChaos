using UnityEngine;

public class SoundManager : MonoBehaviour
{
  private const string PlayerPrefsSfxVolumeKey = "SoundEffectsVolume";
  public static SoundManager Instance;
  [SerializeField]
  private AudioClipRefsSO audioClipRefsSO;

  private float _volume = .1f;
  private void Awake()
  {
    Instance = this;
    _volume = PlayerPrefs.GetFloat(PlayerPrefsSfxVolumeKey, _volume);
  }
  private void Start()
  {
    DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
    DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
    CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
    Player.Instance.OnPickedUpKitchenObject += Player_OnPickedUpKitchenObject;
    BaseCounter.OnAnyObjectPlaced += BaseCounter_OnAnyObjectPlaced;
    TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
  }

  private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
  {
    PlaySound(audioClipRefsSO.trash, ((TrashCounter)sender).transform.position);
  }
  private void BaseCounter_OnAnyObjectPlaced(object sender, System.EventArgs e)
  {
    PlaySound(audioClipRefsSO.objectDrop, ((BaseCounter)sender).transform.position);
  }

  private void Player_OnPickedUpKitchenObject(object sender, System.EventArgs e)
  {
    PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);

  }
  private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
  {
    CuttingCounter cuttingCounter = (CuttingCounter)sender;
    PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);

  }
  private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
  {
    if (Camera.main != null) PlaySound(audioClipRefsSO.deliveryFail, Camera.main.transform.position);
  }
  private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
  {
    if (Camera.main != null) PlaySound(audioClipRefsSO.deliverySuccess, Camera.main.transform.position);
  }
  private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
  {
    AudioSource.PlayClipAtPoint(audioClip, position, volume);
  }
  private void PlaySound(AudioClip[] audioClips, Vector3 position, float volume = 1f)
  {
    AudioSource.PlayClipAtPoint(audioClips[Random.Range(0, audioClips.Length)], position, volume);
  }

  public void PlayFootStepSound(Vector3 position, float volumeMultiplier = 1f)
  {
    PlaySound(audioClipRefsSO.footstep, position, _volume * volumeMultiplier);
  }

  public void PlayWarningSound(Vector3 position, float volumeMultiplier = 1f)
  {
    PlaySound(audioClipRefsSO.warning, position, _volume * volumeMultiplier);
  }

  public void PlayCountdownSound()
  {
    if (Camera.main != null) PlaySound(audioClipRefsSO.warning, Camera.main.transform.position);
  }

  public void ChangeVolume()
  {
    _volume = _volume + .1f;
    if (_volume > 1f)
    {
      _volume = 0;
    }

    PlayerPrefs.SetFloat(PlayerPrefsSfxVolumeKey, _volume);
    PlayerPrefs.Save();
  }
  public float GetVolume()
  {
    return _volume;
  }
}
