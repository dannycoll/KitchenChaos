using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  [SerializeField] private float speed = 5f;
  [SerializeField] private GameInput gameInput;
  [SerializeField] private LayerMask countersLayerMask;
  private Vector3 lastMoveDir;
  private LayerMask playerLayerMask;
  private LayerMask floorLayerMask;
  private LayerMask wallsLayerMask;

  private bool isWalking;

  private void Update()
  {
    HandleMovement();
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
    if (Physics.Raycast(transform.position, moveDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
    {
      if (raycastHit.transform.TryGetComponent<ClearCounter>(out ClearCounter clearCounter))
      {
        clearCounter.Interact();
      }
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
}
