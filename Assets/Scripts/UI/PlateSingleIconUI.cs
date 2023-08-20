using UnityEngine;
using UnityEngine.UI;

public class PlateSingleIconUI : MonoBehaviour
{
  [SerializeField] protected Image image;
  public void SetKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
  {
    image.sprite = kitchenObjectSO.sprite;
  }
}
