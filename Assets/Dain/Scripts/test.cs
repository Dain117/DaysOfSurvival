using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class test :MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Awake()
    {
       
        PhotonNetwork.ConnectUsingSettings();
    }


    public override void OnConnectedToMaster() => PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 6 }, null);

    public override void OnJoinedRoom()
    {
        Debug.Log("Á¢¼Ó");
    }

}
