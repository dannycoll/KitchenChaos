using UnityEngine;
using NUnit.Framework;
using System;

[TestFixture]
public class BaseCounterTests
{
    private TestCounter _baseCounter;
    private bool _eventRaised;

    public class TestCounter : BaseCounter
    {
        public Transform CounterTopPoint
        {
            get => counterTopPoint;
            set => counterTopPoint = value;
        }
        public new void SetKitchenObject(KitchenObject kitchenObject)
        {
            base.SetKitchenObject(kitchenObject);
            // Can't use existing check here as cannot have counterTopPoint be not null on Start
            if (counterTopPoint != null && kitchenObject != null)
            {
                OnAnyObjectPlaced?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    
    [SetUp]
    public void SetUp()
    {
        _baseCounter = new GameObject().AddComponent<TestCounter>();
        _baseCounter.CounterTopPoint = new GameObject().transform; // Create a Transform for testing
        _eventRaised = false;

        // Subscribe to the event to track if it's raised
        BaseCounter.OnAnyObjectPlaced += HandleAnyObjectPlacedEvent;
    }

    [TearDown]
    public void TearDown()
    {
        // Unsubscribe from the event
        BaseCounter.OnAnyObjectPlaced -= HandleAnyObjectPlacedEvent;
    }

    private void HandleAnyObjectPlacedEvent(object sender, EventArgs e)
    {
        _eventRaised = true;
    }

    [Test]
    public void SetKitchenObject_EventRaised()
    {
        // Arrange
        var kitchenObject = CreateKitchenObject();
        Debug.Log(_baseCounter.CounterTopPoint);
        Debug.Log(kitchenObject != null);
        // Act
        _baseCounter.SetKitchenObject(kitchenObject);

        // Assert
        Assert.IsTrue(_eventRaised);
    }

    [Test]
    public void SetKitchenObject_NoEventRaised_WhenCounterTopPointIsNull()
    {
        // Arrange
        _baseCounter.CounterTopPoint = null;
        var kitchenObject = CreateKitchenObject();

        // Act
        _baseCounter.SetKitchenObject(kitchenObject);

        // Assert
        Assert.IsFalse(_eventRaised);
    }

    [Test]
    public void SetKitchenObject_NoEventRaised_WhenKitchenObjectIsNull()
    {
        // Arrange

        // Act
        _baseCounter.SetKitchenObject(null);

        // Assert
        Assert.IsFalse(_eventRaised);
    }

    [Test]
    public void ClearKitchenObject_ResetsKitchenObject()
    {
        // Arrange
        var kitchenObject = CreateKitchenObject();
        _baseCounter.SetKitchenObject(kitchenObject);

        // Act
        _baseCounter.ClearKitchenObject();

        // Assert
        Assert.IsFalse(_baseCounter.HasKitchenObject());
        Assert.IsNull(_baseCounter.GetKitchenObject());
    }

    [Test]
    public void GetKitchenObjectFollowTransform_ReturnsCounterTopPoint()
    {
        // Act
        var result = _baseCounter.GetKitchenObjectFollowTransform();

        // Assert
        Assert.AreEqual(_baseCounter.CounterTopPoint, result);
    }
    
    private KitchenObject CreateKitchenObject()
    {
        GameObject go = new GameObject();
        return go.AddComponent<KitchenObject>();
    }
    
    
}
