using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ClearCounterTest
{
    private TestClearCounter clearCounter;
    private Player player;
    private class TestClearCounter : ClearCounter
    {
        // Expose counterTopPoint through a public property for testing
        public new Transform counterTopPoint { get => base.counterTopPoint; set => base.counterTopPoint = value; }
    }

    [SetUp]
    public void SetUp()
    {
        // Create a new GameObject and add the TestBaseCounter component to it
        GameObject gameObject = new GameObject();
        clearCounter = gameObject.AddComponent<TestClearCounter>();
        player = new GameObject().AddComponent<Player>();

        // Create and set up the counterTopPoint Transform
        clearCounter.counterTopPoint = new GameObject().transform;
    }

    [TearDown]
    public void TearDown()
    {
        // Destroy the GameObject to clean up after each test
        Object.Destroy(clearCounter.gameObject);
        Object.Destroy(player.gameObject);
    }

    [Test]
    public void Interact_WHEN_hasNoKitchenObjectAndPlayerHasKitchenObject_THEN_kitchenObjectParentIsThis()
    {
        var kitchenObject = createKitchenObject();
        player.SetKitchenObject(kitchenObject);
        kitchenObject.SetKitchenObjectParent(player);
        
        clearCounter.Interact(player);
        Assert.AreEqual(clearCounter, kitchenObject.GetKitchenObjectParent());
    }

    [Test]
    public void Interact_WHEN_hasObjectAndPlayerHasObject_THEN_nothingChanges()
    {
        var kitchenObject = createKitchenObject();
        player.SetKitchenObject(kitchenObject);
        kitchenObject.SetKitchenObjectParent(player);
        var kitchenObject2 = createKitchenObject();
        clearCounter.SetKitchenObject(kitchenObject2);
        kitchenObject2.SetKitchenObjectParent(clearCounter);
        
        clearCounter.Interact(player);
        
        Assert.AreEqual(player, kitchenObject.GetKitchenObjectParent());
        Assert.AreEqual(clearCounter, kitchenObject2.GetKitchenObjectParent());
    }

    private KitchenObject createKitchenObject()
    {
        GameObject go = new GameObject();
        return go.AddComponent<KitchenObject>();
    }
}
