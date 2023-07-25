using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
  private const string OpenClose = "OpenClose";
  [SerializeField] private ContainerCounter containerCounter;
  private Animator _animator;
  private static readonly int Close = Animator.StringToHash(OpenClose);

  private void Awake()
  {
    _animator = GetComponent<Animator>();
  }

  private void Start()
  {
    containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
  }

  private void ContainerCounter_OnPlayerGrabbedObject(object sender, System.EventArgs e)
  {
    _animator.SetTrigger(Close);
  }
}
