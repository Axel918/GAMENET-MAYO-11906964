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
    public GameObject[] playerGO;
    public TextMeshProUGUI playerResult;
    public TextMeshProUGUI playerStanding;

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

            PhotonNetwork.Instantiate(playerPrefabs[actorNumber - 1].name, SpawnManager.instance.spawnPoints[actorNumber - 1].position, Quaternion.identity);
        }

        inGamePanels[0].SetActive(true);
        inGamePanels[1].SetActive(false);

        StartCoroutine(CountdownToStart());
    }

    // Update is called once per frame
    void Update()
    {
        
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

        bgm.GetComponent<AudioSource>().Play();

        playerGO = GameObject.FindGameObjectsWithTag("Player");

        yield return new WaitForSeconds(1f);

        foreach (GameObject go in playerGO)
        {
            go.GetComponent<PlayerSetup>().SetPlayerViews();
        }

        TimerManager.instance.timerActive = true;
        countdownTimeText.text = "";
    }

    public IEnumerator TimeOver()
    {
        countdownTimeText.text = "Time's Up!";
        
        foreach (GameObject go in playerGO)
        {
            go.GetComponent<PlayerStatus>().EvaluateScore();
            ScoreManager.instance.players.Add(go);
        }

        ScoreManager.instance.SortScore();

        yield return new WaitForSeconds(1.0f);

        countdownTimeText.text = "";
        inGamePanels[0].SetActive(false);
        inGamePanels[1].SetActive(true);
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
}
