using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
  private Player player;
  private float footstepTimer = .1f;
  private float footstepDelay = .1f;

  private void Awake()
  {
    player = GetComponent<Player>();
  }

  private void Update()
  {
    footstepTimer -= Time.deltaTime;
    if (footstepTimer <= 0)
    {
      footstepTimer = footstepDelay;
      if (player.IsWalking())
      {
        SoundManager.Instance.PlayFootStepSound(transform.position);
      }
    }
  }
}

