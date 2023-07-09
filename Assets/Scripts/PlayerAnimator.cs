using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
  private const string IS_WALKING = "IsWalking";
  private Animator _animator;
  [SerializeField] private Player player;

  private void Awake()
  {
    _animator = GetComponent<Animator>();

  }

  private void Update()
  {
    _animator.SetBool(IS_WALKING, player.IsWalking());
  }


}
