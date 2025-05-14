using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Cinemachine;
using System;
using Mirror;
using HighlightPlus;

public class PlayerSkill : NetworkBehaviour
{
    public void Skill1click () {
        
    }
    public void Skill2click () {
        GameObject enemy = GameObject.FindWithTag("Enemy");
        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        if(distance < 10f) {
            Debug.Log("生成bullet");
            CmdSpawnItem("Bullet", transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        }
    }
    public void Skill3click () {
        CmdSpawnItem("Wall", transform.position, Quaternion.identity);
    }

    [Command(requiresAuthority = false)] // 允许非玩家对象调用
    public void CmdSpawnItem(string prefabName, Vector3 position, Quaternion rotation)
    {
        // 在服务器端生成物体
        GameObject prefab = NetworkManager.singleton.spawnPrefabs.Find(
        p => p.name == prefabName
        );
        
        if (prefab != null)
        {
            GameObject newItem = Instantiate(prefab, position, rotation);
            NetworkServer.Spawn(newItem);
        }
    }
}
