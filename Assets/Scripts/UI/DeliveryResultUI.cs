using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DeliveryResultUI : MonoBehaviour
{
  private const string Popup = "Popup";
  [SerializeField] private Image background;
  [SerializeField] private TextMeshProUGUI resultText;
  [SerializeField] private Image icon;
  [SerializeField] private Color successColor;
  [SerializeField] private Color failColor;
  [SerializeField] private Sprite successIcon;
  [SerializeField] private Sprite failIcon;
  private Animator _animator;
  private static readonly int Popup1 = Animator.StringToHash(Popup);

  private void Awake()
  {
    _animator = GetComponent<Animator>();
  }

  private void Start()
  {
    DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
    DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
    gameObject.SetActive(false);
  }

  private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
  {
    Debug.Log("Delivery Failed");
    gameObject.SetActive(true);
    background.color = failColor;
    icon.sprite = failIcon;
    resultText.text = "DELIVERY\nFAILED";
    _animator.SetTrigger(Popup1);
  }

  private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
  {
    gameObject.SetActive(true);
    background.color = successColor;
    icon.sprite = successIcon;
    resultText.text = "DELIVERY\nSUCCESS";
    _animator.SetTrigger(Popup1);
  }
}
