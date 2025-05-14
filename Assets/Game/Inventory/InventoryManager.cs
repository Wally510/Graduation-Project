using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    static InventoryManager instance;

    public Inventory myBag;
    public GameObject slotGrid;
    public Slot slotPrefab;
    public Text itemInfromation;

    private void Awake() {
        if(instance != null)
            Destroy(this);
        instance = this;
    }

    private void OnEnable() {
        RefreshItem();
        instance.itemInfromation.text = "";
    }

    public static void UpdateItemInfo(string itemDescription){
        instance.itemInfromation.text = itemDescription;
    }

    public static void CreateNewItem(Item item, int i){
        Slot newItem = Instantiate(instance.slotPrefab, instance.slotGrid.transform.GetChild(i).position, Quaternion.identity);
        newItem.gameObject.transform.SetParent(instance.slotGrid.transform.GetChild(i));
        newItem.slotItem = item;
        newItem.slotImage.sprite = item.itemImage;
        newItem.slotNum.text = item.itemHeld.ToString();
    }

    public static void RefreshItem(){
        for(int i = 0; i < 5; i++) {
            if(instance.slotGrid.transform.GetChild(i).childCount == 0)
                continue;
            for(int j = 0; j < instance.slotGrid.transform.GetChild(i).childCount; j++) {
                Destroy(instance.slotGrid.transform.GetChild(i).GetChild(j).gameObject);
            }
        }

        for(int i = 0; i < instance.myBag.itemList.Count; i++) {
            CreateNewItem(instance.myBag.itemList[i], i);
        }
    }

    public static void MinusItem(Item item, int n){
        for(int i = 0; i < 5; i++) {
            if(instance.myBag.itemList[i] == item){
                instance.myBag.itemList[i].itemHeld -= n;
                if(instance.myBag.itemList[i].itemHeld <= 0){
                    instance.myBag.itemList.Remove(item);
                }
                break;
            }
        }
    }

    public static bool AddItem(Item item) {
        if(!instance.myBag.itemList.Contains(item))
        {
            if(instance.myBag.itemList.Count <= 5){
                CreateNewItem(item, instance.myBag.itemList.Count);
                instance.myBag.itemList.Add(item);
            }else{
                return false;
            }
        }
        
        item.itemHeld += 1;
        return true;
    }
}
