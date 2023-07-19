using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateCompleteVisual : MonoBehaviour
{
  [Serializable]
  public struct KitchenObjectSO_GameObject
  {
    public KitchenObjectSO kitchenObjectSO;
    public GameObject gameObject;
  }
  [SerializeField] private PlateKitchenObject plateKitchenObject;
  [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectList;

  private void Start()
  {
    plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    foreach (KitchenObjectSO_GameObject kitchenObject in kitchenObjectSOGameObjectList)
    {
      kitchenObject.gameObject.SetActive(false);
    }
  }

  private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
  {
    foreach (KitchenObjectSO_GameObject kitchenObject in kitchenObjectSOGameObjectList)
    {
      if (kitchenObject.kitchenObjectSO == e.KitchenObjectSO)
      {
        kitchenObject.gameObject.SetActive(true);
      }
    }
  }
}
