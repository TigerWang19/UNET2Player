using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager
{
    public int chosenCharacter = 0;
    public Vector3 spawnPosition;
    public Quaternion spawnRotation;

    Vector3 Position1 = new Vector3(2.3f, 0.05f, -4.0f);
    Vector3 Position2 = new Vector3(-3.0f, 0.05f, -4.0f);

    Quaternion Rotation1 = Quaternion.Euler(0f, -20f, 0f);
    Quaternion Rotation2 = Quaternion.Euler(0f, 200f, 0f);

    //hook , invoked when a host is started
    public override void OnStartHost()
    {
        spawnPosition = Position1;
        spawnRotation = Rotation1;
    }

    public void Player1Button()
    {
        chosenCharacter = 0;
    }

    public void Player2Button()
    {
        chosenCharacter = 1;
    }

    //子类发送网络消息
    public class NetworkMessage : MessageBase
    {
        public int chosenClass;
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        NetworkMessage message = extraMessageReader.ReadMessage<NetworkMessage>();
        int selectedClass = message.chosenClass;

        if (selectedClass == 0)
        {
            GameObject player = Instantiate(Resources.Load("Player1"), spawnPosition, spawnRotation) as GameObject;
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
        if (selectedClass == 1)
        {
            GameObject player = Instantiate(Resources.Load("Player2"), spawnPosition, spawnRotation) as GameObject;
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }

        spawnPosition = Position2;
        spawnRotation = Rotation2;

    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        NetworkMessage test = new NetworkMessage();
        test.chosenClass = chosenCharacter;

        ClientScene.AddPlayer(client.connection, 0, test);
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        //base.OnClientSceneChanged(conn);
    }

}
