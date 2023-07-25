using UnityEngine;
using System;

public class Player : MonoBehaviour, IKitchenObjectParent
{
  public static Player Instance { get; private set; }
  public event EventHandler OnPickedUpKitchenObject;
  public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
  public class OnSelectedCounterChangedEventArgs : EventArgs
  {
    public BaseCounter SelectedCounter;
  }

  [SerializeField] private float speed = 5f;
  [SerializeField] private GameInput gameInput;
  [SerializeField] private LayerMask countersLayerMask;
  [SerializeField] private Transform kitchenObjectHoldPoint;

  private KitchenObject _kitchenObject;
  private Vector3 _lastMoveDir;
  private BaseCounter _selectedCounter;

  private bool _isWalking;

  private void Awake()
  {
    if (Instance != null)
    {
      Destroy(gameObject);
      return;
    }
    Instance = this;
  }

  private void Start()
  {
    gameInput.OnInteractAction += GameInput_OnInteractAction;
    gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
  }

  private void GameInput_OnInteractAction(object sender, EventArgs e)
  {
    if (!KitchenGameManager.Instance.IsGamePlaying()) return;
    if (_selectedCounter != null)
    {
      _selectedCounter.Interact(this);
    }
  }
  private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
  {
    if (!KitchenGameManager.Instance.IsGamePlaying()) return;
    if (_selectedCounter != null)
    {
      _selectedCounter.InteractAlternate(this);
    }
  }


  private void Update()
  {
    HandleMovement();
    HandleInteractions();
  }

  public bool IsWalking()
  {
    return _isWalking;
  }

  private void HandleInteractions()
  {
    Vector2 inputVector = gameInput.GetMovementVectorNormalized();

    Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

    if (moveDir != Vector3.zero)
    {
      _lastMoveDir = moveDir;
    }

    float interactDistance = 2f;
    if (Physics.Raycast(transform.position, _lastMoveDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
    {
      if (raycastHit.transform.TryGetComponent(out BaseCounter counter))
      {
        if (counter != _selectedCounter)
        {
          SetSelectedCounter(counter);
        }
      }
      else
      {
        SetSelectedCounter(null);
      }
    }
    else
    {
      SetSelectedCounter(null);
    }
  }

  private void HandleMovement()
  {
    Vector2 inputVector = gameInput.GetMovementVectorNormalized();

    Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

    Transform transform1;
    (transform1 = transform).forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * 10);

    float playerRadius = .7f;
    float playerHeight = 2f;
    float moveDistance = Time.deltaTime * speed;
    var position = transform1.position;
    bool canMove = !Physics.CapsuleCast(position, position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
    if (!canMove)
    {
      Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
      canMove = Math.Abs(moveDir.x) > 0.3f && !Physics.CapsuleCast(position, position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
      if (canMove)
      {
        moveDir = moveDirX;
      }
      else
      {
        Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
        canMove = Math.Abs(moveDir.z) > 0.3f && !Physics.CapsuleCast(position, position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
        if (canMove)
        {
          moveDir = moveDirZ;
        }
      }

    }
    if (canMove)
    {
      transform.position += moveDir * moveDistance;
    }

    _isWalking = moveDir != Vector3.zero;
    transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * 10);
  }

  private void SetSelectedCounter(BaseCounter newSelectedCounter)
  {
    this._selectedCounter = newSelectedCounter;
    OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { SelectedCounter = _selectedCounter });
  }

  public Transform GetKitchenObjectFollowTransform()
  {
    return kitchenObjectHoldPoint;
  }

  public void SetKitchenObject(KitchenObject kitchenObject)
  {
    this._kitchenObject = kitchenObject;
    if (kitchenObject != null)
    {
      OnPickedUpKitchenObject?.Invoke(this, EventArgs.Empty);
    }
  }

  public void ClearKitchenObject()
  {
    this._kitchenObject = null;
  }

  public bool HasKitchenObject()
  {
    return _kitchenObject != null;
  }

  public KitchenObject GetKitchenObject()
  {
    return this._kitchenObject;
  }
}
