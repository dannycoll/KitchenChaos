using UnityEngine;
using System;
public class KitchenGameManager : MonoBehaviour
{
  public static KitchenGameManager Instance;

  public event EventHandler OnStateChanged;
  public event EventHandler OnGamePaused;
  public event EventHandler OnGameUnpaused;

  public enum State
  {
    WaitingToStart,
    CountdownToStart,
    GamePlaying,
    GameOver
  }

  protected State GameState;
  protected float CountdownToStartTimer = 3f;
  protected float GamePlayingTimer;
  private const float GamePlayingTimerMax = 60f;
  protected bool IsGamePaused;

  private void Awake()
  {
    GameState = State.WaitingToStart;
    Instance = this;
    IsGamePaused = false;
  }

  private void Start()
  {
    GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
  }

  private void GameInput_OnInteractAction(object sender, EventArgs e)
  {
    if (GameState == State.WaitingToStart)
    {
      GameState = State.CountdownToStart;
      OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

  }
  private void Update()
  {
    switch (GameState)
    {
      case State.WaitingToStart:
        break;
      case State.CountdownToStart:
        UpdateCountdownToStart();
        break;
      case State.GamePlaying:
        UpdateGamePlaying();
        break;
    }
  }

  public void UpdateCountdownToStart()
  {
    CountdownToStartTimer -= Time.deltaTime;
    if (CountdownToStartTimer <= 0)
    {
      GameState = State.GamePlaying;
      GamePlayingTimer = GamePlayingTimerMax;
      OnStateChanged?.Invoke(this, EventArgs.Empty);
    }
  }

  public void UpdateGamePlaying()
  {
    GamePlayingTimer -= Time.deltaTime;
    if (GamePlayingTimer <= 0)
    {
      GameState = State.GameOver;
      OnStateChanged?.Invoke(this, EventArgs.Empty);
    }
  }

  private void GameInput_OnPauseAction(object sender, EventArgs e)
  {
    TogglePauseGame();
  }
  public bool IsGamePlaying()
  {
    return GameState == State.GamePlaying;
  }

  public bool IsCountdownToStartActive()
  {
    return GameState == State.CountdownToStart;
  }

  public float GetCountdownToStartTimer()
  {
    return CountdownToStartTimer;
  }

  public bool IsGameOver()
  {
    return GameState == State.GameOver;
  }

  public float GetGamePlayingTimerNormalized()
  {
    return 1 - (GamePlayingTimer / GamePlayingTimerMax);
  }

  public void TogglePauseGame()
  {
    if (IsGamePaused)
    {
      Time.timeScale = 1;
      IsGamePaused = false;
      OnGameUnpaused?.Invoke(this, EventArgs.Empty);
    }
    else
    {
      Time.timeScale = 0;
      IsGamePaused = true;
      OnGamePaused?.Invoke(this, EventArgs.Empty);
    }
  }

}
