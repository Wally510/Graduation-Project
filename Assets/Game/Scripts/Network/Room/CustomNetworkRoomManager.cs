using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomNetworkRoomManager : NetworkRoomManager
{
    [Header("角色预制体")]
    public GameObject EnemyPrefab;
    public GameObject PlayerPrefab;
    public GameObject countdownControllerPrefab; 
    public GameObject gameTimerPrefab;

    private Coroutine countdownCoroutine;

    public override void OnRoomServerSceneChanged(string sceneName)
    {
        base.OnRoomServerSceneChanged(sceneName);

        if (sceneName == GameplayScene)
        {
            GameObject timerObj = Instantiate(gameTimerPrefab);
            NetworkServer.Spawn(timerObj);
        }
    }

    public override void OnRoomServerPlayersReady()
    {
        bool haveEnemy = false;
        foreach (var roomPlayer in FindObjectsOfType<CustomRoomPlayer>())
        {
            if(roomPlayer.selectedCharacterType == "Enemy")
            {
                haveEnemy = true;
            }
        }
        if (countdownCoroutine == null && haveEnemy)
        {
            // 确保 CountdownController 存在
            if (CountdownController.instance == null)
            {
                GameObject controller = Instantiate(countdownControllerPrefab);
                NetworkServer.Spawn(controller); // 必须 spawn 才能同步
            }

            countdownCoroutine = StartCoroutine(StartCountdown());
        }
    }

    private IEnumerator StartCountdown()
    {
        CountdownController ctrl = CountdownController.instance;

        ctrl.countdownTime = 10;
        while (ctrl.countdownTime > 0)
        {
            yield return new WaitForSeconds(1f);
            ctrl.countdownTime--;

            if (!allPlayersReady)
            {
                StopCountdown();
                yield break;
            }
        }

        ServerChangeScene(GameplayScene);
    }

    private void StopCountdown()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }

        if (CountdownController.instance != null)
            CountdownController.instance.countdownTime = -1;
    }

    // 当所有人都准备好，进入 GameScene 之前调用
    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        var roomComp = roomPlayer.GetComponent<CustomRoomPlayer>();
        string type = roomComp.selectedCharacterType;

        GameObject prefabToSpawn = (type == "Enemy") ? EnemyPrefab : PlayerPrefab;
        GameObject gamePlayer = Instantiate(prefabToSpawn);

        return gamePlayer;
    }

    // （可选）在切换场景后，如果需要对刚生成的 gamePlayer 做额外初始化，可重写此方法
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
        // e.g. 将角色名显示在头顶，等等
        return true; // Return true to indicate successful handling
    }
}
