using UnityEngine;
using System;
public class CuttingCounter : BaseCounter, IHasProgress
{

  public static event EventHandler OnAnyCut;

  public new static void ResetStaticData()
  {
    OnAnyCut = null;
  }

  public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

  public event EventHandler OnCut;

  [SerializeField] protected CuttingRecipeSO[] cuttingRecipes;

  private int _cuttingProgress;
  public override void Interact(Player player)
  {
    if (!HasKitchenObject())
    {
      if (player.HasKitchenObject())
      {
        if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
        {
          player.GetKitchenObject().SetKitchenObjectParent(this);
          _cuttingProgress = 0;
          CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(GetKitchenObject().GetKitchenObjectSO());

          OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
          {
            ProgressNormalized = _cuttingProgress / (float)cuttingRecipeSO.cutsNeeded
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
      else
      {
        if (player.GetKitchenObject().TryGetComponent(out PlateKitchenObject plateKitchenObject))
        {
          if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
          {
            GetKitchenObject().DestroySelf();
          }
        }
      }
    }
  }

  public override void InteractAlternate(Player player)
  {
    if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
    {
      _cuttingProgress++;
      OnCut?.Invoke(this, EventArgs.Empty);
      OnAnyCut?.Invoke(this, EventArgs.Empty);

      CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(GetKitchenObject().GetKitchenObjectSO());
      OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
      {
        ProgressNormalized = _cuttingProgress / (float)cuttingRecipeSO.cutsNeeded
      });

      if (_cuttingProgress >= cuttingRecipeSO.cutsNeeded)
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
