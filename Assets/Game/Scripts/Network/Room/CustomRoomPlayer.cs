using System.Collections;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomRoomPlayer : NetworkRoomPlayer
{
    [SyncVar(hook = nameof(OnCharacterTypeChanged))]
    public string selectedCharacterType = "Player"; // "Player" or "Enemy"

    [SyncVar(hook = nameof(OnCharacterIntChanged))]
    public int playerIndex; // 玩家索引

    [Header("RoomPlayer 生成点(Lobby Scene 中)")]
    public Vector3[] roomSpawnPoints;
    public Vector3[] roomSpawnRotations;


    public override void OnStartClient()
    {
        base.OnStartClient();
        transform.GetChild(2).GetComponent<TextMeshPro>().text = "Player " + netId;

        int index = (int)(netId - 1); // netId 从 1 开始
        playerIndex = index;

        Debug.Log($"玩家索引: {netId}");

        if (roomSpawnPoints != null && index < roomSpawnPoints.Length)
        {
            transform.position = roomSpawnPoints[index];
            transform.rotation = Quaternion.Euler(roomSpawnRotations[index]);
            Debug.Log($"玩家索引: {index}");
            if(index == 0)
            {
                selectedCharacterType = "Enemy";
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                selectedCharacterType = "Player";
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
    {
        base.ReadyStateChanged(oldReadyState, newReadyState);
        transform.GetChild(3).GetComponent<TextMeshPro>().text = readyToBegin ? "Ready" : "Not Ready";
        if(readyToBegin){
            transform.GetChild(3).GetComponent<TextMeshPro>().color = Color.green;
        }
        else{
            transform.GetChild(3).GetComponent<TextMeshPro>().color = Color.red;
        }
    }

    void OnCharacterTypeChanged(string oldType, string newType)
    {
        if(isServer && isClient) return;
        if(selectedCharacterType == "Enemy")
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
        else if (selectedCharacterType == "Player")
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    void OnCharacterIntChanged(int oldIndex, int newIndex)
    {
        transform.transform.position = roomSpawnPoints[newIndex];
        transform.transform.rotation = Quaternion.Euler(roomSpawnRotations[newIndex]);
    }


    [Command]
    public void CmdSetCharacterType(string characterType, int i)
    {
        selectedCharacterType = characterType;
        playerIndex = i;
        if(characterType == "Enemy")
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
        else if (characterType == "Player")
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    // 不 override CmdChangeReadyState（这是父类已实现的）
    public void SetReady(bool ready)
    {
        CmdChangeReadyState(ready); // 调用父类方法
    }


    // （删除 OnClientReady，NetworkRoomPlayer 不支持 override 它）
}
