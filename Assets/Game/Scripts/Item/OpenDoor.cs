using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject ButtonE;
    public bool IsLook = false;
    public Inventory PlayerBag;
    public Item KeyItem;
    private bool isDoorOpen = false; // 用于判断门是否已经打开

    // Update is called once per frame
    void Update()
    {
        if(IsLook){
            ButtonE.GetComponent<RectTransform>().anchoredPosition = transform.GetComponent<UIFollow>().GetScreenPosition(transform.position);

            if(Input.GetKeyDown(KeyCode.E) && !isDoorOpen){
                // 设置门为打开状态
                isDoorOpen = true; 

                //判断背包里是否由钥匙
                for(int i = 0; i < 5; i++) {
                    if(PlayerBag.itemList[i] == KeyItem){
                        //打开门
                        transform.Rotate(0, 90, 0);
                        //删除钥匙
                        PlayerBag.itemList[i].itemHeld -= 1;
                        InventoryManager.RefreshItem();
                        break;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"){
            IsLook = true;
            ButtonE.transform.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player"){
            IsLook = false;
            ButtonE.transform.gameObject.SetActive(false);
        }
    }
}
