using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DeliveryCounterTests
{
    private DeliveryCounter _deliveryCounter;
    private Player _player;
    private DeliveryManager _deliveryManager;
    
    [SetUp]
    public void Setup()
    {
        _deliveryCounter = new GameObject().AddComponent<DeliveryCounter>();
        _player = new GameObject().AddComponent<Player>();
        _deliveryManager = new GameObject().AddComponent<DeliveryManager>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_deliveryCounter);
        Object.Destroy(_player);
        Object.Destroy(_deliveryManager);
    }

    [Test]
    public void Interact_PlayerHasPlate_THEN_ObjectIsDestroyed()
    {
        var plate = new GameObject().AddComponent<PlateKitchenObject>();
        _player.SetKitchenObject(plate);
        plate.SetKitchenObjectParent(_player);
        
        _deliveryCounter.Interact(_player);
        
        Assert.IsNull(_player.GetKitchenObject());
    }
}
