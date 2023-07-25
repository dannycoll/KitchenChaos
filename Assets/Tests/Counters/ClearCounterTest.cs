using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ClearCounterTest
{
    private TestClearCounter _clearCounter;
    private Player _player;
    private class TestClearCounter : ClearCounter
    {
        // Expose counterTopPoint through a public property for testing
        public Transform CounterTopPoint { set => counterTopPoint = value; }
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

    [SetUp]
    public void SetUp()
    {
        // Create a new GameObject and add the TestBaseCounter component to it
        GameObject gameObject = new GameObject();
        _clearCounter = gameObject.AddComponent<TestClearCounter>();
        _player = new GameObject().AddComponent<Player>();

        // Create and set up the counterTopPoint Transform
        _clearCounter.CounterTopPoint = new GameObject().transform;
    }

    [TearDown]
    public void TearDown()
    {
        // Destroy the GameObject to clean up after each test
        Object.Destroy(_clearCounter.gameObject);
        Object.Destroy(_player.gameObject);
    }

    [Test]
    public void Interact_WHEN_hasNoKitchenObjectAndPlayerHasKitchenObject_THEN_kitchenObjectParentIsThis()
    {
        var kitchenObject = CreateKitchenObject();
        _player.SetKitchenObject(kitchenObject);
        kitchenObject.SetKitchenObjectParent(_player);
        
        _clearCounter.Interact(_player);
        Assert.AreEqual(_clearCounter, kitchenObject.GetKitchenObjectParent());
    }

    [Test]
    public void Interact_WHEN_hasObjectAndPlayerHasObject_THEN_nothingChanges()
    {
        var kitchenObject = CreateKitchenObject();
        _player.SetKitchenObject(kitchenObject);
        kitchenObject.SetKitchenObjectParent(_player);
        var kitchenObject2 = CreateKitchenObject();
        _clearCounter.SetKitchenObject(kitchenObject2);
        kitchenObject2.SetKitchenObjectParent(_clearCounter);
        
        _clearCounter.Interact(_player);
        
        Assert.AreEqual(_player, kitchenObject.GetKitchenObjectParent());
        Assert.AreEqual(_clearCounter, kitchenObject2.GetKitchenObjectParent());
    }
    
    [Test]
    public void Interact_WHEN_hasObjectAndPlayerHasNoObject_THEN_playerGetsObject()
    {
        var kitchenObject = CreateKitchenObject();
        _clearCounter.SetKitchenObject(kitchenObject);
        kitchenObject.SetKitchenObjectParent(_clearCounter);
        
        _clearCounter.Interact(_player);
        
        Assert.AreEqual(_player, kitchenObject.GetKitchenObjectParent());
    }
    
    [Test]
    public void Interact_WHEN_hasObjectAndPlayerHasPlate_THEN_playerGetsObjectOnPlate()
    {
        var plate = CreatePlate();
        
        plate.SetKitchenObjectParent(_player);
        var kitchenObject2 = CreateKitchenObject();
        var scriptableObject = ScriptableObject.CreateInstance<KitchenObjectSO>();
        kitchenObject2.KitchenObjectSO = scriptableObject;
        plate.ValidKitchenObjects.Add(scriptableObject);
        
        _clearCounter.SetKitchenObject(kitchenObject2);
        kitchenObject2.SetKitchenObjectParent(_clearCounter);
        
        _clearCounter.Interact(_player);
        
        Assert.IsNull(_clearCounter.GetKitchenObject());
        _player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject);
        Assert.IsTrue(plateKitchenObject.GetKitchenObjectSOs().Contains(kitchenObject2.GetKitchenObjectSO()));
    }
    
    [Test]
    public void Interact_WHEN_hasInvalidObjectAndPlayerHasPlate_THEN_nothingChanges()
    {
        var plate = CreatePlate();
        
        plate.SetKitchenObjectParent(_player);
        var kitchenObject2 = CreateKitchenObject();
        var scriptableObject = ScriptableObject.CreateInstance<KitchenObjectSO>();
        kitchenObject2.KitchenObjectSO = scriptableObject;
        
        _clearCounter.SetKitchenObject(kitchenObject2);
        kitchenObject2.SetKitchenObjectParent(_clearCounter);
        
        _clearCounter.Interact(_player);
        
        _player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject);
        Assert.IsFalse(plateKitchenObject.GetKitchenObjectSOs().Contains(kitchenObject2.GetKitchenObjectSO()));
        Assert.AreEqual(_clearCounter, kitchenObject2.GetKitchenObjectParent());
    }
    
    [Test]
    public void Interact_WHEN_hasPlateAndPlayerHasValidObject_THEN_getsObjectOnPlate()
    {
        var plate = CreatePlate();
        
        plate.SetKitchenObjectParent(_clearCounter);
        var kitchenObject2 = CreateKitchenObject();
        var scriptableObject = ScriptableObject.CreateInstance<KitchenObjectSO>();
        kitchenObject2.KitchenObjectSO = scriptableObject;
        plate.ValidKitchenObjects.Add(scriptableObject);
        
        _player.SetKitchenObject(kitchenObject2);
        kitchenObject2.SetKitchenObjectParent(_player);
        
        _clearCounter.Interact(_player);
        
        Assert.IsNull(_player.GetKitchenObject());
        _clearCounter.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject);
        Assert.IsTrue(plateKitchenObject.GetKitchenObjectSOs().Contains(kitchenObject2.GetKitchenObjectSO()));
    }

    private static TestKitchenObject CreateKitchenObject()
    {
        GameObject go = new GameObject();
        return go.AddComponent<TestKitchenObject>();
    }

    private static TestPlate CreatePlate()
    {
        var plate = new GameObject().AddComponent<TestPlate>();
        plate.ValidKitchenObjects = new List<KitchenObjectSO>();
        return plate;
    }
}
