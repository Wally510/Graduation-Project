using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurt : MonoBehaviour
{
    private Enemy enemy;
    public ParticleSystem stopTrail;
    public float stopTime = 5f; // 停止时间
    
    private void Awake() {
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)){
            Stop();
        }
    }
    public void Stop(){
        //enemy.canMove = false;
        transform.GetChild(0).GetComponent<Animator>().SetFloat("Speed", 0f);
        stopTrail.Play();

        StartCoroutine(StopFinish());
    }

    IEnumerator StopFinish(){
        yield return new WaitForSeconds(stopTime);

        //enemy.canMove = true;
        stopTrail.Stop();
    }
}
