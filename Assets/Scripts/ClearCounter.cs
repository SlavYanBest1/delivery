using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter {

   

   [SerializeField] private KitchenObjectSO kitchenObjectSO; 
   
   public override void Interact(Player player) {
      if (!HasKitchenObject()) {
          // если нет предмета
          if (player.HasKitchenObject()) { 
              // игрок что то несет 
              player.GetKitchenObject().SetKitchenObjectParent(this);
         }  else{
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
}
