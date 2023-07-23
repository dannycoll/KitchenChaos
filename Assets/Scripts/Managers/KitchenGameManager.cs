using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class KitchenGameManager : MonoBehaviour
{
  public static KitchenGameManager Instance;

  public event EventHandler OnStateChanged;
  public event EventHandler OnGamePaused;
  public event EventHandler OnGameUnpaused;
  private enum State
  {
    WaitingToStart,
    CountdownToStart,
    GamePlaying,
    GameOver
  }

  private State state;
  private float countdownToStartTimer = 3f;
  private float gamePlayingTimer;
  private float gamePlayingTimerMax = 60f;
  private bool isGamePaused;

  private void Awake()
  {
    state = State.WaitingToStart;
    Instance = this;
    isGamePaused = false;
  }

  private void Start()
  {
    GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
  }

  private void GameInput_OnInteractAction(object sender, EventArgs e)
  {
    if (state == State.WaitingToStart)
    {
      state = State.CountdownToStart;
      OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

  }
  private void Update()
  {
    switch (state)
    {
      case State.WaitingToStart:

        break;
      case State.CountdownToStart:
        countdownToStartTimer -= Time.deltaTime;
        if (countdownToStartTimer <= 0)
        {
          state = State.GamePlaying;
          gamePlayingTimer = gamePlayingTimerMax;
          OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
        break;
      case State.GamePlaying:
        gamePlayingTimer -= Time.deltaTime;
        if (gamePlayingTimer <= 0)
        {
          state = State.GameOver;
          OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
        break;
    }
  }

  private void GameInput_OnPauseAction(object sender, EventArgs e)
  {
    TogglePauseGame();
  }
  public bool IsGamePlaying()
  {
    return state == State.GamePlaying;
  }

  public bool IsCountdownToStartActive()
  {
    return state == State.CountdownToStart;
  }

  public float GetCountdownToStartTimer()
  {
    return countdownToStartTimer;
  }

  public bool IsGameOver()
  {
    return state == State.GameOver;
  }

  public float GetGamePlayingTimerNormalized()
  {
    return 1 - (gamePlayingTimer / gamePlayingTimerMax);
  }

  public void TogglePauseGame()
  {
    if (isGamePaused)
    {
      Time.timeScale = 1;
      isGamePaused = false;
      OnGameUnpaused?.Invoke(this, EventArgs.Empty);
    }
    else
    {
      Time.timeScale = 0;
      isGamePaused = true;
      OnGamePaused?.Invoke(this, EventArgs.Empty);
    }
  }

}