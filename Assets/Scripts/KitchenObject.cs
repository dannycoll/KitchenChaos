using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
  [SerializeField] private KitchenObjectSO kitchenObjectSO;

  private ClearCounter clearCounter;

  public KitchenObjectSO GetKitchenObjectSO => kitchenObjectSO;

  public void SetClearCounter(ClearCounter clearCounter)
  {
    if (this.clearCounter != null)
    {
      this.clearCounter.ClearKitchenObject();
    }
    this.clearCounter = clearCounter;
    transform.parent = this.clearCounter.GetCounterTopPoint();
    transform.localPosition = Vector3.zero;
  }
  public ClearCounter GetClearCounter => clearCounter;

}
