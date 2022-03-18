using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class DeathRaceGameManager : MonoBehaviourPunCallbacks
{
    public static DeathRaceGameManager instance = null;

    public GameObject[] vehiclePrefabs;
    public Transform[] startingPositions;
    public Text timeText;
    public GameObject killEventText;
    public List<GameObject> playersLeft = new List<GameObject>();
    
    public GameObject winnerPanel;
    public GameObject eventsPanel;
    public Text winnerText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            object playerSelectionNumber;

            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(Constants.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
            {
                Debug.Log((int)playerSelectionNumber);

                int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
                Vector3 instantiatePosition = startingPositions[actorNumber - 1].position;
                PhotonNetwork.Instantiate(vehiclePrefabs[(int)playerSelectionNumber].name, instantiatePosition, Quaternion.identity);
            }

            winnerPanel.SetActive(false);
            eventsPanel.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayersRemaining()
    {
        if (playersLeft.Count <= 1)
        {
            StartCoroutine(DisplayWinner());
        }
        else
        {
            return;
        }
    }

    IEnumerator DisplayWinner()
    {
        yield return new WaitForSeconds(2.5f);
        winnerText.text = playersLeft[0].GetComponent<PlayerSetup>().playerName + " wins!";
        playersLeft[0].GetComponent<VehicleMovement>().enabled = false;
        winnerPanel.SetActive(true);
        eventsPanel.SetActive(false);
    }
}
