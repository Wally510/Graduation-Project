using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class Character : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnCharacterHealthChanged))]
    public int playerHealth = 2;
    private CharacterController _cc;
    public bool canMove = true; //是否可以移动
    public float MoveSpeed = 5f;     //移动速度
    public float JumpForce = 0.2f;     //跳跃高度
    public float Physicalmax = 4f;     //体力条值
    public GameObject Physicalgame;    //体力条图片
    public GameObject Blood;
    public float Physical;
    private Vector3 _movementVelocity;
    private Playerinput _playerinput;
    private float _verticalVelocity;
    public float Gravity = -9.8f;     //重力
    public GameObject playerModel;    //玩家模型
    private Animator _animator;     //动画
    private Camera theCam;

    public bool isDark = false; //是否处于黑暗状态

    public override void OnStartLocalPlayer()
    {
        GameObject.FindWithTag("GameCanvas").transform.GetChild(0).gameObject.SetActive(true);
        GameObject.FindWithTag("GameCanvas").transform.GetChild(2).gameObject.SetActive(true);
        Physicalgame = GameObject.FindWithTag("Physical");
        Blood = GameObject.FindWithTag("GameCanvas").transform.GetChild(6).gameObject;
    }

    private void Awake() {
        //获取摄像机
        theCam = Camera.main;

        _cc = GetComponent<CharacterController>();
        _animator = transform.GetChild(0).GetComponent<Animator>();
        _playerinput = GetComponent<Playerinput>();
        //Physicalgame = GameObject.FindWithTag("Physical");

        Physical = Physicalmax;
    }

    private void FixedUpdate() 
    {
        if(!isLocalPlayer) return;

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

        //体力条图片显示
        //Physicalgame.GetComponent<RectTransform>().anchoredPosition = GetComponent<UIFollow>().GetScreenPosition(transform.position);
        Physicalgame.GetComponent<Image>().fillAmount = Physical / Physicalmax;

        if(_playerinput.HorizontalInput != 0 || _playerinput.VerticalInput != 0){
            //转向摄像机方向
            transform.rotation = Quaternion.Euler(0f, theCam.transform.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(_movementVelocity.x, 0f, _movementVelocity.z));
            playerModel.transform.rotation = newRotation;
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
        

        //跑步体力条下降or恢复
        if(_playerinput.ShiftKeyDown && _movementVelocity.magnitude > 0.1f){
            Physical -= Time.deltaTime;
        }else{
            Physical += Time.deltaTime;
        }

        //重置体力条，保证体力条范围
        if(Physical <= 0){
            Physical = 0;
        }else if(Physical >= Physicalmax){
            Physical = Physicalmax;
        }
        
        if(!isDark){
            if(Physical > 0){
                //按Shift跑步
                _animator.SetBool("Isrun", _playerinput.ShiftKeyDown);
                CmdBool("Isrun", _playerinput.ShiftKeyDown);

                //更改跑步速度
                MoveSpeed = (_playerinput.ShiftKeyDown) ? 5f : 2f;
            }else{
                _animator.SetBool("Isrun", false);
                CmdBool("Isrun", false);
                MoveSpeed = 2f;
            }
        }
        
        _movementVelocity *= MoveSpeed * Time.deltaTime;

        /* if(_playerinput.SpaceKeyDown)
        {
            _movementVelocity.y = JumpForce;
        } */
        
        _animator.SetBool("Jump", !_cc.isGrounded);
        CmdBool("Jump", !_cc.isGrounded);
    }

    void OnCharacterHealthChanged(int oldValue, int newValue)
    {
        if(newValue == 1)
        {
            Physicalmax = 1.5f;
            if(isLocalPlayer){
                Blood.SetActive(true);
            }
        }else if(newValue == 0)
        {
            canMove = false;
        }else if(newValue <= 0){
            playerHealth = 0;
        }
    }

    public void TakeDamage()
    {
        playerHealth -= 1;
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
