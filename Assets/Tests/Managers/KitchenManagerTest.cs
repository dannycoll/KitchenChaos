using System;
using NUnit.Framework;
using UnityEngine;

public class KitchenGameManagerTests
{
    private TestableKGM _kitchenGameManager;

    private class TestableKGM : KitchenGameManager
    {
        public State TestGameState
        {
            get => GameState;
            set => GameState = value;
        }

        public float TestCountdownToStartTimer
        {
            set => CountdownToStartTimer = value;
        }

        public float TestGamePlayingTimer
        {
            set => GamePlayingTimer = value;
        }

        public bool TestIsGamePaused
        {
            get => IsGamePaused;
        }
    }
    [SetUp]
    public void Setup()
    {
        _kitchenGameManager = new GameObject().AddComponent<TestableKGM>();
        _kitchenGameManager.TestGameState = KitchenGameManager.State.WaitingToStart; // Set initial state
    }

    [Test]
    public void Update_CountdownToStartTimerExpires_UpdatesStateToGamePlaying()
    {
        _kitchenGameManager.TestGameState = KitchenGameManager.State.CountdownToStart;
        _kitchenGameManager.TestCountdownToStartTimer = 0;

        _kitchenGameManager.UpdateCountdownToStart();

        Assert.AreEqual(KitchenGameManager.State.GamePlaying, _kitchenGameManager.TestGameState);
    }

    [Test]
    public void Update_GamePlayingTimerExpires_UpdatesStateToGameOver()
    {
        _kitchenGameManager.TestGameState = KitchenGameManager.State.GamePlaying;
        _kitchenGameManager.TestGamePlayingTimer = 0;

        _kitchenGameManager.UpdateGamePlaying();

        Assert.AreEqual(KitchenGameManager.State.GameOver, _kitchenGameManager.TestGameState);
    }
    
    [Test]
    public void TogglePauseGame_PausesAndUnpausesGame()
    {
        _kitchenGameManager.TogglePauseGame(); // Pause the game
        Assert.IsTrue(_kitchenGameManager.TestIsGamePaused);

        _kitchenGameManager.TogglePauseGame(); // Unpause the game
        Assert.IsFalse(_kitchenGameManager.TestIsGamePaused);
    }
}
