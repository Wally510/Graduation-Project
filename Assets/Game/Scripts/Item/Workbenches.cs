using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Workbenches : MonoBehaviour
{
    public  List<Item> items = new List<Item>();
    public Inventory PlayerBag;
    public GameObject material1;
    public GameObject material2;
    public GameObject material3;
    public GameObject getitem;
    public Text gettext;

    public void Getclick(int gi, int mi1, int mi2, int mi3, int mj1, int mj2, int mj3)
    {
        getitem.transform.GetChild(0).gameObject.SetActive(true);
        getitem.transform.GetChild(0).GetComponent<Image>().sprite = items[gi].itemImage;
        gettext.text = items[gi].itemInfo;

        if(mi1 >= 0){
            for(int i = 0; i <= 3; i++) {
                material1.transform.GetChild(i).gameObject.SetActive(true);
            }

            material1.transform.GetChild(0).GetComponent<Image>().sprite = items[mi1].itemImage;
            material1.transform.GetChild(1).GetComponent<Text>().text = items[mi1].itemHeld.ToString();
            material1.transform.GetChild(3).GetComponent<Text>().text = mj1.ToString();

            if(items[mi1].itemHeld >= mj1){
                material1.transform.GetChild(1).GetComponent<Text>().color = Color.yellow;
            }else{
                material1.transform.GetChild(1).GetComponent<Text>().color = Color.red;
            }
        }else{
            for(int i = 0; i <= 3; i++) {
                material1.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        if(mi2 >= 0){
            for(int i = 0; i <= 3; i++) {
                material2.transform.GetChild(i).gameObject.SetActive(true);
            }

            material2.transform.GetChild(0).GetComponent<Image>().sprite = items[mi2].itemImage;
            material2.transform.GetChild(1).GetComponent<Text>().text = items[mi2].itemHeld.ToString();
            material2.transform.GetChild(3).GetComponent<Text>().text = mj2.ToString();

            if(items[mi2].itemHeld >= mj2){
                material2.transform.GetChild(1).GetComponent<Text>().color = Color.yellow;
            }else{
                material2.transform.GetChild(1).GetComponent<Text>().color = Color.red;
            }
        }else{
            for(int i = 0; i <= 3; i++) {
                material2.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        if(mi3 >= 0){
            for(int i = 0; i <= 3; i++) {
                material3.transform.GetChild(i).gameObject.SetActive(true);
            }

            material3.transform.GetChild(0).GetComponent<Image>().sprite = items[mi3].itemImage;
            material3.transform.GetChild(1).GetComponent<Text>().text = items[mi3].itemHeld.ToString();
            material3.transform.GetChild(3).GetComponent<Text>().text = mj3.ToString();

            if(items[mi3].itemHeld >= mj3){
                material3.transform.GetChild(1).GetComponent<Text>().color = Color.yellow;
            }else{
                material3.transform.GetChild(1).GetComponent<Text>().color = Color.red;
            }
        }else{
            for(int i = 0; i <= 3; i++) {
                material3.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void Makeclick(int gi, int mi1, int mi2, int mi3, int mj1, int mj2, int mj3) {
        bool ismake = true;
        if(PlayerBag.itemList.Count <= 5){
            if(mi1 >= 0){
                if(items[mi1].itemHeld < mj1){
                    ismake = false;
                }
            }

            if(mi2 >= 0){
                if(items[mi2].itemHeld < mj2){
                    ismake = false;
                }
            }

            if(mi3 >= 0){
                if(items[mi3].itemHeld < mj3){
                    ismake = false;
                }
            }
        }else{
            ismake = false;
        }

        if(ismake){
            if(mi1 >= 0){
                InventoryManager.MinusItem(items[mi1], mj1);
            }
            if(mi2 >= 0){
                InventoryManager.MinusItem(items[mi2], mj2);
            }
            if(mi3 >= 0){
                InventoryManager.MinusItem(items[mi3], mj3);
            }
            InventoryManager.AddItem(items[gi]);
            InventoryManager.RefreshItem();
        }
    }
}
