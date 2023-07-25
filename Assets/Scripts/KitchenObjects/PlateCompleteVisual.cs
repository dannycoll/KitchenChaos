using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateCompleteVisual : MonoBehaviour
{
  [Serializable]
  public struct KitchenObjectSOGameObject
  {
    public KitchenObjectSO kitchenObjectSO;
    public GameObject gameObject;
  }
  [SerializeField] private PlateKitchenObject plateKitchenObject;
  [SerializeField] private List<KitchenObjectSOGameObject> kitchenObjectSOGameObjectList;

  private void Start()
  {
    plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    foreach (KitchenObjectSOGameObject kitchenObject in kitchenObjectSOGameObjectList)
    {
      kitchenObject.gameObject.SetActive(false);
    }
  }

  private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
  {
    foreach (KitchenObjectSOGameObject kitchenObject in kitchenObjectSOGameObjectList)
    {
      if (kitchenObject.kitchenObjectSO == e.KitchenObjectSO)
      {
        kitchenObject.gameObject.SetActive(true);
      }
    }
  }
}
