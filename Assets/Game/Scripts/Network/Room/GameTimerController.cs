using Mirror;
using UnityEngine;

public class GameTimerController : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnTimerChanged))]
    public int remainingTime = 1200; // 20 分钟 = 1200 秒

    public static GameTimerController instance;

    private void Awake()
    {
        instance = this;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        InvokeRepeating(nameof(ServerCountdown), 1f, 1f);
    }

    [Server]
    void ServerCountdown()
    {
        if (remainingTime <= 0)
        {
            CancelInvoke(nameof(ServerCountdown));
            RpcEndGame(); // 通知所有客户端结束
            return;
        }

        remainingTime--;
    }

    void OnTimerChanged(int oldTime, int newTime)
    {
        // 可以更新 UI，比如调用 UIManager.Instance.UpdateTimer(newTime);
    }

    [ClientRpc]
    void RpcEndGame()
    {
        Debug.Log("游戏时间结束！");
        // 在这里执行结束逻辑，如跳转到结算界面
        if (isServer)
            NetworkManager.singleton.StopHost(); // 服务器断开
        else
            NetworkManager.singleton.StopClient(); // 客户端断开
    }
}
