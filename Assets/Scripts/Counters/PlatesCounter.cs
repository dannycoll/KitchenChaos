using UnityEngine;
using System;

public class PlatesCounter : BaseCounter
{
  public event EventHandler OnPlateSpawned;
  public event EventHandler OnPlateRemoved;
  [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
  private float _spawnPlateTimer;
  private const float SpawnPlateTimer = 4f;
  private int _plateSpawnedAmount;
  private const int PlateCapacity = 5;

  private void Update()
  {
    _spawnPlateTimer += Time.deltaTime;
    if (_spawnPlateTimer > SpawnPlateTimer)
    {
      _spawnPlateTimer = 0f;
      if (KitchenGameManager.Instance.IsGamePlaying() && _plateSpawnedAmount < PlateCapacity)
      {
        _plateSpawnedAmount++;
        OnPlateSpawned?.Invoke(this, EventArgs.Empty);
      }
    }
  }

  public override void Interact(Player player)
  {
    if (!player.HasKitchenObject())
    {
      if (_plateSpawnedAmount > 0)
      {
        _plateSpawnedAmount--;
        KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
        OnPlateRemoved?.Invoke(this, EventArgs.Empty);
      }
    }
  }
}
