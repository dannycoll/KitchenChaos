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
  private float _countdownToStartTimer = 3f;
  protected float GamePlayingTimer;
  private const float GamePlayingTimerMax = 60f;
  private bool _isGamePaused;

  private void Awake()
  {
    GameState = State.WaitingToStart;
    Instance = this;
    _isGamePaused = false;
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
        _countdownToStartTimer -= Time.deltaTime;
        if (_countdownToStartTimer <= 0)
        {
          GameState = State.GamePlaying;
          GamePlayingTimer = GamePlayingTimerMax;
          OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
        break;
      case State.GamePlaying:
        GamePlayingTimer -= Time.deltaTime;
        if (GamePlayingTimer <= 0)
        {
          GameState = State.GameOver;
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
    return GameState == State.GamePlaying;
  }

  public bool IsCountdownToStartActive()
  {
    return GameState == State.CountdownToStart;
  }

  public float GetCountdownToStartTimer()
  {
    return _countdownToStartTimer;
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
    if (_isGamePaused)
    {
      Time.timeScale = 1;
      _isGamePaused = false;
      OnGameUnpaused?.Invoke(this, EventArgs.Empty);
    }
    else
    {
      Time.timeScale = 0;
      _isGamePaused = true;
      OnGamePaused?.Invoke(this, EventArgs.Empty);
    }
  }

}
