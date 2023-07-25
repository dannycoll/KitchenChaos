using UnityEngine;
using System;

public class StoveCounter : BaseCounter, IHasProgress
{
  public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
  public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

  public class OnStateChangedEventArgs : EventArgs
  {
    public State State;
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

  private State _state;
  private float _fryingTimer;
  private float _burningTimer;
  private FryingRecipeSO _fryingRecipe;
  private BurningRecipeSO _burningRecipe;
  private void Start()
  {
    _state = State.Idle;
  }
  private void Update()
  {
    switch (_state)
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
      _fryingTimer += Time.deltaTime;

      if (_fryingTimer > _fryingRecipe.fryingTimeNeeded)
      {
        _fryingTimer = 0;
        GetKitchenObject().DestroySelf();
        KitchenObject.SpawnKitchenObject(_fryingRecipe.output, this);
        ChangeState(State.Fried);
        _burningTimer = 0;
        _burningRecipe = GetBurningRecipeForInput(GetKitchenObject().GetKitchenObjectSO());
      }
      OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
      {
        ProgressNormalized = _fryingTimer / _fryingRecipe.fryingTimeNeeded
      });
    }
  }

  private void ChangeState(State state)
  {
    this._state = state;
    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
    {
      State = state
    });
  }
  private void Burn()
  {
    if (HasKitchenObject())
    {
      _burningTimer += Time.deltaTime;
      if (_burningTimer > _burningRecipe.fryingTimeNeeded)
      {
        _burningTimer = 0;
        GetKitchenObject().DestroySelf();
        KitchenObject.SpawnKitchenObject(_burningRecipe.output, this);
        ChangeState(State.Burned);
      }
      OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
      {
        ProgressNormalized = _burningTimer / _burningRecipe.fryingTimeNeeded
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
          this._fryingRecipe = GetFryingRecipeForInput(GetKitchenObject().GetKitchenObjectSO());
          _fryingTimer = 0;
          ChangeState(State.Frying);
          OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
          {
            ProgressNormalized = _fryingTimer / _fryingRecipe.fryingTimeNeeded
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
        _fryingTimer = 0;
        _burningTimer = 0;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
          ProgressNormalized = _fryingTimer / _fryingRecipe.fryingTimeNeeded
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
            _fryingTimer = 0;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
              ProgressNormalized = _fryingTimer / _fryingRecipe.fryingTimeNeeded
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
    return _state == State.Fried;
  }
}
