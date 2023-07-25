using UnityEngine;
using System;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
  public delegate void AnyObjectPlacedEventHandler(object sender, EventArgs e);
  public static AnyObjectPlacedEventHandler OnAnyObjectPlaced;

  private void Start()
  {
    _isCounterTopPointNotNull = counterTopPoint != null;
  }

  public static void ResetStaticData()
  {
    OnAnyObjectPlaced = null;
  }
  [SerializeField] protected Transform counterTopPoint;

  private KitchenObject _kitchenObject;
  private bool _isCounterTopPointNotNull;

  public virtual void Interact(Player player) {}

  public virtual void InteractAlternate(Player player) {}

  public Transform GetKitchenObjectFollowTransform()
  {
    return counterTopPoint;
  }

  public void SetKitchenObject(KitchenObject kitchenObject)
  {
    this._kitchenObject = kitchenObject;

    // Check if counterTopPoint is not null before invoking the event
    if (_isCounterTopPointNotNull && kitchenObject != null)
    {
      OnAnyObjectPlaced?.Invoke(this, EventArgs.Empty);
    }
  }

  public void ClearKitchenObject()
  {
    this._kitchenObject = null;
  }

  public bool HasKitchenObject()
  {
    return this._kitchenObject != null;
  }

  public KitchenObject GetKitchenObject()
  {
    return this._kitchenObject;
  }
}
