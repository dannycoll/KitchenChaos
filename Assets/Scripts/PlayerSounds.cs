using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
  private Player _player;
  private float _footstepTimer = .1f;
  private const float FootstepDelay = .1f;

  private void Awake()
  {
    _player = GetComponent<Player>();
  }

  private void Update()
  {
    _footstepTimer -= Time.deltaTime;
    if (_footstepTimer <= 0)
    {
      _footstepTimer = FootstepDelay;
      if (_player.IsWalking())
      {
        SoundManager.Instance.PlayFootStepSound(transform.position);
      }
    }
  }
}

