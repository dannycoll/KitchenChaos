using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CuttingCounter : BaseCounter
{

  public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
  public class OnProgressChangedEventArgs : EventArgs
  {
    public float progressNormalized;
  }
  public event EventHandler OnCut;

  [SerializeField] private CuttingRecipeSO[] cuttingRecipes;

  private int cuttingProgress;
  public override void Interact(Player player)
  {
    if (!HasKitchenObject())
    {
      if (player.HasKitchenObject())
      {
        if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
        {
          player.GetKitchenObject().SetKitchenObjectParent(this);
          cuttingProgress = 0;
          CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(GetKitchenObject().GetKitchenObjectSO());

          OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
          {
            progressNormalized = (float)cuttingProgress / (float)cuttingRecipeSO.cutsNeeded
          });
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
      cuttingProgress++;
      OnCut?.Invoke(this, EventArgs.Empty);

      CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(GetKitchenObject().GetKitchenObjectSO());
      OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
      {
        progressNormalized = (float)cuttingProgress / (float)cuttingRecipeSO.cutsNeeded
      });

      if (cuttingProgress >= cuttingRecipeSO.cutsNeeded)
      {

        KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
        GetKitchenObject().DestroySelf();

        KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
      }

    }
  }

  private bool HasRecipeWithInput(KitchenObjectSO input)
  {
    var cuttingRecipe = GetCuttingRecipeSOForInput(input);
    return cuttingRecipe != null;
  }
  private KitchenObjectSO GetOutputForInput(KitchenObjectSO input)
  {
    var cuttingRecipe = GetCuttingRecipeSOForInput(input);
    if (cuttingRecipe != null)
      return cuttingRecipe.output;
    return null;
  }

  private CuttingRecipeSO GetCuttingRecipeSOForInput(KitchenObjectSO input)
  {
    foreach (CuttingRecipeSO item in cuttingRecipes)
    {
      if (item.input == input)
      {
        return item;
      }
    }
    return null;
  }
}
