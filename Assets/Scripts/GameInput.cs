using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
  private const string PLAYER_PREFS_BINDINGS = "PlayerBindings";
  public static GameInput Instance { get; private set; }

  public event EventHandler OnInteractAction;
  public event EventHandler OnInteractAlternateAction;
  public event EventHandler OnPauseAction;

  public enum Binding
  {
    MoveUp,
    MoveDown,
    MoveLeft,
    MoveRight,
    Interact,
    InteractAlternate,
    Pause

  }

  private PlayerInputActions playerInputActions;
  private void Awake()
  {
    Instance = this;
    playerInputActions = new PlayerInputActions();
    if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
    {
      playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
    }
    playerInputActions.Player.Enable();

    playerInputActions.Player.Interact.performed += Interact_Performed;
    playerInputActions.Player.InteractAlternate.performed += InteractAlternate_Performed;
    playerInputActions.Player.Pause.performed += Pause_Performed;

  }

  private void OnDestroy()
  {
    playerInputActions.Player.Interact.performed -= Interact_Performed;
    playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_Performed;
    playerInputActions.Player.Pause.performed -= Pause_Performed;

    playerInputActions.Dispose();
  }
  private void Pause_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
  {
    OnPauseAction?.Invoke(this, EventArgs.Empty);
  }
  private void Interact_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
  {
    OnInteractAction?.Invoke(this, EventArgs.Empty);
  }

  private void InteractAlternate_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
  {
    OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
  }

  public Vector2 GetMovementVectorNormalized()
  {
    Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

    inputVector = inputVector.normalized;

    return inputVector;
  }

  public string GetBindingText(Binding binding)
  {
    switch (binding)
    {
      case Binding.Interact:
        return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
      case Binding.InteractAlternate:
        return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
      case Binding.Pause:
        return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
      case Binding.MoveUp:
        return playerInputActions.Player.Move.bindings[1].ToDisplayString();
      case Binding.MoveDown:
        return playerInputActions.Player.Move.bindings[2].ToDisplayString();
      case Binding.MoveLeft:
        return playerInputActions.Player.Move.bindings[3].ToDisplayString();
      case Binding.MoveRight:
        return playerInputActions.Player.Move.bindings[4].ToDisplayString();
      default:
        return "";
    }
  }

  public void ChangeBinding(Binding binding, Action onActionRebound)
  {
    playerInputActions.Player.Disable();

    InputAction inputAction;
    int bindingIndex;

    switch (binding)
    {
      case Binding.Interact:
        inputAction = playerInputActions.Player.Interact;
        bindingIndex = 1;
        break;
      case Binding.InteractAlternate:
        inputAction = playerInputActions.Player.InteractAlternate;
        bindingIndex = 1;
        break;
      case Binding.Pause:
        inputAction = playerInputActions.Player.Pause;
        bindingIndex = 1;
        break;
      case Binding.MoveUp:
        inputAction = playerInputActions.Player.Move;
        bindingIndex = 1;
        break;
      case Binding.MoveDown:
        inputAction = playerInputActions.Player.Move;
        bindingIndex = 2;
        break;
      case Binding.MoveLeft:
        inputAction = playerInputActions.Player.Move;
        bindingIndex = 3;
        break;
      case Binding.MoveRight:
        inputAction = playerInputActions.Player.Move;
        bindingIndex = 4;
        break;
      default:
        inputAction = null;
        bindingIndex = 0;
        break;
    }
    inputAction.PerformInteractiveRebinding(bindingIndex)
      .OnComplete(callback =>
      {
        callback.Dispose();
        playerInputActions.Player.Enable();
        onActionRebound();

        PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
        PlayerPrefs.Save();
      }).Start();
  }
}
