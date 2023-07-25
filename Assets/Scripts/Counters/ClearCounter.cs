
public class ClearCounter : BaseCounter
{
  public override void Interact(Player player)
  {
    if (!HasKitchenObject())
    {
      if (player.HasKitchenObject())
      {
        player.GetKitchenObject().SetKitchenObjectParent(this);
      }
    }
    else
    {
      if (!player.HasKitchenObject())
      {
        GetKitchenObject().SetKitchenObjectParent(player);
      }
      else
      {
        if (player.GetKitchenObject().TryGetComponent(out PlateKitchenObject plateKitchenObject))
        {
          if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
          {
            GetKitchenObject().DestroySelf();
          }
        }
        else
        {
          if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
          {
            if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
            {
              player.GetKitchenObject().DestroySelf();
            }
          }
        }
      }
    }
  }
}
