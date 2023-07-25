using System;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

public class BaseCounterTest
{
  private TestBaseCounter _baseCounter;

  private class TestBaseCounter : BaseCounter
  {
    // Expose counterTopPoint through a public property for testing
    public Transform CounterTopPoint { set => counterTopPoint = value; }
  }

  [SetUp]
  public void SetUp()
  {
    // Create a new GameObject and add the TestBaseCounter component to it
    GameObject gameObject = new GameObject();
    _baseCounter = gameObject.AddComponent<TestBaseCounter>();

    // Create and set up the counterTopPoint Transform
    _baseCounter.CounterTopPoint = new GameObject().transform;
  }

  [TearDown]
  public void TearDown()
  {
    // Destroy the GameObject to clean up after each test
    Object.Destroy(_baseCounter.gameObject);
  }

  [Test]
  public void SetKitchenObject_ObjectIsNotNull()
  {
    // Arrange
    KitchenObject kitchenObject = CreateKitchenObject();

    // Act
    _baseCounter.SetKitchenObject(kitchenObject);

    // Assert
    Assert.IsTrue(_baseCounter.HasKitchenObject());
    Assert.AreEqual(kitchenObject, _baseCounter.GetKitchenObject());
  }

  [Test]
  public void SetKitchenObject_RaisesEvent()
  {
    // Arrange
    KitchenObject kitchenObject = CreateKitchenObject();
    bool eventRaised = false;
    BaseCounter.OnAnyObjectPlaced += (_, _) => eventRaised = true;

    // Act
    _baseCounter.SetKitchenObject(kitchenObject);

    // Assert
    Assert.IsTrue(eventRaised);
  }

  [Test]
  public void ClearKitchenObjct_SetsKitchenObjectToNull()
  {
    KitchenObject kitchenObject = CreateKitchenObject();
    _baseCounter.SetKitchenObject(kitchenObject);
    _baseCounter.ClearKitchenObject();
    Assert.IsNull(_baseCounter.GetKitchenObject());
  }

  [Test]
  public void ResetStaticData_SetsOnAnyObjectPlacedToNull()
  {
    BaseCounter.ResetStaticData();

    // Act
    BaseCounter.OnAnyObjectPlaced += SomeTestMethod1;
    BaseCounter.OnAnyObjectPlaced += SomeTestMethod2;
    
    BaseCounter.ResetStaticData();
    
    Assert.IsNull(BaseCounter.OnAnyObjectPlaced, "All event handlers should have been removed after calling ResetStaticData.");

  }

  private KitchenObject CreateKitchenObject()
  {
    GameObject go = new GameObject();
    return go.AddComponent<KitchenObject>();
  }
  
  private void SomeTestMethod1(object sender, EventArgs e) { /* Some implementation here */ }
  private void SomeTestMethod2(object sender, EventArgs e) { /* Some implementation here */ }

}
