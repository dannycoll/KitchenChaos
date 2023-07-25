using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class OptionsUI : MonoBehaviour
{
  public static OptionsUI Instance;
  [SerializeField] private Button soundEffectsButton;
  [SerializeField] private Button musicButton;
  [SerializeField] private Button closeButton;
  [SerializeField] private Button moveUpButton;
  [SerializeField] private Button moveDownButton;
  [SerializeField] private Button moveLeftButton;
  [SerializeField] private Button moveRightButton;
  [SerializeField] private Button interactButton;
  [SerializeField] private Button interactAlternateButton;
  [SerializeField] private Button pauseButton;
  [SerializeField] private TextMeshProUGUI soundEffectsText;
  [SerializeField] private TextMeshProUGUI musicText;
  [SerializeField] private TextMeshProUGUI moveUpText;
  [SerializeField] private TextMeshProUGUI moveDownText;
  [SerializeField] private TextMeshProUGUI moveLeftText;
  [SerializeField] private TextMeshProUGUI moveRightText;
  [SerializeField] private TextMeshProUGUI interactText;
  [SerializeField] private TextMeshProUGUI interactAlternateText;
  [SerializeField] private TextMeshProUGUI pauseText;
  [SerializeField] private Transform pressToRebindKeyVisual;

  private Action _closeButtonAction;

  private void Awake()
  {
    Instance = this;
    soundEffectsButton.onClick.AddListener(() =>
    {
      SoundManager.Instance.ChangeVolume();
      UpdateVisual();
    });
    musicButton.onClick.AddListener(() =>
    {
      MusicManager.Instance.ChangeVolume();
      UpdateVisual();
    });
    closeButton.onClick.AddListener(() =>
    {
      _closeButtonAction();
      Hide();
    });
    moveUpButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.MoveUp));
    moveDownButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.MoveDown));
    moveLeftButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.MoveLeft));
    moveRightButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.MoveRight));
    interactButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Interact));
    interactAlternateButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.InteractAlternate));
    pauseButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Pause));

    Hide();
  }

  private void Start()
  {
    KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;
    UpdateVisual();
  }

  private void KitchenGameManager_OnGameUnpaused(object sender, EventArgs e)
  {
    Hide();
  }

  private void UpdateVisual()
  {
    soundEffectsText.text = "Sound Effects " + Mathf.RoundToInt(SoundManager.Instance.GetVolume() * 10).ToString();
    musicText.text = "Music " + Mathf.RoundToInt(MusicManager.Instance.GetVolume() * 10).ToString();
    moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
    moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
    moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
    moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
    interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
    interactAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
    pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
  }

  private void Hide()
  {
    gameObject.SetActive(false);
  }
  public void Show(Action closeButtonAction)
  {
    this._closeButtonAction = closeButtonAction;
    gameObject.SetActive(true);

    soundEffectsButton.Select();
  }

  private void ShowPressToRebindKeyVisual()
  {
    pressToRebindKeyVisual.gameObject.SetActive(true);
  }
  private void HidePressToRebindKeyVisual()
  {
    pressToRebindKeyVisual.gameObject.SetActive(false);
  }

  private void RebindBinding(GameInput.Binding binding)
  {
    ShowPressToRebindKeyVisual();
    GameInput.Instance.ChangeBinding(binding, () =>
    {
      HidePressToRebindKeyVisual();
      UpdateVisual();
    });
  }
}