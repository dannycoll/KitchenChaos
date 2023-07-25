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

  [SerializeField] private List<KitchenObjectSO> validKitchenObjects;
  private List<KitchenObjectSO> _kitchenObjectSOList;

  private void Awake()
  {
    _kitchenObjectSOList = new List<KitchenObjectSO>();
  }
  public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
  {
    if (!validKitchenObjects.Contains(kitchenObjectSO))
    {
      return false;
    }
    if (_kitchenObjectSOList.Contains(kitchenObjectSO))
    {
      return false;
    }
    _kitchenObjectSOList.Add(kitchenObjectSO);
    OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
    {
      KitchenObjectSO = kitchenObjectSO
    });
    return true;
  }

  public List<KitchenObjectSO> GetKitchenObjectSOs()
  {
    return _kitchenObjectSOList;
  }
}
