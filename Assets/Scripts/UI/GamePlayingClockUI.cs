using UnityEngine;
using UnityEngine.UI;
public class GamePlayingClockUI : MonoBehaviour
{
  [SerializeField]
  private Image clockImage;

  private void Start()
  {
    KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
    Hide();
  }
  private void Update()
  {
    clockImage.fillAmount = KitchenGameManager.Instance.GetGamePlayingTimerNormalized();
  }

  private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
  {
    if (KitchenGameManager.Instance.IsGamePlaying())
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
