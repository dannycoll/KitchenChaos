using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CuttingCounterTests
{
    private TestCuttingCounter _cuttingCounter;
    private Player _player;

    private class TestCuttingCounter : CuttingCounter
    {
        public CuttingRecipeSO[] CuttingRecipes
        {
            set => cuttingRecipes = value;
        }
    }
    
    [SetUp]
    public void Setup()
    {
        _cuttingCounter = new GameObject().AddComponent<TestCuttingCounter>();
        _cuttingCounter.CuttingRecipes = new CuttingRecipeSO[]{};
        _player = new GameObject().AddComponent<Player>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_cuttingCounter);
        Object.Destroy(_player);
    }

    [Test]
    public void Interact_WHEN_HasNoObjectAndPlayerHasInvalidObject_DoesNotReceiveObject()
    {
        var kitchenObject = new GameObject().AddComponent<KitchenObject>();
        _player.SetKitchenObject(kitchenObject);
        kitchenObject.SetKitchenObjectParent(_player);
        
        _cuttingCounter.Interact(_player);
        
        Assert.AreEqual(_player.GetKitchenObject(), kitchenObject);
        Assert.IsNull(_cuttingCounter.GetKitchenObject());
    }
    
    [Test]
    public void Interact_WHEN_HasObjectAndPlayerHasNoObject_PlayerReceivesObject()
    {
        var kitchenObject = new GameObject().AddComponent<KitchenObject>();
        _cuttingCounter.SetKitchenObject(kitchenObject);
        kitchenObject.SetKitchenObjectParent(_cuttingCounter);
        
        _cuttingCounter.Interact(_player);
        
        Assert.AreEqual(kitchenObject,_player.GetKitchenObject());
        Assert.IsNull(_cuttingCounter.GetKitchenObject());
    }

    [Test]
    public void Interact_WHEN_HasNoObjectAndPlayerHasValidObject_CounterReceivesObject()
    {
        var kitchenObject = new GameObject().AddComponent<TestKitchenObject>();
        var recipe = ScriptableObject.CreateInstance<CuttingRecipeSO>();
        var kitchenObjectSO = ScriptableObject.CreateInstance<KitchenObjectSO>();
        kitchenObject.KitchenObjectSO = kitchenObjectSO;
        recipe.input = kitchenObjectSO;
        _cuttingCounter.CuttingRecipes = new[] { recipe };
        
        _player.SetKitchenObject(kitchenObject);
        kitchenObject.SetKitchenObjectParent(_player);
        
        _cuttingCounter.Interact(_player);
        
        Assert.IsNull(_player.GetKitchenObject());
        Assert.IsNotNull(_cuttingCounter.GetKitchenObject());
        Assert.AreEqual(kitchenObject, _cuttingCounter.GetKitchenObject());
    }

    [Test]
    public void Interact_WHEN_HasObjectAndPlayerHasPlate_PlateReceivesObject()
    {
        var plate = CreatePlate();
        plate.SetKitchenObjectParent(_player);
        
        var kitchenObject = new GameObject().AddComponent<TestKitchenObject>();
        var kitchenObjectSO = ScriptableObject.CreateInstance<KitchenObjectSO>();
        kitchenObject.KitchenObjectSO = kitchenObjectSO;
        plate.ValidKitchenObjects.Add(kitchenObjectSO);
        
        _cuttingCounter.SetKitchenObject(kitchenObject);
        kitchenObject.SetKitchenObjectParent(_cuttingCounter);
        
        _cuttingCounter.Interact(_player);
        
        Assert.IsNull(_cuttingCounter.GetKitchenObject());
        _player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject);
        Assert.IsTrue(plateKitchenObject.GetKitchenObjectSOs().Contains(kitchenObject.GetKitchenObjectSO()));
    }

    [Test]
    public void InteractAlt_WHEN_HasValidObject_Cuts()
    {
        var input = new GameObject().AddComponent<TestKitchenObject>();
        var output = new GameObject().AddComponent<TestKitchenObject>();
        var recipe = ScriptableObject.CreateInstance<CuttingRecipeSO>();
        var inputSO = ScriptableObject.CreateInstance<KitchenObjectSO>();
        var outputSO = ScriptableObject.CreateInstance<KitchenObjectSO>();
        outputSO.prefab = output.transform;

        input.KitchenObjectSO = inputSO;
        recipe.input = inputSO;
        recipe.cutsNeeded = 1;
        recipe.output = outputSO;
        _cuttingCounter.CuttingRecipes = new[] { recipe };
        _cuttingCounter.SetKitchenObject(input);
        input.SetKitchenObjectParent(_cuttingCounter);
        _cuttingCounter.InteractAlternate(_player);
        
        // TODO: can I verify that the object spawned is the same as the output GO?
        Assert.AreNotEqual(input, _cuttingCounter.GetKitchenObject());
    }
    
    private class TestKitchenObject : KitchenObject
    {
        public KitchenObjectSO KitchenObjectSO
        {
            set => kitchenObjectSO = value;
        }
    }
    
    private class TestPlate : PlateKitchenObject
    {
        public List<KitchenObjectSO> ValidKitchenObjects
        {
            get => validKitchenObjects;
            set => validKitchenObjects = value;
        }
    }
    
    private static TestPlate CreatePlate()
    {
        var plate = new GameObject().AddComponent<TestPlate>();
        plate.ValidKitchenObjects = new List<KitchenObjectSO>();
        return plate;
    }
}
