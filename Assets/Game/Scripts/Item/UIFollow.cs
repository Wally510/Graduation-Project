using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Diagnostics;
using static UnityEngine.GraphicsBuffer;

public class UIFollow : MonoBehaviour
{
    public Vector3 offsetv3;
    public Vector2 offset;

    public Vector2 GetScreenPosition(Vector3 v3){
        Vector3 viewportPos = new Vector3();
        v3 = v3 + offsetv3;
        viewportPos = Camera.main.WorldToViewportPoint(v3);
        Transform ca = GameObject.Find("Game Canvas").transform;
        RectTransform canvasRtm = ca.GetComponent<RectTransform>();
        Vector2 uguiPos = Vector2.zero;
        uguiPos.x = (viewportPos.x - offset.x) * canvasRtm.sizeDelta.x;
        uguiPos.y = (viewportPos.y - offset.y) * canvasRtm.sizeDelta.y;
        return uguiPos;
    }
}
