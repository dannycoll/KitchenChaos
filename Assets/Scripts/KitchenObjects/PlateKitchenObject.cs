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
  private List<KitchenObjectSO> _kitchenObjectSOList;

  private void Awake()
  {
    _kitchenObjectSOList = new List<KitchenObjectSO>();
  }
  public bool TryAddIngredient(KitchenObjectSO ingredientToAdd)
  {
    if (!validKitchenObjects.Contains(ingredientToAdd))
    {
      return false;
    }
    if (_kitchenObjectSOList.Contains(ingredientToAdd))
    {
      return false;
    }
    _kitchenObjectSOList.Add(ingredientToAdd);
    OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
    {
      KitchenObjectSO = ingredientToAdd
    });
    return true;
  }

  public List<KitchenObjectSO> GetKitchenObjectSOs()
  {
    return _kitchenObjectSOList;
  }
}
