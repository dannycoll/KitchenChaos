using UnityEngine;
using TMPro;
public class GameStartCountdownUI : MonoBehaviour
{
  private const string NumberPopup = "NumberPopup";
  [SerializeField]
  private TextMeshProUGUI countdownText;
  private Animator _animator;
  private int _previousCountdownNumber;
  private static readonly int Popup = Animator.StringToHash(NumberPopup);

  private void Awake()
  {
    _animator = GetComponent<Animator>();
  }
  private void Start()
  {
    KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
    Hide();
  }

  private void Update()
  {
    int countdownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountdownToStartTimer());
    countdownText.text = countdownNumber.ToString();

    if (_previousCountdownNumber != countdownNumber)
    {
      _previousCountdownNumber = countdownNumber;
      _animator.SetTrigger(Popup);
      SoundManager.Instance.PlayCountdownSound();
    }
  }


  private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
  {
    if (KitchenGameManager.Instance.IsCountdownToStartActive())
    {
      Show();
    }
    else
    {
      Hide();
    }
  }

  private void Show()
  {
    gameObject.SetActive(true);
  }

  private void Hide()
  {
    gameObject.SetActive(false);
  }
}
