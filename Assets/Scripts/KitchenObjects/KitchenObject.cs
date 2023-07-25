using UnityEngine;

public class KitchenObject : MonoBehaviour
{
  [SerializeField] protected KitchenObjectSO kitchenObjectSO;

  private IKitchenObjectParent _kitchenObjectParent;

  public KitchenObjectSO GetKitchenObjectSO() => kitchenObjectSO;

  public void SetKitchenObjectParent(IKitchenObjectParent parent)
  {
    if (this._kitchenObjectParent != null)
    {
      this._kitchenObjectParent.ClearKitchenObject();
    }
    this._kitchenObjectParent = parent;
    this._kitchenObjectParent.SetKitchenObject(this);
    Transform transform1;
    (transform1 = transform).parent = this._kitchenObjectParent.GetKitchenObjectFollowTransform();
    transform1.localPosition = Vector3.zero;
  }

  public IKitchenObjectParent GetKitchenObjectParent()
  {
    return this._kitchenObjectParent;
  }

  public void DestroySelf()
  {
    _kitchenObjectParent.ClearKitchenObject();
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
