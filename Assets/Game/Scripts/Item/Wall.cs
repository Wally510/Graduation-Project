using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Wall : NetworkBehaviour
{
    public ParticleSystem Trail;
    void Awake()
    {
        Trail.Play();
        //StartCoroutine(DestroyAfterSeconds(10f));
    }

    /* [Server] // 确保只在服务器执行
    private System.Collections.IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        
        // 同步销毁物体
        NetworkServer.Destroy(gameObject);
    } */
}
