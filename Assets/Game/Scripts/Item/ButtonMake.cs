using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMake : MonoBehaviour
{
    public int pagei;
    public Transform page;
    public void onClick(){
        page.GetChild(pagei).GetChild(0).GetComponent<Lookitem>().onMakeclick();
    }
}
