using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class DeliveryManagerTest
{
    private class TestDeliveryManager : DeliveryManager
    {
        public List<RecipeSO> TestWaitingRecipeList
        {
            get => WaitingRecipeList;
            set => WaitingRecipeList = value;
        }

        public float TestSpawnRecipeTimer
        {
            get => SpawnRecipeTimer;
            set => SpawnRecipeTimer = value;
        }

        public RecipeListSO RecipeListSO
        {
            get => recipeListSO;
            set => recipeListSO = value;
        }
    }
    
    private class TestPlate : PlateKitchenObject
    {
        public List<KitchenObjectSO> TestKitchenObjectSOList
        {
            set => KitchenObjectSOList = value;
        }
    }
    
    private class TestKitchenGameManager : KitchenGameManager
    {
        public State TestState
        {
            set => GameState = value;
        }

        public float GameTimer
        {
            set => GamePlayingTimer = value;
        }
    }
    
    private TestDeliveryManager    _deliveryManager;
    private TestPlate              _plate;
    private TestKitchenGameManager _kitchenGameManager;

    [SetUp]
    public void Setup()
    {
        _deliveryManager = new GameObject().AddComponent<TestDeliveryManager>();
        _plate = new GameObject().AddComponent<TestPlate>();
        _kitchenGameManager = new GameObject().AddComponent<TestKitchenGameManager>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_deliveryManager);
    }
    
    [Test]
    public void DeliverRecipe_Has_InvalidRecipe_Triggers_RecipeFailed()
    {
        bool eventTriggered = false;
        _deliveryManager.OnRecipeFailed += (_, _) => eventTriggered = true;
        var recipeSO = ScriptableObject.CreateInstance<RecipeSO>();
        recipeSO.kitchenObjectSOList = new List<KitchenObjectSO>() { ScriptableObject.CreateInstance<KitchenObjectSO>() };
        _deliveryManager.TestWaitingRecipeList = new List<RecipeSO> { recipeSO };
        _plate.TestKitchenObjectSOList = new List<KitchenObjectSO>();

        _deliveryManager.DeliverRecipe(_plate);
        
        Assert.IsTrue(eventTriggered);
    }
    
    [Test]
    public void DeliverRecipe_Has_InvalidRecipeWithSameCount_Triggers_RecipeFailed()
    {
        bool eventTriggered = false;
        _deliveryManager.OnRecipeFailed += (_, _) => eventTriggered = true;
        var recipeSO = ScriptableObject.CreateInstance<RecipeSO>();
        recipeSO.kitchenObjectSOList = new List<KitchenObjectSO>() { ScriptableObject.CreateInstance<KitchenObjectSO>() };
        _deliveryManager.TestWaitingRecipeList = new List<RecipeSO> { recipeSO };
        _plate.TestKitchenObjectSOList = new List<KitchenObjectSO>() {ScriptableObject.CreateInstance<KitchenObjectSO>()};

        _deliveryManager.DeliverRecipe(_plate);
        
        Assert.IsTrue(eventTriggered);
    }
    
    [Test]
    public void DeliverRecipe_Has_ValidRecipe_Triggers_RecipeSuccess()
    {
        bool eventTriggered = false;
        _deliveryManager.OnRecipeSuccess += (_, _) => eventTriggered = true;
        var recipeSO = ScriptableObject.CreateInstance<RecipeSO>();
        var kitchenObjectSO = ScriptableObject.CreateInstance<KitchenObjectSO>();
        recipeSO.kitchenObjectSOList = new List<KitchenObjectSO>() { kitchenObjectSO };
        _deliveryManager.TestWaitingRecipeList = new List<RecipeSO> { recipeSO };
        _plate.TestKitchenObjectSOList = new List<KitchenObjectSO>() { kitchenObjectSO };

        _deliveryManager.DeliverRecipe(_plate);
        
        Assert.IsTrue(eventTriggered);
    }

    [Test]
    public void UpdateRecipeSpawns_OverTimer_SpawnsNewRecipe()
    {
        _kitchenGameManager.TestState = KitchenGameManager.State.GamePlaying;
        _deliveryManager.TestSpawnRecipeTimer = 5f;
        _deliveryManager.TestWaitingRecipeList = new List<RecipeSO>();
        var recipeListSO = ScriptableObject.CreateInstance<RecipeListSO>();
        recipeListSO.recipes = new List<RecipeSO>() { ScriptableObject.CreateInstance<RecipeSO>() };
        _deliveryManager.RecipeListSO = recipeListSO;
        bool eventTriggered = false;
        _deliveryManager.OnRecipeSpawned += (_, _) => eventTriggered = true;
        _deliveryManager.UpdateRecipeSpawns();
        
        Assert.IsTrue(eventTriggered);
        Assert.IsNotEmpty(_deliveryManager.TestWaitingRecipeList);
    }
}
