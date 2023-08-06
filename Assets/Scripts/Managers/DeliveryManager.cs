using System.Collections.Generic;
using UnityEngine;
using System;

public class DeliveryManager : MonoBehaviour
{
  public event EventHandler OnRecipeSpawned;
  public event EventHandler OnRecipeCompleted;
  public event EventHandler OnRecipeSuccess;
  public event EventHandler OnRecipeFailed;


  public static DeliveryManager Instance { get; set; }
  [SerializeField]
  protected RecipeListSO recipeListSO;
  protected List<RecipeSO> WaitingRecipeList;
  private const int WaitingRecipeListMaxLen = 4;

  protected float SpawnRecipeTimer;
  private const float SpawnRecipeTimerMax = 4f;
  private int _successfulRecipes;
  private void Awake()
  {
    Instance = this;
    WaitingRecipeList = new List<RecipeSO>();
    SpawnRecipeTimer = 0;
    _successfulRecipes = 0;
  }

  private void Update()
  {
    UpdateRecipeSpawns();
  }

  public void UpdateRecipeSpawns()
  {
    SpawnRecipeTimer += Time.deltaTime;
    if (KitchenGameManager.Instance.IsGamePlaying() && SpawnRecipeTimer >= SpawnRecipeTimerMax && WaitingRecipeList.Count < WaitingRecipeListMaxLen)
    {
      RecipeSO waitingRecipeSO = recipeListSO.recipes[UnityEngine.Random.Range(0, recipeListSO.recipes.Count)];
      WaitingRecipeList.Add(waitingRecipeSO);
      SpawnRecipeTimer = 0;

      OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
    }
  }

  public void DeliverRecipe(PlateKitchenObject plate)
  {
    foreach (RecipeSO waitingRecipeSO in WaitingRecipeList)
    {
      if (waitingRecipeSO.kitchenObjectSOList.Count == plate.GetKitchenObjectSOs().Count)
      {
        bool plateContentsMatchRecipe = true;
        foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
        {
          bool ingredientFound = false;
          foreach (KitchenObjectSO plateKitchenObjectSO in plate.GetKitchenObjectSOs())
          {
            if (plateKitchenObjectSO == recipeKitchenObjectSO)
            {
              ingredientFound = true;
              break;
            }
          }
          if (!ingredientFound)
          {
            plateContentsMatchRecipe = false;
          }
        }
        if (plateContentsMatchRecipe)
        {
          WaitingRecipeList.Remove(waitingRecipeSO);
          OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
          OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
          _successfulRecipes++;
          return;
        }
      }
      OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }
  }

  public List<RecipeSO> GetWaitingRecipes()
  {
    return WaitingRecipeList;
  }

  public int GetSuccessfulRecipes()
  {
    return _successfulRecipes;
  }
}
