using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject[] playerPrefabs;
    public GameObject[] inGamePanels;
    public GameObject[] playerScoreItems;
    public GameObject[] playerGO;
    public TextMeshProUGUI playerResult;
    public TextMeshProUGUI playerStanding;
    public Transform[] spawnPoints;

    public static GameManager instance;

    // Introductory Countdown
    public int countdownTime;
    public TextMeshProUGUI countdownTimeText;

    public GameObject bgm;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

            PhotonNetwork.Instantiate(playerPrefabs[actorNumber - 1].name, spawnPoints[actorNumber - 1].position, Quaternion.identity);
        }

        ActivatePanel(inGamePanels[0]);

        StartCoroutine(CountdownToStart());
    }

    // Countdown before game begins
    IEnumerator CountdownToStart()
    {
        yield return new WaitForSeconds(1f);

        while (countdownTime > 0)
        {
            countdownTimeText.text = countdownTime.ToString("0");

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }

        countdownTimeText.text = "GO!";

        playerGO = GameObject.FindGameObjectsWithTag("Player");

        yield return new WaitForSeconds(1f);

        bgm.GetComponent<AudioSource>().Play();

        int i = 0;

        foreach (GameObject go in playerGO)
        {
            go.GetComponent<PlayerSetup>().SetPlayerViews();
            playerScoreItems[i].SetActive(true);
            i++;
        }

        TimerManager.instance.timerActive = true;
        countdownTimeText.text = "";
    }

    public IEnumerator TimeOver()
    {
        countdownTimeText.text = "Time's Up!";
        
        yield return new WaitForSeconds(1.0f);

        foreach (GameObject go in playerGO)
        {
            ScoreManager.instance.players.Add(go);
        }

        ScoreManager.instance.SortScore();

        countdownTimeText.text = "";
        ActivatePanel(inGamePanels[1]);
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    public void OnReturnToLobbyButtonClikced()
    {
        StartCoroutine(LeaveTheRoom());
    }

    IEnumerator LeaveTheRoom()
    {
        yield return new WaitForSeconds(1.0f);
        
        PhotonNetwork.LeaveRoom();
    }

    public void ActivatePanel(GameObject chosenPanel)
    {
        inGamePanels[0].SetActive(chosenPanel.Equals(inGamePanels[0]));
        inGamePanels[1].SetActive(chosenPanel.Equals(inGamePanels[1]));
    }
}
