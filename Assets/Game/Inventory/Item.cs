using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
public class Item : ScriptableObject
{
    public int itemNumber;
    public float itemDistance;
    public Vector3 itemOffset = Vector3.zero;
    public string itemName;
    public Sprite itemImage;
    //public GameObject itemGameObject;
    public int itemHeld;
    [TextArea]
    public string itemInfo;
}
