using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change : MonoBehaviour
{
    bool isFullScreen = true;
    void Update()
    {
        //  按ESC退出全屏
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isFullScreen){
                Screen.SetResolution(640, 360, false);       
                isFullScreen = false;
            }else{
                Screen.SetResolution(1920, 1080, true);       
                isFullScreen = true;
            }
            
        }
    }
}
