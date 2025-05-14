using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonE : NetworkBehaviour
{
    public GameObject ButtonE;
    public bool IsLook = false;
    public enum WhichDo{
        Get,Open,Write
    }
    public WhichDo whichDo;

    private void Awake() {
        ButtonE = GameObject.FindWithTag("KeyE").transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsLook){
            ButtonE.GetComponent<RectTransform>().anchoredPosition = transform.GetComponent<UIFollow>().GetScreenPosition(transform.position);

            if(Input.GetKeyDown(KeyCode.E)){
                if(whichDo == WhichDo.Get)
                {
                    bool e = transform.GetComponent<ItemOnWorld>().AddNewItem();

                    if(e){
                        ButtonE.transform.gameObject.SetActive(false);
                        CmdDestroy();
                        Destroy(gameObject);
                    }
                }else if(whichDo == WhichDo.Open)
                {
                    transform.GetComponent<OnClickItem>().onclick();
                }else if(whichDo == WhichDo.Write)
                {
                    transform.GetComponent<OnClickItem>().onclick();
                    transform.GetComponent<OnClickItem>().Write();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Character cc = other.gameObject.GetComponent<Character>();
        if(other.tag == "Player" && cc != null && cc.isLocalPlayer){
            IsLook = true;
            ButtonE.transform.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Character cc = other.gameObject.GetComponent<Character>();
        if(other.tag == "Player" && cc != null && cc.isLocalPlayer){
            IsLook = false;
            ButtonE.transform.gameObject.SetActive(false);
        }
    }

    [Command(requiresAuthority=false)]
    void CmdDestroy()
    {
        RpcDestroy();
        Destroy(gameObject);
    }

    [ClientRpc]
    void RpcDestroy()
    {
        Destroy(gameObject);
    }
}
