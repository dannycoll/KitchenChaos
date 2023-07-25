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
        Assert.IsTrue(false);
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
    public void Interact_WHEN_hasObjectAndPlayerHasNoObject_THEN_PlayerGetsObject()
    {
        var kitchenObject = CreateKitchenObject();
        _clearCounter.SetKitchenObject(kitchenObject);
        kitchenObject.SetKitchenObjectParent(_clearCounter);
        
        _clearCounter.Interact(_player);
        
        Assert.AreEqual(_player, kitchenObject.GetKitchenObjectParent());
    }

    private KitchenObject CreateKitchenObject()
    {
        GameObject go = new GameObject();
        return go.AddComponent<KitchenObject>();
    }
}
