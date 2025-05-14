using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonE : MonoBehaviour
{
    public GameObject Buttone;
    public bool IsLook = false;

    // Update is called once per frame
    void Update()
    {
        if(IsLook){
            Buttone.GetComponent<RectTransform>().anchoredPosition = transform.GetComponent<UIFollow>().GetScreenPosition(transform.position);

            if(Input.GetKeyDown(KeyCode.E)){
                
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"){
            IsLook = true;
            Buttone.transform.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player"){
            IsLook = false;
            Buttone.transform.gameObject.SetActive(false);
        }
    }
}
