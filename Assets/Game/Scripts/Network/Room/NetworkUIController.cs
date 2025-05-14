using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Discovery;
using UnityEngine;

public class NetworkUIController : MonoBehaviour
{
    public GameObject Panel;
    public GameObject roomPanel;


    public void Button_Quit(){
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void StartGame() {
        Panel.SetActive(false);
        roomPanel.SetActive(true);
    }

    public void ReturnGame() {
        Panel.SetActive(true);
        roomPanel.SetActive(false);
    }

    public void StartClientGame() {
        NetworkManager.singleton.StartClient();
    }
    
}
