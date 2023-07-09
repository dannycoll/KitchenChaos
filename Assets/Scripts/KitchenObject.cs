using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
  [SerializeField] private KitchenObjectSO kitchenObjectSO;

  private IKitchenObjectParent kitchenObjectParent;

  public KitchenObjectSO GetKitchenObjectSO => kitchenObjectSO;

  public void SetKitchenObjectParent(IKitchenObjectParent parent)
  {
    if (this.kitchenObjectParent != null)
    {
      this.kitchenObjectParent.ClearKitchenObject();
    }
    this.kitchenObjectParent = parent;
    this.kitchenObjectParent.SetKitchenObject(this);
    transform.parent = this.kitchenObjectParent.GetKitchenObjectFollowTransform();
    transform.localPosition = Vector3.zero;
  }
  public IKitchenObjectParent GetKitchenObjectParent => kitchenObjectParent;

}
