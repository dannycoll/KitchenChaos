using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour
{
  private const string IsFlashing = "IsFlashing";
  [SerializeField] private StoveCounter stoveCounter;
  private Animator _animator;
  private static readonly int Flashing = Animator.StringToHash(IsFlashing);

  private void Awake()
  {
    _animator = GetComponent<Animator>();
    _animator.SetBool(Flashing, false);
  }
  private void Start()
  {
    stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
  }

  private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
  {
    float burnShowProgressAmount = .5f;
    bool showBurnWarning = stoveCounter.IsFried() && e.ProgressNormalized >= burnShowProgressAmount;

    _animator.SetBool(Flashing, showBurnWarning);
  }
}
