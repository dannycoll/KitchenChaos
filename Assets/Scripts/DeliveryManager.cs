using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; set; }
  [SerializeField]
  private RecipeListSO recipeListSO;
  private List<RecipeSO> waitingRecipeList;
  private int waitingRecipeListMaxLen = 4;

  private float spawnRecipeTimer;
  private float spawnRecipeTimerMax = 4f;
 private void Awake() {
    Instance = this;
    waitingRecipeList = new List<RecipeSO>();
    spawnRecipeTimer = 0;
  }

  private void Update() {
    spawnRecipeTimer += Time.deltaTime;
    if(spawnRecipeTimer >= spawnRecipeTimerMax && waitingRecipeList.Count < waitingRecipeListMaxLen) {
      RecipeSO waitingRecipeSO = recipeListSO.recipes[Random.Range(0, recipeListSO.recipes.Count)];
      waitingRecipeList.Add(waitingRecipeSO);
      spawnRecipeTimer = 0;
    }
  }

  public void DeliverRecipe(PlateKitchenObject plate) {
    foreach (RecipeSO waitingRecipeSO in waitingRecipeList) {
        if(waitingRecipeSO.kitchenObjectSOList.Count == plate.GetKitchenObjectSOs().Count) {
        bool plateContentsMatchRecipe = true;
        foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList) {
          bool ingredientFound = false;
          foreach (KitchenObjectSO plateKitchenObjectSO in plate.GetKitchenObjectSOs()) {
                    if (plateKitchenObjectSO == recipeKitchenObjectSO) {
              ingredientFound = true;
              break;
            }
                }
            if(!ingredientFound) {
          plateContentsMatchRecipe = false;
        }
            }
        if(plateContentsMatchRecipe) {
          waitingRecipeList.Remove(waitingRecipeSO);
          return;
        }
        }
    }
  }
}
