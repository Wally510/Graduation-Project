using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI2 : MonoBehaviour
{
    public Text countdownText;
    public GameObject CharaUI;

    private CustomRoomPlayer localRoomPlayer;

    void Start()
    {
        // 延迟一点时间确保房间玩家对象生成完成
        Invoke(nameof(FindLocalRoomPlayer), 0.5f);
    }

    void Update()
    {
        if (CountdownController.instance == null) return;

        int countdown = CountdownController.instance.countdownTime;

        if (countdown >= 0)
        {
            countdownText.text = $"游戏将在 {countdown} 秒后开始...";
            countdownText.gameObject.SetActive(true);
        }
        else
        {
            countdownText.gameObject.SetActive(false);
        }
    }

    void FindLocalRoomPlayer()
    {
        foreach (var roomPlayer in FindObjectsOfType<CustomRoomPlayer>())
        {
            if (roomPlayer.isLocalPlayer)
            {
                localRoomPlayer = roomPlayer;
                break;
            }
        }
        if(localRoomPlayer == null) return;
        localRoomPlayer.transform.GetChild(2).GetComponent<TextMeshPro>().text = "Player " + localRoomPlayer.netId;
        Debug.Log($"玩家: {localRoomPlayer.netId}, 角色: {localRoomPlayer.selectedCharacterType}, 是否准备: {localRoomPlayer.readyToBegin}");
    }

    public void ChooseEnemy()
    {
        if (localRoomPlayer == null) return;
        bool enemyFull = false;
        foreach (var roomPlayer in FindObjectsOfType<CustomRoomPlayer>())
        {
            if (!roomPlayer.isLocalPlayer)
            {
                if(roomPlayer.selectedCharacterType == "Enemy")
                {
                    Debug.Log("敌人已满，无法选择敌人角色");
                    enemyFull = true;
                    return;
                }
            }
        }
        if(enemyFull) return;
        localRoomPlayer.transform.position = localRoomPlayer.roomSpawnPoints[0];
        localRoomPlayer.transform.rotation = Quaternion.Euler(localRoomPlayer.roomSpawnRotations[0]);
        localRoomPlayer.playerIndex = 0;
        localRoomPlayer.CmdSetCharacterType("Enemy", 0);
    }

    public void ChoosePlayer()
    {
        if (localRoomPlayer == null) return;
        int[] index = new int[] { 0, 0, 0, 0, 0, 0 };
        foreach (var roomPlayer in FindObjectsOfType<CustomRoomPlayer>())
        {
            if (!roomPlayer.isLocalPlayer)
            {
                if(roomPlayer.selectedCharacterType == "Player")
                {
                    index[roomPlayer.playerIndex] = 1;
                }
            }
        }
        int cindex = 1;
        for (int i = 1; i < index.Length; i++)
        {
            if (index[i] == 0)
            {
                localRoomPlayer.transform.position = localRoomPlayer.roomSpawnPoints[i];
                localRoomPlayer.transform.rotation = Quaternion.Euler(localRoomPlayer.roomSpawnRotations[i]);
                localRoomPlayer.playerIndex = i;
                cindex = i;
                break;
            }
        }
        localRoomPlayer.CmdSetCharacterType("Player", cindex);
        //UpdateUIForChoice("Player");
    }

    public void ToggleReady(bool isReady)
    {
        if (localRoomPlayer == null) return;
        //Debug.Log($"玩家: {localRoomPlayer.netId}, 角色: {localRoomPlayer.selectedCharacterType}, 是否准备: {localRoomPlayer.readyToBegin}");
        CharaUI.transform.GetChild(0).GetChild(0).gameObject.SetActive(!isReady);
        CharaUI.transform.GetChild(0).GetChild(1).gameObject.SetActive(isReady);
        localRoomPlayer.transform.GetChild(3).GetComponent<TextMeshPro>().text = isReady ? "Ready" : "Not Ready";
        if(isReady){
            localRoomPlayer.transform.GetChild(3).GetComponent<TextMeshPro>().color = Color.green;
        }
        else{
            localRoomPlayer.transform.GetChild(3).GetComponent<TextMeshPro>().color = Color.red;
        }
        localRoomPlayer.SetReady(isReady);
    }
}
