using Mirror;
using UnityEngine;

public class CharacterSelectUI : MonoBehaviour
{
    public GameObject CharaUI;

    string selectedCharacterType;

    public void ChooseEnemy()
    {
        selectedCharacterType = "Enemy";
        StartConnection();
        CharaUI.transform.GetChild(3).gameObject.SetActive(true);
        CharaUI.transform.GetChild(5).gameObject.SetActive(false);
    }

    public void ChoosePlayer()
    {
        selectedCharacterType = "Player";
        StartConnection();
        CharaUI.transform.GetChild(0).gameObject.SetActive(true);
        CharaUI.transform.GetChild(2).gameObject.SetActive(true);
        CharaUI.transform.GetChild(5).gameObject.SetActive(false);
    }

    void StartConnection()
    {
        NetworkManager.singleton.StartClient();
        NetworkClient.OnConnectedEvent += OnConnected;
    }

    void OnConnected()
    {
        Debug.Log("Connected to server");

        // Step 1: 发送角色选择信息
        var msg = new SelectCharacterMessage { characterType = selectedCharacterType };
        NetworkClient.Send(msg);

        // Step 2: 显式请求添加玩家（这个会触发 OnServerAddPlayer）
        NetworkClient.Send(new AddPlayerMessage());
    }
}
