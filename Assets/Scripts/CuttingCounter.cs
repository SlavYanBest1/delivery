using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter {


    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
          // если нет предмета
          if (player.HasKitchenObject()) { 
              //если есть рецепт с входными данными то нарезаем
              // игрок что то несет 
              if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {           
                  player.GetKitchenObject().SetKitchenObjectParent(this);
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
            // То мы его разрезаем , если есть кухонный предмет и его можно нарезать
            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            
            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
        }
    }
       // запросы на входные и выходные данные
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
      foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
        if (cuttingRecipeSO.input == inputKitchenObjectSO) {
          return true;
        }
      }
      return false;
    }
       // запросы на входные и выходные данные
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
      foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
        if (cuttingRecipeSO.input == inputKitchenObjectSO) {
          return cuttingRecipeSO.output;
        }
      }
      return null;
    }

}
