using UnityEngine;

public interface IKitchenObjectParent
{
  public void SetKitchenObject(KitchenObject kitchenObject);

  public void ClearKitchenObject();

  public bool HasKitchenObject();

  public Transform GetKitchenObjectFollowTransform();

  public KitchenObject GetKitchenObject();

}
