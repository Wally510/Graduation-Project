using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;

public class Playerinput : NetworkBehaviour
{

    public float HorizontalInput;
    public float VerticalInput;
    public bool ShiftKeyDown;
    public bool SpaceKeyDown;

    public override void OnStartLocalPlayer()
    {
        CinemachineFreeLook vcam = FindObjectOfType<CinemachineFreeLook>();
        if (vcam != null)
        {
            vcam.Follow = transform.GetChild(1);
            vcam.LookAt = transform.GetChild(1);
        }
        vcam.transform.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    void HandleMovement(){
        if(isLocalPlayer){
            SpaceKeyDown = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0);
            ShiftKeyDown = Input.GetKey(KeyCode.LeftShift);
            HorizontalInput = Input.GetAxisRaw("Horizontal");
            VerticalInput = Input.GetAxisRaw("Vertical");
        }
    }

    private void OnDisable() {
          ClearCache();
    }

    public void ClearCache(){
        SpaceKeyDown = false;
        ShiftKeyDown = false;
        HorizontalInput = 0;
        VerticalInput = 0;  
    }
}
