using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item thisItem;
    public Inventory PlayerInventory;

    public bool AddNewItem() {
        if(!PlayerInventory.itemList.Contains(thisItem))
        {
            if(PlayerInventory.itemList.Count <= 5){
                thisItem.itemHeld += 1;
                InventoryManager.CreateNewItem(thisItem, PlayerInventory.itemList.Count);
                PlayerInventory.itemList.Add(thisItem);
            }else{
                return false;
            }
        }else{
            thisItem.itemHeld += 1;
            InventoryManager.RefreshItem();
        }

        return true;
    }
}
