using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{

  [SerializeField] private CuttingRecipeSO[] cuttingRecipes;
  public override void Interact(Player player)
  {
    if (!HasKitchenObject())
    {
      if (player.HasKitchenObject())
      {
        if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
        {
          player.GetKitchenObject().SetKitchenObjectParent(this);
        }
      }
    }
    else
    {
      if (!player.HasKitchenObject())
      {
        GetKitchenObject().SetKitchenObjectParent(player);
      }
    }
  }

  public override void InteractAlternate(Player player)
  {
    if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
    {
      KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
      GetKitchenObject().DestroySelf();

      KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);

    }
  }

  private bool HasRecipeWithInput(KitchenObjectSO input)
  {
    foreach (CuttingRecipeSO item in cuttingRecipes)
    {
      if (item.input == input)
      {
        return true;
      }
    }
    return false;
  }
  private KitchenObjectSO GetOutputForInput(KitchenObjectSO input)
  {
    foreach (CuttingRecipeSO item in cuttingRecipes)
    {
      if (item.input == input)
      {
        return item.output;
      }
    }
    return null;
  }
}
