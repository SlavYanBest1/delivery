using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter {

    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs {
      public float progressNormalized;
    }

    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress; 

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
          // если нет предмета
          if (player.HasKitchenObject()) { 
              //если есть рецепт с входными данными то нарезаем
              // игрок что то несет 
              if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {           
                  player.GetKitchenObject().SetKitchenObjectParent(this);
                  cuttingProgress = 0;

                  CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                  // запуск события
                  OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs{
                    progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                  }); 
              } 
          }  else {
            //если не несет, то у игрока ничего нет
          }
        } else {
          // если есть 
          if (player.HasKitchenObject()) {
            //если есть, то он что то несет
          } else {
            //если у игрока ничего нет, то мы отадем с собой
            GetKitchenObject().SetKitchenObjectParent(player);
          }
      }
    }

    public override void InteractAlternate(Player player) {
        // если уже есть нарезанный предмет, то не нарезаем снов
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
            cuttingProgress++; //увелич процент резки на 1+    

            OnCut?.Invoke(this, EventArgs.Empty); 
                
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs{
                    progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                  }); 

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {
            // То мы его разрезаем , если есть кухонный предмет и его можно нарезать
            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            
            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
          }
      }
    }
       // возвращает логическое значение 
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
      CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
      return cuttingRecipeSO != null;
    }
       // запросы на входные и выходные данные
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
      CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
      if (cuttingRecipeSO != null) {
        return cuttingRecipeSO.output;
      } else {
        return null;
      }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) { 
      foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
        if (cuttingRecipeSO.input == inputKitchenObjectSO) {
          return cuttingRecipeSO;
        }
      }
      return null;
    }

}
