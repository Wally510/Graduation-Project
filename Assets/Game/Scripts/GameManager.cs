using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Inventory myBag;

    public List<Item> items = new List<Item>();

    private void Awake() {
        myBag.itemList.Clear();
        for(int i = 0; i < items.Count; i++){
            items[i].itemHeld = 0;
        }
        InventoryManager.RefreshItem();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //  按ESC退出全屏
        if (Input.GetKey(KeyCode.Escape))
        {
            Screen.SetResolution(960, 540, false);       
        }
    }
}
