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
  private List<RecipeSO> _waitingRecipeList;
  private const int WaitingRecipeListMaxLen = 4;

  private float _spawnRecipeTimer;
  private const float SpawnRecipeTimerMax = 4f;
  private int _successfulRecipes;
  private void Awake()
  {
    Instance = this;
    _waitingRecipeList = new List<RecipeSO>();
    _spawnRecipeTimer = 0;
    _successfulRecipes = 0;
  }

  private void Update()
  {
    _spawnRecipeTimer += Time.deltaTime;
    if (KitchenGameManager.Instance.IsGamePlaying() && _spawnRecipeTimer >= SpawnRecipeTimerMax && _waitingRecipeList.Count < WaitingRecipeListMaxLen)
    {
      RecipeSO waitingRecipeSO = recipeListSO.recipes[UnityEngine.Random.Range(0, recipeListSO.recipes.Count)];
      _waitingRecipeList.Add(waitingRecipeSO);
      _spawnRecipeTimer = 0;

      OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
    }
  }

  public void DeliverRecipe(PlateKitchenObject plate)
  {
    foreach (RecipeSO waitingRecipeSO in _waitingRecipeList)
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
          _waitingRecipeList.Remove(waitingRecipeSO);
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
    return _waitingRecipeList;
  }

  public int GetSuccessfulRecipes()
  {
    return _successfulRecipes;
  }
}
