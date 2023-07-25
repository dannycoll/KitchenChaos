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

  private State _state;
  private float _countdownToStartTimer = 3f;
  private float _gamePlayingTimer;
  private const float GamePlayingTimerMax = 60f;
  private bool _isGamePaused;

  private void Awake()
  {
    _state = State.WaitingToStart;
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
    if (_state == State.WaitingToStart)
    {
      _state = State.CountdownToStart;
      OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

  }
  private void Update()
  {
    switch (_state)
    {
      case State.WaitingToStart:

        break;
      case State.CountdownToStart:
        _countdownToStartTimer -= Time.deltaTime;
        if (_countdownToStartTimer <= 0)
        {
          _state = State.GamePlaying;
          _gamePlayingTimer = GamePlayingTimerMax;
          OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
        break;
      case State.GamePlaying:
        _gamePlayingTimer -= Time.deltaTime;
        if (_gamePlayingTimer <= 0)
        {
          _state = State.GameOver;
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
    return _state == State.GamePlaying;
  }

  public bool IsCountdownToStartActive()
  {
    return _state == State.CountdownToStart;
  }

  public float GetCountdownToStartTimer()
  {
    return _countdownToStartTimer;
  }

  public bool IsGameOver()
  {
    return _state == State.GameOver;
  }

  public float GetGamePlayingTimerNormalized()
  {
    return 1 - (_gamePlayingTimer / GamePlayingTimerMax);
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
