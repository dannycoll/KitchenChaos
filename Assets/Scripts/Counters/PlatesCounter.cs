using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatesCounter : BaseCounter
{
  public event EventHandler OnPlateSpawned;
  public event EventHandler OnPlateRemoved;
  [SerializeField] KitchenObjectSO plateKitchenObjectSO;
  private float spawnPlateTimer;
  private const float SPAWN_PLATE_TIMER = 4f;
  private int plateSpawnedAmount = 0;
  private int PLATE_CAPACITY = 5;

  private void Update()
  {
    spawnPlateTimer += Time.deltaTime;
    if (spawnPlateTimer > SPAWN_PLATE_TIMER)
    {
      spawnPlateTimer = 0f;
      if (plateSpawnedAmount < PLATE_CAPACITY)
      {
        plateSpawnedAmount++;
        OnPlateSpawned?.Invoke(this, EventArgs.Empty);
      }
    }
  }

  public override void Interact(Player player)
  {
    if (!player.HasKitchenObject())
    {
      if (plateSpawnedAmount > 0)
      {
        plateSpawnedAmount--;
        KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
        OnPlateRemoved?.Invoke(this, EventArgs.Empty);
      }
    }
  }
}
