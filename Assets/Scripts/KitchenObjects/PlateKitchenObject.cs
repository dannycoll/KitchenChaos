using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateKitchenObject : KitchenObject
{
  public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
  public class OnIngredientAddedEventArgs : EventArgs
  {
    public KitchenObjectSO KitchenObjectSO;
  }

  [SerializeField] protected List<KitchenObjectSO> validKitchenObjects;
  protected List<KitchenObjectSO> KitchenObjectSOList;

  private void Awake()
  {
    KitchenObjectSOList = new List<KitchenObjectSO>();
  }
  public bool TryAddIngredient(KitchenObjectSO ingredientToAdd)
  {
    if (!validKitchenObjects.Contains(ingredientToAdd))
    {
      return false;
    }
    if (KitchenObjectSOList.Contains(ingredientToAdd))
    {
      return false;
    }
    KitchenObjectSOList.Add(ingredientToAdd);
    OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
    {
      KitchenObjectSO = ingredientToAdd
    });
    return true;
  }

  public List<KitchenObjectSO> GetKitchenObjectSOs()
  {
    return KitchenObjectSOList;
  }
}
