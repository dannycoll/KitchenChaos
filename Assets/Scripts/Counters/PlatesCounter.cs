using UnityEngine;
using System;

public class PlatesCounter : BaseCounter
{
    public event EventHandler                  OnPlateSpawned;
    public event EventHandler                  OnPlateRemoved;
    [SerializeField] protected KitchenObjectSO plateKitchenObjectSO;
    protected                  float           SpawnPlateTimer;
    private const              float           SpawnPlateTimerMax = 4f;
    protected                  int             PlateSpawnedAmount;
    private const              int             PlateCapacity = 5;

    private void Update()
    {
        HandlePlateSpawns();
    }

    public void HandlePlateSpawns()
    {
        SpawnPlateTimer += Time.deltaTime;
        if (SpawnPlateTimer < SpawnPlateTimerMax) return;
        SpawnPlateTimer = 0f;
        if (!KitchenGameManager.Instance.IsGamePlaying() || PlateSpawnedAmount >= PlateCapacity) return;
        PlateSpawnedAmount++;
        OnPlateSpawned?.Invoke(this, EventArgs.Empty);
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (PlateSpawnedAmount > 0)
            {
                PlateSpawnedAmount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}