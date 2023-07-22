using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class KitchenGameManager : MonoBehaviour
{
  public static KitchenGameManager Instance;

  public event EventHandler OnStateChanged;
  private enum State
  {
    WaitingToStart,
    CountdownToStart,
    GamePlaying,
    GameOver
  }

  private State state;
  private float waitingToStartTimer = 1f;
  private float countdownToStartTimer = 3f;
  private float gamePlayingTimer = 60f;
  private void Awake()
  {
    state = State.WaitingToStart;
    Instance = this;
  }

  private void Update()
  {
    switch (state)
    {
      case State.WaitingToStart:
        waitingToStartTimer -= Time.deltaTime;
        if (waitingToStartTimer <= 0)
        {
          state = State.CountdownToStart;
          OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
        break;
      case State.CountdownToStart:
        countdownToStartTimer -= Time.deltaTime;
        if (countdownToStartTimer <= 0)
        {
          state = State.GamePlaying;
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
}
