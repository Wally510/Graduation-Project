using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class Slot : NetworkBehaviour
{
    public bool click;
    public  Item slotItem;
    public Image slotImage;
    public Text slotNum;
    private GameObject localPlayer;

    public void ItemOnClicked() {

        if(slotItem.itemNumber >= 7 && slotItem.itemNumber <= 9){
            localPlayer = NetworkClient.connection.identity.gameObject;
            if(slotItem.itemNumber == 7){
                localPlayer.GetComponent<PlayerSkill>().Skill1click();
            }else if(slotItem.itemNumber == 8){
                localPlayer.GetComponent<PlayerSkill>().Skill2click();
            }else if(slotItem.itemNumber == 9){
                localPlayer.GetComponent<PlayerSkill>().Skill3click();
            }
            InventoryManager.MinusItem(slotItem, 1);
            InventoryManager.RefreshItem();
            return;
        }else if(slotItem.itemNumber >= 10){
            return;
        }
        //显示3D模型
        GameObject.FindWithTag("RawImage").transform.GetChild(0).gameObject.SetActive(click);

        //添加字幕说明
        InventoryManager.UpdateItemInfo(slotItem.itemInfo);

        //获取3D模型于摄像机
        GameObject targetcamera =  GameObject.FindWithTag("Target").transform.GetChild(0).gameObject;
        GameObject target =  GameObject.FindWithTag("Target").transform.GetChild(1).gameObject;

        //更换3D模型
        targetcamera.GetComponent<ChinarSmoothUi3DCamera>().pivot = target.transform.GetChild(slotItem.itemNumber);
        targetcamera.GetComponent<ChinarSmoothUi3DCamera>().targetDistance = slotItem.itemDistance;
        targetcamera.GetComponent<ChinarSmoothUi3DCamera>().pivotOffset = slotItem.itemOffset;

        for(int i = 0; i < target.transform.childCount; i++) {
            target.transform.GetChild(i).gameObject.SetActive(false);
        }
        target.transform.GetChild(slotItem.itemNumber).gameObject.SetActive(true);

        
    }
}
