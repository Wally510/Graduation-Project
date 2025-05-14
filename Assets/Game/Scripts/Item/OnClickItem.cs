using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickItem : MonoBehaviour
{
    public int number;
    public bool click;
    [TextArea]
    public string write;
    public void onclick(){
        GameObject.FindWithTag("RawImage").transform.GetChild(number).gameObject.SetActive(click);
    }

    public void Write() {
        GameObject.FindWithTag("RawImage").transform.GetChild(number).GetChild(1).GetComponent<Text>().text = write;
    }
}
