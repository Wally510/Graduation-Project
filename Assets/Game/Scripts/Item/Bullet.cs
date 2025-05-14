using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    [SerializeField, Tooltip("最大转弯速度")]
    private float MaximumRotationSpeed = 120.0f;
 
    [SerializeField, Tooltip("加速度")]
    private float AcceleratedVeocity = 12.8f;
 
    [SerializeField, Tooltip("最高速度")]
    private float MaximumVelocity = 30.0f;

    public Transform Target; // 目标
    public float CurrentVelocity = 0.0f;   // 当前速度
    public ParticleSystem Trail;


    // Start is called before the first frame update
    void Awake()
    {
        Target = GameObject.FindWithTag("Enemy").transform;
        Trail.Play();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;

        Vector3 offset = (Target.position + new Vector3(0,1,0) - transform.position).normalized;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation( Target.position - transform.position), MaximumRotationSpeed * Time.deltaTime);
 
        // 计算当前方向与目标方向的角度差
        float angle = Vector3.Angle(transform.forward, offset);
        
        // 根据最大旋转速度，计算转向目标共计需要的时间
        float needTime = angle / ( MaximumRotationSpeed * ( CurrentVelocity / MaximumVelocity ));
 
        // 如果角度很小，就直接对准目标
        if (needTime < 0.001f)
        {
            transform.forward = offset;
        }
        else
        {
            // 当前帧间隔时间除以需要的时间，获取本次应该旋转的比例。
            transform.forward = Vector3.Slerp(transform.forward, offset, deltaTime / needTime).normalized;
        }
 
        // 如果当前速度小于最高速度，则进行加速
        if (CurrentVelocity < MaximumVelocity )
            CurrentVelocity += deltaTime * AcceleratedVeocity;
        
        // 朝自己的前方位移
        transform.position += transform.forward * CurrentVelocity * deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHurt>().Stop();
            //CmdHitEnemy(other.GetComponent<NetworkIdentity>());
            Destroy(gameObject, 0.5f);
        }
    }

    /* [Command]
    public void CmdHitEnemy(NetworkIdentity victimId)
    {
        EnemyHurt target = victimId.GetComponent<EnemyHurt>();
        if (target != null)
        {
            target.Stop(); // ✅ 服务端逻辑修改血量
        }
    } */

}
