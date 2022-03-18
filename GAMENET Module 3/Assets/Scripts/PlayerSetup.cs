using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public Camera camera;

    [SerializeField]
    private TextMeshProUGUI playerNameText;

    public GameObject playerUiPrefab;

    [SerializeField]
    private GameObject hpCanvas;
    public string playerName;

    // Start is called before the first frame update
    void Start()
    {
        this.camera = transform.Find("Camera").GetComponent<Camera>();

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("rc"))
        {
            GetComponent<VehicleMovement>().enabled = photonView.IsMine;
            GetComponent<LapController>().enabled = photonView.IsMine;
            camera.enabled = photonView.IsMine;
            hpCanvas.SetActive(false);
        }
        else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("dr"))
        {  
            GetComponent<VehicleMovement>().enabled = photonView.IsMine;
            camera.enabled = photonView.IsMine;
            hpCanvas.SetActive(true);

            // Add Player to the list
            DeathRaceGameManager.instance.playersLeft.Add(this.gameObject);
        }

        // Set player name
        playerNameText.text = photonView.Owner.NickName;
        playerName = photonView.Owner.NickName;
    }

    
}
