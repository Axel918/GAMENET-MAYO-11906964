using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")]
    public GameObject LoginUIPanel;
    public TMP_InputField PlayerNameInput;

    [Header("Connecting Info Panel")]
    public GameObject ConnectingInfoUIPanel;

    [Header("GameOptions  Panel")]
    public GameObject GameOptionsUIPanel;

    [Header("Create Room Panel")]
    public GameObject CreateRoomUIPanel;
    public GameObject CreateUIPanel;
    public TMP_InputField roomNameInputField;
    private int maxPlayers;

    [Header("Show Room List Panel")]
    public GameObject ShowRoomListUIPanel;
    public GameObject RoomListItemPrefab;
    public GameObject roomListParent;

    [Header("Inside Room Panel")]
    public GameObject InsideRoomUIPanel;
    public TextMeshProUGUI RoomNameText;
    public TextMeshProUGUI PlayerCountText;
    public GameObject PlayerListPrefab;
    public GameObject PlayerListParent;
    public GameObject StartGameButton;


    
    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListGameObjects;
    private Dictionary<int, GameObject> playerListGameObjects;

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        ActivatePanel(LoginUIPanel);
        PhotonNetwork.AutomaticallySyncScene = true;
        PlayerNameInput.characterLimit = 10;
        roomNameInputField.characterLimit = 20;
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListGameObjects = new Dictionary<string, GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region UI Callback Methods
    public void OnLoginButtonClicked()
    {
        string playerName = PlayerNameInput.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            ActivatePanel(ConnectingInfoUIPanel);

            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            Debug.Log("PlayerName is invalid!");
        }
    }

    public void OnReturnButtonClicked()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void OnCreateRoomButtonClicked()
    {
        ActivatePanel(CreateRoomUIPanel);
    }

    public void OnCancelButtonClicked()
    {
        ActivatePanel(GameOptionsUIPanel);
    }

    public void OnCreateButtonClicked()
    {
        if (maxPlayers != 0)
        {
            ActivatePanel(CreateUIPanel);

            string roomName = roomNameInputField.text;

            if (string.IsNullOrEmpty(roomName))
            {
                roomName = "Room " + Random.Range(1000, 10000);
            }

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = (byte)maxPlayers;

            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }
        else
        {
            Debug.Log("Invalid Inputs!");
            return;
        }
    }

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnShowRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        
        ActivatePanel(ShowRoomListUIPanel);
    }

    public void OnBackButtonClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        ActivatePanel(GameOptionsUIPanel);
    }

    public void OnLogoutButtonClicked()
    {
        PhotonNetwork.Disconnect();
        ActivatePanel(LoginUIPanel);
    }

    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }
    #endregion

    #region Photon Callbacks
    public override void OnConnected()
    {
        Debug.Log("Connected to Internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon");
        ActivatePanel(GameOptionsUIPanel);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " has disconnected to Photon");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " created!");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " has joined " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
        ActivatePanel(InsideRoomUIPanel);

        RoomNameText.text = PhotonNetwork.CurrentRoom.Name;
        PlayerCountText.text = "Players: " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;

        if (playerListGameObjects == null)
        {
            playerListGameObjects = new Dictionary<int, GameObject>();
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerListItem = Instantiate(PlayerListPrefab);
            playerListItem.transform.SetParent(PlayerListParent.transform);
            playerListItem.transform.localScale = Vector3.one;

            playerListItem.GetComponent<PlayerListItemInitializer>().Initialize(player.ActorNumber, player.NickName);

            object isPlayerReady;
            if (player.CustomProperties.TryGetValue(Constants.PLAYER_READY, out isPlayerReady))
            {
                playerListItem.GetComponent<PlayerListItemInitializer>().SetPlayerReady((bool)isPlayerReady);
            }
            playerListGameObjects.Add(player.ActorNumber, playerListItem);
        }

        StartGameButton.SetActive(false);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListGameObjects();

        Debug.Log("OnRoomListUpdate called");

        foreach (RoomInfo info in roomList)
        {
            Debug.Log(info.Name);
            
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }
            }
            else
            {
                // Update existing rooms info
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList[info.Name] = info;
                }
                else
                {
                    cachedRoomList.Add(info.Name, info);
                }      
            }
        }

        foreach(RoomInfo info in cachedRoomList.Values)
        {
            GameObject listItem = Instantiate(RoomListItemPrefab);
            listItem.transform.SetParent(roomListParent.transform);
            listItem.transform.localScale = Vector3.one;

            listItem.transform.Find("RoomNameText").GetComponent<TextMeshProUGUI>().text = info.Name;
            listItem.transform.Find("PlayerCountText").GetComponent<TextMeshProUGUI>().text = "Player Count: " + info.PlayerCount + " / " + info.MaxPlayers;
            listItem.transform.Find("JoinButton").GetComponent<Button>().onClick.AddListener(() => OnJoinRoomClicked(info.Name));

            roomListGameObjects.Add(info.Name, listItem);
        }
    }

    public override void OnLeftLobby()
    {
        ClearRoomListGameObjects();
        cachedRoomList.Clear();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject playerListItem = Instantiate(PlayerListPrefab);
        playerListItem.transform.SetParent(PlayerListParent.transform);
        playerListItem.transform.localScale = Vector3.one;

        playerListItem.GetComponent<PlayerListItemInitializer>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

        playerListGameObjects.Add(newPlayer.ActorNumber, playerListItem);

        RoomNameText.text = PhotonNetwork.CurrentRoom.Name;
        PlayerCountText.text = "Players: " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;

        StartGameButton.SetActive(CheckAllPlayerReady());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomNameText.text = PhotonNetwork.CurrentRoom.Name;
        PlayerCountText.text = "Players: " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;

        Destroy(playerListGameObjects[otherPlayer.ActorNumber].gameObject);
        playerListGameObjects.Remove(otherPlayer.ActorNumber);
    }

    public override void OnLeftRoom()
    {
        foreach (GameObject playerlistGameObject in playerListGameObjects.Values)
        {
            Destroy(playerlistGameObject);
        }

        playerListGameObjects.Clear();
        playerListGameObjects = null;

        ActivatePanel(GameOptionsUIPanel);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        GameObject playerListGameObject;

        if (playerListGameObjects.TryGetValue(targetPlayer.ActorNumber, out playerListGameObject))
        {
            object isPlayerReady;

            if (changedProps.TryGetValue(Constants.PLAYER_READY, out isPlayerReady))
            {
                playerListGameObject.GetComponent<PlayerListItemInitializer>().SetPlayerReady((bool)isPlayerReady);
            }
        }

        StartGameButton.SetActive(CheckAllPlayerReady());
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            StartGameButton.SetActive(CheckAllPlayerReady());
        }
    }
    #endregion

    #region Public Methods
    public void ActivatePanel(GameObject chosenPanel)
    {
        LoginUIPanel.SetActive(chosenPanel.Equals(LoginUIPanel));
        ConnectingInfoUIPanel.SetActive(chosenPanel.Equals(ConnectingInfoUIPanel));
        GameOptionsUIPanel.SetActive(chosenPanel.Equals(GameOptionsUIPanel));
        CreateRoomUIPanel.SetActive(chosenPanel.Equals(CreateRoomUIPanel));
        CreateUIPanel.SetActive(chosenPanel.Equals(CreateUIPanel));
        ShowRoomListUIPanel.SetActive(chosenPanel.Equals(ShowRoomListUIPanel));
        InsideRoomUIPanel.SetActive(chosenPanel.Equals(InsideRoomUIPanel));

    }

    public void SetMaxPlayers(int _maxPlayers)
    {
        if(this.maxPlayers == _maxPlayers)
        {
            this.maxPlayers = 0;
        }
        else
        {
            this.maxPlayers = _maxPlayers;
        }
    }
    #endregion

    #region Private Methods
    private void OnJoinRoomClicked(string roomName)
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        PhotonNetwork.JoinRoom(roomName);
    }

    private void ClearRoomListGameObjects()
    {
        foreach (var item in roomListGameObjects.Values)
        {
            Destroy(item);
        }

        roomListGameObjects.Clear();
    }

    private bool CheckAllPlayerReady()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;

            if (p.CustomProperties.TryGetValue(Constants.PLAYER_READY, out isPlayerReady))
            {
                if (!(bool)isPlayerReady)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return true;
    }
    #endregion
}
