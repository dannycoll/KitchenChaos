using System.Collections;
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
  private RecipeListSO recipeListSO;
  private List<RecipeSO> waitingRecipeList;
  private int waitingRecipeListMaxLen = 4;

  private float spawnRecipeTimer;
  private float spawnRecipeTimerMax = 4f;
  private int successfulRecipes;
  private void Awake()
  {
    Instance = this;
    waitingRecipeList = new List<RecipeSO>();
    spawnRecipeTimer = 0;
    successfulRecipes = 0;
  }

  private void Update()
  {
    spawnRecipeTimer += Time.deltaTime;
    if (KitchenGameManager.Instance.IsGamePlaying() && spawnRecipeTimer >= spawnRecipeTimerMax && waitingRecipeList.Count < waitingRecipeListMaxLen)
    {
      RecipeSO waitingRecipeSO = recipeListSO.recipes[UnityEngine.Random.Range(0, recipeListSO.recipes.Count)];
      waitingRecipeList.Add(waitingRecipeSO);
      spawnRecipeTimer = 0;

      OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
    }
  }

  public void DeliverRecipe(PlateKitchenObject plate)
  {
    foreach (RecipeSO waitingRecipeSO in waitingRecipeList)
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
          waitingRecipeList.Remove(waitingRecipeSO);
          OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
          OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
          successfulRecipes++;
          return;
        }
      }
      OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }
  }

  public List<RecipeSO> GetWaitingRecipes()
  {
    return waitingRecipeList;
  }

  public int GetSuccessfulRecipes()
  {
    return successfulRecipes;
  }
}