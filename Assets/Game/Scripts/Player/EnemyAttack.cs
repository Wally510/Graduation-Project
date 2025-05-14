using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class EnemyAttack : NetworkBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player in range");
            CmdHitPlayer(other.GetComponent<NetworkIdentity>());
        }
    }

    [Command]
    public void CmdHitPlayer(NetworkIdentity victimId)
    {
        Character target = victimId.GetComponent<Character>();
        if (target != null)
        {
            target.TakeDamage(); // ✅ 服务端逻辑修改血量
        }
    }
}
