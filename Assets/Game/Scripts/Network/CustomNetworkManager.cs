using Mirror;
using UnityEngine;
using System.Collections.Generic;

public class CustomNetworkManager : NetworkManager
{
    public GameObject EnemyPrefab;
    public GameObject PlayerPrefab;

    private Dictionary<int, string> connectionToCharacter = new Dictionary<int, string>();

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<SelectCharacterMessage>(OnSelectCharacter);
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        string characterType = connectionToCharacter.ContainsKey(conn.connectionId)
            ? connectionToCharacter[conn.connectionId]
            : "Warrior"; // default

        GameObject prefab = characterType == "Mage" ? PlayerPrefab : EnemyPrefab;

        GameObject player = Instantiate(prefab);
        NetworkServer.AddPlayerForConnection(conn, player);
    }

    void OnSelectCharacter(NetworkConnectionToClient conn, SelectCharacterMessage msg)
    {
        connectionToCharacter[conn.connectionId] = msg.characterType;
    }
}
