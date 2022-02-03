using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LaunchManager : MonoBehaviourPunCallbacks
{
    public GameObject enterGamePanel;
    public GameObject connectionStatusPanel;
    public GameObject lobbyPanel;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        enterGamePanel.SetActive(true);
        connectionStatusPanel.SetActive(false);
        lobbyPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Checks if player is connected to photon servers
    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.NickName + " connected to photon servers");
        connectionStatusPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    // Checks if player is connected to the internet
    public override void OnConnected()
    {
        Debug.Log("Connected to internet");
    }

    // Called if no match has been found
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning(message);
        CreateAndJoinRoom();
    }
    public void ConnectToPhotonServer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            connectionStatusPanel.SetActive(true);
            enterGamePanel.SetActive(false);
        }
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    // Create random room
    private void CreateAndJoinRoom()
    {
        string randomRoomName = "Room " + Random.Range(0, 10000);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 20;

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }

    // Checks if we joined a room
    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " has entered " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("GameScene");
    }

    // Checks if another player has entered the room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " has entered room " + PhotonNetwork.CurrentRoom.Name + ". Room has now " + PhotonNetwork.CurrentRoom.PlayerCount + " players.");
    }
}
