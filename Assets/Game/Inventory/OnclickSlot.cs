using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnclickSlot : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            if(transform.GetChild(0).childCount != 0){
                transform.GetChild(0).GetChild(0).GetComponent<Slot>().ItemOnClicked();
            }
        }else if(Input.GetKeyDown(KeyCode.Alpha2)){
            if(transform.GetChild(1).childCount != 0){
                transform.GetChild(1).GetChild(0).GetComponent<Slot>().ItemOnClicked();
            }
        }else if(Input.GetKeyDown(KeyCode.Alpha3)){
            if(transform.GetChild(2).childCount != 0){
                transform.GetChild(2).GetChild(0).GetComponent<Slot>().ItemOnClicked();
            }
        }else if(Input.GetKeyDown(KeyCode.Alpha4)){
            if(transform.GetChild(3).childCount != 0){
                transform.GetChild(3).GetChild(0).GetComponent<Slot>().ItemOnClicked();
            }
        }else if(Input.GetKeyDown(KeyCode.Alpha5)){
            if(transform.GetChild(4).childCount != 0){
                transform.GetChild(4).GetChild(0).GetComponent<Slot>().ItemOnClicked();
            }
        }
    }
}
