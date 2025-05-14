using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGet : MonoBehaviour
{
    public GameObject Packpack;
    public Sprite MyImage;
    public GameObject Mything;
    
    public void GetThings () {
        Mything.GetComponent<UnityEngine.UI.Image>().sprite = MyImage;
        for(int i = 0; i < 5; i++) {
            if(Packpack.transform.GetChild(i).childCount == 0){
                GameObject instance = Instantiate(Mything);
                instance.transform.SetParent(Packpack.transform.GetChild(i));
                instance.transform.localPosition = Vector3.zero;
                break;
            }
        }
    }
}
