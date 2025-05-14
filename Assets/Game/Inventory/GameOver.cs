using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Inventory PlayerBag;
    public Image image1;
    public Image image2;
    public Image image3;
    public Image image4;
    public GameObject getitem1;
    public GameObject getitem2;
    public GameObject getitem3;
    public GameObject getitem4;

    public void Onclick(int n) {
        for(int i = 0; i < PlayerBag.itemList.Count; i++) {
            if(PlayerBag.itemList[i].itemNumber == n) {
                InventoryManager.MinusItem(PlayerBag.itemList[i], 1);
                InventoryManager.RefreshItem();
                break;
            }
        }
        if(n == 10){
            image1.color = new Color(0, 0, 0, 1);
        }else if(n == 11){
            image2.color = new Color(0, 0, 0, 1);
        }else if(n == 12){
            image3.color = new Color(0, 0, 0, 1);
        }else if(n == 13){
            image4.color = new Color(0, 0, 0, 1);
        }
    }
}
