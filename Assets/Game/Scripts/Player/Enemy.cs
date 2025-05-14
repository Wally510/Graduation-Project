using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using DG.Tweening;

public class Enemy : NetworkBehaviour
{
    private CharacterController _cc;
    [SyncVar]
    public bool canMove = true; //是否可以移动
    public float MoveSpeed = 5f;     //移动速度
    private Vector3 _movementVelocity;
    private Playerinput _playerinput;
    private float _verticalVelocity;
    public float Gravity = -9.8f;     //重力
    public GameObject enemyModel;    //玩家模型
    public GameObject Attack;
    private Animator _animator;     //动画
    private Camera theCam;

    private void Awake() {
        //获取摄像机
        theCam = Camera.main;

        _cc = GetComponent<CharacterController>();
        _animator = transform.GetChild(0).GetComponent<Animator>();
        _playerinput = GetComponent<Playerinput>();
    }

    private void FixedUpdate() 
    {
        if(!isLocalPlayer) return;

        if(Input.GetMouseButtonDown(0) && canMove == true){
            canMove = false;
            _animator.SetTrigger("Attack");
            CmdTrigger("Attack");
            _animator.SetFloat("Speed", 0f);
            CmdFloat("Speed", 0f);
            Attack.SetActive(true);
            StartCoroutine(AttackFinish());
        }

        if(!canMove) return;

        CalculatePlayerMovement();

        //从高向低处移动时垂直移动(靠重力下降)
        if(_cc.isGrounded == false){
            _verticalVelocity = Gravity;
        }else{
            _verticalVelocity = Gravity * 0.3f;
        }
        //_verticalVelocity = Gravity * 0.3f;
        _movementVelocity += _verticalVelocity * Vector3.up *Time.deltaTime;

        //实现移动
        _cc.Move(_movementVelocity);
        //_movementVelocity = Vector3.zero;

        if(_playerinput.HorizontalInput != 0 || _playerinput.VerticalInput != 0){
            //转向摄像机方向
            transform.rotation = Quaternion.Euler(0f, theCam.transform.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(_movementVelocity.x, 0f, _movementVelocity.z));
            enemyModel.transform.rotation = newRotation;
        }
    }

    private void CalculatePlayerMovement(){

        //按键移动

        //_movementVelocity.Set(_playerinput.HorizontalInput, 0f, _playerinput.VerticalInput);
        _movementVelocity = (transform.forward * _playerinput.VerticalInput) + (transform.right * _playerinput.HorizontalInput);
        _movementVelocity.Normalize();
        //_movementVelocity = Quaternion.Euler(0, -45f, 0) * _movementVelocity;

        //移动停止动画(walk)
        _animator.SetFloat("Speed", _movementVelocity.magnitude);
        CmdFloat("Speed", _movementVelocity.magnitude);
        
        _movementVelocity *= MoveSpeed * Time.deltaTime;

        /* if(_playerinput.SpaceKeyDown)
        {
            _movementVelocity.y = JumpForce;
        } */
        
        _animator.SetBool("Jump", !_cc.isGrounded);
        CmdBool("Jump", !_cc.isGrounded);
    }

    IEnumerator AttackFinish()
    {
        yield return new WaitForSeconds(2f);
        
        Attack.SetActive(false);
        canMove = true;
    }

    [Command(requiresAuthority=false)]
    void CmdTrigger(string t)
    {
        if (!isLocalPlayer)
            transform.GetChild(0).GetComponent<Animator>().SetTrigger(t);
        RpcTrigger(t);
    }

    [ClientRpc]
    void RpcTrigger(string t)
    {
        if (!isLocalPlayer)
            transform.GetChild(0).GetComponent<Animator>().SetTrigger(t);
    }

    [Command(requiresAuthority=false)]
    void CmdFloat(string t, float n)
    {
        if (!isLocalPlayer)
            transform.GetChild(0).GetComponent<Animator>().SetFloat(t, n);
        RpcFloat(t, n);
    }

    [ClientRpc]
    void RpcFloat(string t, float n)
    {
        if (!isLocalPlayer)
            transform.GetChild(0).GetComponent<Animator>().SetFloat(t, n); 
    }

    [Command(requiresAuthority=false)]
    void CmdBool(string t, bool n)
    {
        if (!isLocalPlayer)
            transform.GetChild(0).GetComponent<Animator>().SetBool(t, n);
        RpcBool(t, n);
    }

    [ClientRpc]
    void RpcBool(string t, bool n)
    {
        if (!isLocalPlayer)
            transform.GetChild(0).GetComponent<Animator>().SetBool(t, n); 
    }
}
