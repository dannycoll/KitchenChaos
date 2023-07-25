using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
  [SerializeField] private KitchenObjectSO kitchenObjectSO;

  private IKitchenObjectParent kitchenObjectParent;

  public KitchenObjectSO GetKitchenObjectSO() => kitchenObjectSO;

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

  public IKitchenObjectParent GetKitchenObjectParent()
  {
    return this.kitchenObjectParent;
  }

  public void DestroySelf()
  {
    kitchenObjectParent.ClearKitchenObject();
    Destroy(gameObject);
  }

  public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
  {
    Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
    KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

    kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
    return kitchenObject;
  }

  public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
  {
    if (this is PlateKitchenObject)
    {
      plateKitchenObject = this as PlateKitchenObject;
      return true;
    }
    plateKitchenObject = null;
    return false;
  }
}
