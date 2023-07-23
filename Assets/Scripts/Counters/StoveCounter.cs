using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoveCounter : BaseCounter, IHasProgress
{
  public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
  public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

  public class OnStateChangedEventArgs : EventArgs
  {
    public State state;
  }
  public enum State
  {
    Idle,
    Frying,
    Fried,
    Burned
  }

  [SerializeField] private FryingRecipeSO[] fryingRecipes;
  [SerializeField] private BurningRecipeSO[] burningRecipes;

  private State state;
  private float fryingTimer;
  private float burningTimer;
  private FryingRecipeSO fryingRecipe;
  private BurningRecipeSO burningRecipe;
  private void Start()
  {
    state = State.Idle;
  }
  private void Update()
  {
    switch (state)
    {
      case State.Idle:
        break;
      case State.Frying:
        Fry();
        break;
      case State.Fried:
        Burn();
        break;
      case State.Burned:
        break;
    }

  }

  private void Fry()
  {
    if (HasKitchenObject())
    {
      fryingTimer += Time.deltaTime;

      if (fryingTimer > fryingRecipe.fryingTimeNeeded)
      {
        fryingTimer = 0;
        GetKitchenObject().DestroySelf();
        KitchenObject.SpawnKitchenObject(fryingRecipe.output, this);
        ChangeState(State.Fried);
        burningTimer = 0;
        burningRecipe = GetBurningRecipeForInput(GetKitchenObject().GetKitchenObjectSO());
      }
      OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
      {
        progressNormalized = fryingTimer / fryingRecipe.fryingTimeNeeded
      });
    }
  }

  private void ChangeState(State state)
  {
    this.state = state;
    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
    {
      state = state
    });
  }
  private void Burn()
  {
    if (HasKitchenObject())
    {
      burningTimer += Time.deltaTime;
      if (burningTimer > burningRecipe.fryingTimeNeeded)
      {
        burningTimer = 0;
        GetKitchenObject().DestroySelf();
        KitchenObject.SpawnKitchenObject(burningRecipe.output, this);
        ChangeState(State.Burned);
      }
      OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
      {
        progressNormalized = burningTimer / burningRecipe.fryingTimeNeeded
      });
    }
  }
  public override void Interact(Player player)
  {
    if (!HasKitchenObject())
    {
      if (player.HasKitchenObject())
      {
        if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
        {
          player.GetKitchenObject().SetKitchenObjectParent(this);
          this.fryingRecipe = GetFryingRecipeForInput(GetKitchenObject().GetKitchenObjectSO());
          fryingTimer = 0;
          ChangeState(State.Frying);
          OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
          {
            progressNormalized = fryingTimer / fryingRecipe.fryingTimeNeeded
          });
        }
      }
    }
    else
    {
      if (!player.HasKitchenObject())
      {
        GetKitchenObject().SetKitchenObjectParent(player);
        ChangeState(State.Idle);
        fryingTimer = 0;
        burningTimer = 0;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
          progressNormalized = fryingTimer / fryingRecipe.fryingTimeNeeded
        });
      }
      else
      {
        if (player.GetKitchenObject().TryGetComponent(out PlateKitchenObject plateKitchenObject))
        {
          if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
          {
            GetKitchenObject().DestroySelf();
            ChangeState(State.Idle);
            fryingTimer = 0;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
              progressNormalized = fryingTimer / fryingRecipe.fryingTimeNeeded
            });
          }
        }
      }
    }
  }

  private bool HasRecipeWithInput(KitchenObjectSO input)
  {
    var fryingRecipe = GetFryingRecipeForInput(input);
    return fryingRecipe != null;
  }
  private KitchenObjectSO GetOutputForInput(KitchenObjectSO input)
  {
    var fryingRecipe = GetFryingRecipeForInput(input);
    if (fryingRecipe != null)
      return fryingRecipe.output;
    return null;
  }

  private FryingRecipeSO GetFryingRecipeForInput(KitchenObjectSO input)
  {
    foreach (FryingRecipeSO item in fryingRecipes)
    {
      if (item.input == input)
      {
        return item;
      }
    }
    return null;
  }

  private BurningRecipeSO GetBurningRecipeForInput(KitchenObjectSO input)
  {
    foreach (BurningRecipeSO item in burningRecipes)
    {
      if (item.input == input)
      {
        return item;
      }
    }
    return null;
  }

  public bool IsFried()
  {
    return state == State.Fried;
  }
}
