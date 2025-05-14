using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : MonoBehaviour
{
    private Character player;

    private void Awake() {
        player = GetComponent<Character>();
    }

    public void TakeDamage()
    {
        player.playerHealth -= 1;
    }
}
