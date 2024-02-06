using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter {

    public override void Interact(Player player) {
        //если игрок что то держит
        if (player.HasKitchenObject()) {
            player.GetKitchenObject().DestroySelf();
        }
    }
   
}
