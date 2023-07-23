using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour
{
  private const string IS_FLASHING = "IsFlashing";
  [SerializeField] private StoveCounter stoveCounter;
  private Animator animator;

  private void Awake()
  {
    animator = GetComponent<Animator>();
    animator.SetBool(IS_FLASHING, false);
  }
  private void Start()
  {
    stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
  }

  private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
  {
    float burnShowProgressAmount = .5f;
    bool showBurnWarning = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;

    animator.SetBool(IS_FLASHING, showBurnWarning);
  }
}
