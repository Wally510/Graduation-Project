using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Cinemachine;
using System;
using Mirror;
using HighlightPlus;

public class EnemySkill : NetworkBehaviour
{
    private Enemy input;
    public float skill1cooldown = 10f; // 技能冷却时间
    public float skill1time;
    public Transform skill1Prefab; // 技能预制体
    public float teleportDistance = 5f; // 瞬移的距离
    public float warpDuration = 0.5f; // 瞬移的持续时间

    public float skill2cooldown = 6f; // 技能冷却时间
    public float skill2time;
    public Transform skill2Prefab; // 技能预制体

    public float skill3cooldown = 15f; // 技能冷却时间
    public float skill3time;
    public Transform skill3Prefab; // 技能预制体

    [Space]
    public Material glowMaterial;
    public Material enemyMaterial;

    [Header("Particles")]
    public ParticleSystem blueTrail;
    public ParticleSystem whiteTrail;

    public override void OnStartLocalPlayer()
    {
        GameObject.FindWithTag("GameCanvas").transform.GetChild(3).gameObject.SetActive(true);
        skill1Prefab = GameObject.FindWithTag("EnemySkill").transform.GetChild(0);
        skill2Prefab = GameObject.FindWithTag("EnemySkill").transform.GetChild(1);
        skill3Prefab = GameObject.FindWithTag("EnemySkill").transform.GetChild(2);
}
    
    private void Awake() {
        input = GetComponent<Enemy>();
        skill1time = skill1cooldown;
        skill2time = skill2cooldown;
        skill3time = skill3cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isLocalPlayer) return;

        if(skill1time > 0f){
            skill1time -= Time.deltaTime;
            skill1Prefab.GetChild(0).GetComponent<Image>().fillAmount = skill1time / skill1cooldown;
            skill1Prefab.GetChild(1).GetComponent<Text>().text = ((int)skill1time).ToString() + " s";
        }
        if(skill1time <= 0f){
            skill1Prefab.GetChild(0).GetComponent<Image>().fillAmount = 0f;
            skill1Prefab.GetChild(1).gameObject.SetActive(false);
        }

        if(skill2time > 0f){
            skill2time -= Time.deltaTime;
            skill2Prefab.GetChild(0).GetComponent<Image>().fillAmount = skill2time / skill2cooldown;
            skill2Prefab.GetChild(1).GetComponent<Text>().text = ((int)skill2time).ToString() + " s";
        }
        if(skill2time <= 0f){
            skill2Prefab.GetChild(0).GetComponent<Image>().fillAmount = 0f;
            skill2Prefab.GetChild(1).gameObject.SetActive(false);
        }

