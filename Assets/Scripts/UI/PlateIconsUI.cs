using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
  [SerializeField] protected PlateKitchenObject plateKitchenObject;
  [SerializeField] protected Transform iconTemplate;

  private void Awake()
  {
    iconTemplate.gameObject.SetActive(false);
  }

  private void Start()
  {
    plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
  }

  private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
  {
    UpdateVisual();
  }

  public void UpdateVisual()
  {
    foreach (Transform child in transform)
    {
      if (child == iconTemplate) continue;
      Destroy(child.gameObject);
    }
    foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOs())
    {
      Transform icon = Instantiate(iconTemplate, transform);
      icon.GetComponent<PlateSingleIconUI>().SetKitchenObjectSO(kitchenObjectSO);
      icon.gameObject.SetActive(true);
    }
  }
}
