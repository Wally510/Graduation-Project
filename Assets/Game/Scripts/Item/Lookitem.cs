using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lookitem : MonoBehaviour
{
    public int pagei;
    public int geti;
    public int mi1;
    public int mi2;
    public int mi3;
    public int mj1;
    public int mj2;
    public int mj3;

    public Workbenches work;
    public ButtonMake make;

    public void onGetclick(){
        work.Getclick(geti, mi1, mi2, mi3, mj1, mj2, mj3);
        make.pagei = pagei;
    }

    public void onMakeclick() {
        work.Makeclick(geti, mi1, mi2, mi3, mj1, mj2, mj3);
        work.Getclick(geti, mi1, mi2, mi3, mj1, mj2, mj3);
    }
}
