using System;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlatesCounterTest
{
    private TestPlatesCounter _platesCounter;
    private TestPlayer        _player;
    private bool              _eventRaised;
    private GameInput         _gameInput;

    private TestKitchenGameManager _kitchenGameManager;

    private class TestKitchenGameManager : KitchenGameManager
    {
        public State TestState
        {
            set => GameState = value;
        }

        public float GameTimer
        {
            set => GamePlayingTimer = value;
        }
    }

    private class TestPlayer : Player
    {
        public GameInput GameInput
        {
            set => gameInput = value;
        }
    }

    private void HandlePlateSpawned(object sender, EventArgs e)
    {
        _eventRaised = true;
    }

    private class TestPlatesCounter : PlatesCounter
    {
        public KitchenObjectSO PlateKitchenObjectSO
        {
            set => plateKitchenObjectSO = value;
        }

        public int TestPlatesSpawned
        {
            get => PlateSpawnedAmount;
            set => PlateSpawnedAmount = value;
        }

        public float SpawnTimer
        {
            get => SpawnPlateTimer;
            set => SpawnPlateTimer = value;
        }
    }

    [SetUp]
    public void Setup()
    {
        _platesCounter = new GameObject().AddComponent<TestPlatesCounter>();
        _player = new GameObject().AddComponent<TestPlayer>();
        _kitchenGameManager = new GameObject().AddComponent<TestKitchenGameManager>();
        _gameInput = new GameObject().AddComponent<GameInput>();

        _kitchenGameManager.TestState = KitchenGameManager.State.GamePlaying;
        _kitchenGameManager.GameTimer = 20f;
        _platesCounter.OnPlateSpawned += HandlePlateSpawned;
        var kitchenSO = ScriptableObject.CreateInstance<KitchenObjectSO>();
        kitchenSO.prefab = new GameObject().AddComponent<KitchenObject>().transform;
        _platesCounter.PlateKitchenObjectSO = kitchenSO;
        _player.GameInput = _gameInput;

        _eventRaised = false;
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_player);
        Object.Destroy(_platesCounter);
        Object.Destroy(_kitchenGameManager);
        Object.Destroy(_gameInput);
    }

    [Test]
    public void Interact_WHEN_NoPlatePlayerGetsNoObject()
    {
        _platesCounter.Interact(_player);
        Assert.IsNull(_player.GetKitchenObject());
    }

    [Test]
    public void Interact_WHEN_HasPlatePlayerGetsObject()
    {
        _platesCounter.TestPlatesSpawned = 1;
        _platesCounter.Interact(_player);
        Assert.IsNotNull(_player.GetKitchenObject());
    }

    [Test]
    public void HandleSpawns_WHEN_TimerOverMax_THEN_ResetsAndPlateSpawns()
    {
        _platesCounter.SpawnTimer = 5f;
        _platesCounter.HandlePlateSpawns();
        
        Assert.IsTrue(_eventRaised);
        Assert.AreEqual(0f, _platesCounter.SpawnTimer);
        Assert.AreEqual(1, _platesCounter.TestPlatesSpawned);
    }
    
    [Test]
    public void HandleSpawns_WHEN_TimerUnderMax_THEN_EventNotCalled()
    {
        _platesCounter.SpawnTimer = 1f;
        _platesCounter.HandlePlateSpawns();
        
        Assert.IsFalse(_eventRaised);
    }
    
    [Test]
    public void HandleSpawns_WHEN_PlatesSpawnedOverMax_THEN_EventNotCalled()
    {
        _platesCounter.SpawnTimer = 5f;
        _platesCounter.TestPlatesSpawned = 10;
        _platesCounter.HandlePlateSpawns();
        
        Assert.IsFalse(_eventRaised);
    }
}