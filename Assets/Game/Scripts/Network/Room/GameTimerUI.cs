using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimerUI : MonoBehaviour
{
    void Update()
    {
        if (GameTimerController.instance == null) return;

        int time = GameTimerController.instance.remainingTime;
        int minutes = time / 60;
        int seconds = time % 60;
        transform.GetComponent<Text>().text = $"{minutes:D2}:{seconds:D2}";
    }
}
