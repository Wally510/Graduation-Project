using Mirror;
using UnityEngine;

public class CountdownController : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnCountdownChanged))]
    public int countdownTime = -1;

    public static CountdownController instance;

    void Awake()
    {
        instance = this;
    }

    private void OnCountdownChanged(int oldVal, int newVal)
    {
        Debug.Log($"倒计时同步更新：{newVal}");
    }
}