        if(skill3time > 0f){
            skill3time -= Time.deltaTime;
            skill3Prefab.GetChild(0).GetComponent<Image>().fillAmount = skill3time / skill3cooldown;
            skill3Prefab.GetChild(1).GetComponent<Text>().text = ((int)skill3time).ToString() + " s";
        }
        if(skill3time <= 0f){
            skill3Prefab.GetChild(0).GetComponent<Image>().fillAmount = 0f;
            skill3Prefab.GetChild(1).gameObject.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.Alpha1) && skill1time <= 0f){
            Vector3 targetPosition = transform.position + transform.forward * teleportDistance;
            input.canMove = false;
            Warp(targetPosition);
            skill1time = skill1cooldown;
            skill1Prefab.GetChild(0).GetComponent<Image>().fillAmount = 1f;
            skill1Prefab.GetChild(1).gameObject.SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2) && skill2time <= 0f){
            GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
            for(int i = 0; i < Players.Length; i++){
                Players[i].GetComponent<HighlightEffect>().highlighted = true;
            }
            transform.GetComponent<HighlightEffect>().highlighted = true;
            SkinnedMeshRenderer[] skinMeshList = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer smr in skinMeshList)
            {
                smr.material = glowMaterial;
            }

            StartCoroutine(Skill2Finish());
        }

        if(Input.GetKeyDown(KeyCode.Alpha3) && skill3time <= 0f){
            GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
            for(int i = 0; i < Players.Length; i++){
                Players[i].GetComponent<Character>().isDark = true;
                Players[i].GetComponent<Character>().MoveSpeed = 1f;
            }

            StartCoroutine(Skill3Finish());

            skill3time = skill3cooldown;
            skill3Prefab.GetChild(0).GetComponent<Image>().fillAmount = 1f;
            skill3Prefab.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void Warp(Vector3 vector3) {
        //CmdSpawnClone();
        GameObject clone = Instantiate(GetComponent<Enemy>().enemyModel, transform.position, transform.rotation);
        Destroy(clone.GetComponent<EnemyDestroy>().sword.gameObject);
        Destroy(clone.GetComponent<EnemyDestroy>().mask.gameObject);
        Destroy(clone.GetComponent<Animator>());

        SkinnedMeshRenderer[] skinMeshList = clone.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in skinMeshList)
        {
            smr.material = glowMaterial;
            smr.material.DOFloat(2, "_AlphaThreshold", 5f).OnComplete(()=>Destroy(clone));
        }

        ShowBody(false);
        
        transform.DOMove(vector3, warpDuration).SetEase(Ease.InExpo).OnComplete(()=>FinishWarp());

        //Particles
        blueTrail.Play();
        whiteTrail.Play();
    }

    [Command(requiresAuthority = false)] // 允许非玩家对象调用
    public void CmdSpawnClone()
    {
        string prefabName = "Clone";
        // 在服务器端生成物体
        GameObject prefab = NetworkManager.singleton.spawnPrefabs.Find(
        p => p.name == prefabName
        );
        
        if (prefab != null)
        {
            GameObject clone = Instantiate(prefab, transform.position, transform.rotation);
            Destroy(clone.GetComponent<EnemyDestroy>().sword.gameObject);
            Destroy(clone.GetComponent<EnemyDestroy>().mask.gameObject);
            Destroy(clone.GetComponent<Animator>());

            SkinnedMeshRenderer[] skinMeshList = clone.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer smr in skinMeshList)
            {
                smr.material = glowMaterial;
                smr.material.DOFloat(2, "_AlphaThreshold", 5f).OnComplete(()=>Destroy(clone));
            }
            
            // 同步到所有客户端
            NetworkServer.Spawn(clone);
        }  
    }

    void FinishWarp(){
        ShowBody(true);

        StartCoroutine(HideSword());
        StartCoroutine(StopParticles());
    }

    IEnumerator StopParticles()
    {
        yield return new WaitForSeconds(.2f);
        blueTrail.Stop();
        whiteTrail.Stop();
    }

    IEnumerator HideSword()
    {
        yield return new WaitForSeconds(.4f);

        input.canMove = true;
    }

    void ShowBody(bool state){
        SkinnedMeshRenderer[] skinMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in skinMeshList)
        {
            smr.enabled = state;
        }
    }

    IEnumerator Skill2Finish()
    {
        yield return new WaitForSeconds(2f);
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(Players.Length);
        for(int i = 0; i < Players.Length; i++){
            Players[i].GetComponent<HighlightEffect>().highlighted = false;
        }
        transform.GetComponent<HighlightEffect>().highlighted = false;
        SkinnedMeshRenderer[] skinMeshList = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in skinMeshList)
        {
            smr.material = enemyMaterial;
        }

        skill2time = skill2cooldown;
        skill2Prefab.GetChild(0).GetComponent<Image>().fillAmount = 1f;
        skill2Prefab.GetChild(1).gameObject.SetActive(true);
    }

    IEnumerator Skill3Finish()
    {
        yield return new WaitForSeconds(3f);
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < Players.Length; i++){
            Players[i].GetComponent<Character>().isDark = false;
        }
    }
}
