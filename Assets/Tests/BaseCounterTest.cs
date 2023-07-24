using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BaseCounterTest
{
  private TestBaseCounter baseCounter;

  private class TestBaseCounter : BaseCounter
  {
    // Expose counterTopPoint through a public property for testing
    public new Transform counterTopPoint { get => base.counterTopPoint; set => base.counterTopPoint = value; }
  }

  [SetUp]
  public void SetUp()
  {
    // Create a new GameObject and add the TestBaseCounter component to it
    GameObject gameObject = new GameObject();
    baseCounter = gameObject.AddComponent<TestBaseCounter>();

    // Create and set up the counterTopPoint Transform
    baseCounter.counterTopPoint = new GameObject().transform;
  }

  [TearDown]
  public void TearDown()
  {
    // Destroy the GameObject to clean up after each test
    Object.Destroy(baseCounter.gameObject);
  }

  [Test]
  public void SetKitchenObject_ObjectIsNotNull()
  {
    // Arrange
    KitchenObject kitchenObject = createKitchenObject();

    // Act
    baseCounter.SetKitchenObject(kitchenObject);

    // Assert
    Assert.IsTrue(baseCounter.HasKitchenObject());
    Assert.AreEqual(kitchenObject, baseCounter.GetKitchenObject());
  }

  [Test]
  public void SetKitchenObject_RaisesEvent()
  {
    // Arrange
    KitchenObject kitchenObject = createKitchenObject();
    bool eventRaised = false;
    BaseCounter.OnAnyObjectPlaced += (sender, args) => eventRaised = true;

    // Act
    baseCounter.SetKitchenObject(kitchenObject);

    // Assert
    Assert.IsTrue(eventRaised);
  }

  private KitchenObject createKitchenObject()
  {
    GameObject go = new GameObject();
    return (KitchenObject)go.AddComponent<KitchenObject>();
  }
}
