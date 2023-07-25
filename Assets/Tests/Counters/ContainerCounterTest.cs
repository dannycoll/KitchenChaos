using UnityEngine;
using NUnit.Framework;

[TestFixture]
public class ContainerCounterTests
{
    private TestContainerCounter _containerCounter;
    private Player _player;

    private class TestContainerCounter : ContainerCounter
    {
        public KitchenObjectSO KitchenObjectSO
        {
            get => kitchenObjectSO;
            set => kitchenObjectSO = value;
        }
    }
    
    [SetUp]
    public void SetUp()
    {
        _containerCounter = new GameObject().AddComponent<TestContainerCounter>();
        var kitchenObjectSO = ScriptableObject.CreateInstance<KitchenObjectSO>();
        kitchenObjectSO.prefab = new GameObject().AddComponent<KitchenObject>().transform;
        _containerCounter.KitchenObjectSO = kitchenObjectSO;
        _player = new GameObject().AddComponent<Player>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_containerCounter.gameObject);
        Object.Destroy(_player.gameObject);
    }

    [Test]
    public void Interact_PlayerDoesNotHaveKitchenObject_SpawnObjectAndTriggerEvent()
    {
        // Arrange
        bool eventTriggered = false;
        _containerCounter.OnPlayerGrabbedObject += (_, _) => eventTriggered = true;

        // Act
        _containerCounter.Interact(_player);

        // Assert
        Assert.IsTrue(_player.HasKitchenObject());
        Assert.IsTrue(eventTriggered);
    }

    [Test]
    public void Interact_PlayerHasKitchenObject_DoNothing()
    {
        // Arrange
        bool eventTriggered = false;
        _containerCounter.OnPlayerGrabbedObject += (_, _) => eventTriggered = true;

        // Simulate the player already having a kitchen object
        _player.SetKitchenObject(new GameObject().AddComponent<KitchenObject>());

        // Act
        _containerCounter.Interact(_player);

        // Assert
        Assert.IsFalse(eventTriggered);
    }
}