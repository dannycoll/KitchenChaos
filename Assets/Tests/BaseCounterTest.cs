using NUnit.Framework;
using UnityEngine;

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

  private KitchenObject CreateKitchenObject()
  {
    GameObject go = new GameObject();
    return go.AddComponent<KitchenObject>();
  }
}
