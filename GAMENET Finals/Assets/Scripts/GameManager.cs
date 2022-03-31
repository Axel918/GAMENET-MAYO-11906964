using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject[] playerPrefabs;
    public GameObject[] playerScoreItems;
    public GameObject[] inGamePanels;
    public GameObject[] playerGO;
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

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

            PhotonNetwork.Instantiate(playerPrefabs[actorNumber - 1].name, SpawnManager.instance.spawnPoints[actorNumber - 1].position, Quaternion.identity);
        }

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

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerScoreItems[i].SetActive(true);

            playerGO[i].GetComponent<PlayerStatus>().playerActorNumber = i;
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
        }

        ScoreManager.instance.SortScore();
        
        yield return new WaitForSeconds(1.0f);

        countdownTimeText.text = "";
        inGamePanels[0].SetActive(false);
        inGamePanels[1].SetActive(true);
        
    }

    public void CheckWinner(string name, int score)
    {
        foreach (GameObject go in playerGO)
        {
            if (go.GetComponent<PlayerStatus>().playerName == name && go.GetComponent<PlayerStatus>().playerScore == score)
            {
                go.GetComponent<PlayerEvents>().PlayerStanding();
            }
        }
    }
}
