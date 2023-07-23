using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
  private const string PLAYER_PREFS_SFX_VOLUME_KEY = "SoundEffectsVolume";
  public static SoundManager Instance;
  [SerializeField]
  private AudioClipRefsSO audioClipRefsSO;

  private float volume = .1f;
  private void Awake()
  {
    Instance = this;
    volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SFX_VOLUME_KEY, volume);
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
    PlaySound(audioClipRefsSO.trash, (sender as TrashCounter).transform.position);
  }
  private void BaseCounter_OnAnyObjectPlaced(object sender, System.EventArgs e)
  {
    PlaySound(audioClipRefsSO.objectDrop, (sender as BaseCounter).transform.position);
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
    PlaySound(audioClipRefsSO.deliveryFail, Camera.main.transform.position);
  }
  private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
  {
    PlaySound(audioClipRefsSO.deliverySuccess, Camera.main.transform.position);
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
    PlaySound(audioClipRefsSO.footstep, position, volume * volumeMultiplier);
  }

  public void PlayWarningSound(Vector3 position, float volumeMultiplier = 1f)
  {
    PlaySound(audioClipRefsSO.warning, position, volume * volumeMultiplier);
  }

  public void PlayCountdownSound()
  {
    PlaySound(audioClipRefsSO.warning, Camera.main.transform.position);
  }

  public void ChangeVolume()
  {
    volume = volume + .1f;
    if (volume > 1f)
    {
      volume = 0;
    }

    PlayerPrefs.SetFloat(PLAYER_PREFS_SFX_VOLUME_KEY, volume);
    PlayerPrefs.Save();
  }
  public float GetVolume()
  {
    return volume;
  }
}
