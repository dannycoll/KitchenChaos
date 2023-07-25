using UnityEngine;
using TMPro;
public class TutorialUI : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI moveUpKey;
  [SerializeField] private TextMeshProUGUI moveDownKey;
  [SerializeField] private TextMeshProUGUI moveLeftKey;
  [SerializeField] private TextMeshProUGUI moveRightKey;
  [SerializeField] private TextMeshProUGUI interactKey;
  [SerializeField] private TextMeshProUGUI interactAltKey;
  [SerializeField] private TextMeshProUGUI pauseKey;

  private void Start()
  {
    GameInput.Instance.OnRebindKey += GameInput_OnRebindKey;
    KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
    UpdateVisual();
    Show();
  }

  private void GameInput_OnRebindKey(object sender, System.EventArgs e)
  {
    UpdateVisual();
  }

  private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
  {
    if (KitchenGameManager.Instance.IsCountdownToStartActive())
    {
      Hide();
    }
  }

  private void UpdateVisual()
  {
    moveDownKey.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
    moveUpKey.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
    moveLeftKey.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
    moveRightKey.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
    interactKey.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
    interactAltKey.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
    pauseKey.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
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
