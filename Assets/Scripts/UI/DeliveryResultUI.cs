using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DeliveryResultUI : MonoBehaviour
{
  private const string POPUP = "Popup";
  [SerializeField] private Image background;
  [SerializeField] private TextMeshProUGUI resultText;
  [SerializeField] private Image icon;
  [SerializeField] private Color successColor;
  [SerializeField] private Color failColor;
  [SerializeField] private Sprite successIcon;
  [SerializeField] private Sprite failIcon;
  private Animator animator;

  private void Awake()
  {
    animator = GetComponent<Animator>();
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
    animator.SetTrigger(POPUP);
  }

  private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
  {
    gameObject.SetActive(true);
    background.color = successColor;
    icon.sprite = successIcon;
    resultText.text = "DELIVERY\nSUCCESS";
    animator.SetTrigger(POPUP);
  }
}
