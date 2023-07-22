using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour, IKitchenObjectParent
{
  public static Player Instance { get; private set; }
  public event EventHandler OnPickedUpKitchenObject;
  public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
  public class OnSelectedCounterChangedEventArgs : System.EventArgs
  {
    public BaseCounter selectedCounter;
  }

  [SerializeField] private float speed = 5f;
  [SerializeField] private GameInput gameInput;
  [SerializeField] private LayerMask countersLayerMask;
  [SerializeField] private Transform kitchenObjectHoldPoint;

  private KitchenObject kitchenObject;
  private Vector3 lastMoveDir;
  private BaseCounter selectedCounter;

  private bool isWalking;

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

  private void GameInput_OnInteractAction(object sender, System.EventArgs e)
  {
    if (selectedCounter != null)
    {
      selectedCounter.Interact(this);
    }
  }
  private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e)
  {
    if (selectedCounter != null)
    {
      selectedCounter.InteractAlternate(this);
    }
  }


  private void Update()
  {
    HandleMovement();
    HandleInteractions();
  }

  public bool IsWalking()
  {
    return isWalking;
  }

  private void HandleInteractions()
  {
    Vector2 inputVector = gameInput.GetMovementVectorNormalized();

    Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

    if (moveDir != Vector3.zero)
    {
      lastMoveDir = moveDir;
    }

    float interactDistance = 2f;
    if (Physics.Raycast(transform.position, lastMoveDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
    {
      if (raycastHit.transform.TryGetComponent<BaseCounter>(out BaseCounter counter))
      {
        if (counter != selectedCounter)
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

    transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * 10);

    float playerRadius = .7f;
    float playerHeight = 2f;
    float moveDistance = Time.deltaTime * speed;
    bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
    if (!canMove)
    {
      Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
      canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
      if (canMove)
      {
        moveDir = moveDirX;
      }
      else
      {
        Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
        canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
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

    isWalking = moveDir != Vector3.zero;
    transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * 10);
  }

  private void SetSelectedCounter(BaseCounter newSelectedCounter)
  {
    this.selectedCounter = newSelectedCounter;
    OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });
  }

  public Transform GetKitchenObjectFollowTransform()
  {
    return kitchenObjectHoldPoint;
  }

  public void SetKitchenObject(KitchenObject kitchenObject)
  {
    this.kitchenObject = kitchenObject;
    if (kitchenObject != null)
    {
      OnPickedUpKitchenObject?.Invoke(this, EventArgs.Empty);
    }
  }

  public void ClearKitchenObject()
  {
    this.kitchenObject = null;
  }

  public bool HasKitchenObject()
  {
    return kitchenObject != null;
  }

  public KitchenObject GetKitchenObject()
  {
    return this.kitchenObject;
  }
}
