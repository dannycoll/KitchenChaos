
public class DeliveryCounter : BaseCounter
{
  public override void Interact(Player player)
  {
    if (player.HasKitchenObject())
    {
      if (player.GetKitchenObject().TryGetComponent(out PlateKitchenObject plateKitchenObject))
      {
        DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
        player.GetKitchenObject().DestroySelf();
      }
    }
  }
}
