using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject[] playerPrefabs;
    public GameObject[] playerScoreItems;
    public GameObject[] inGamePanels;
    public GameObject background;

    public static GameManager instance;

    // Introductory Countdown
    public int countdownTime;
    public TextMeshProUGUI countdownTimeText;

    // Timer
    private float currentTime;
    public int startMinutes;
    public TextMeshProUGUI currentTimeText;
    private bool timerActive;
    private bool lastMinute;

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
        currentTime = startMinutes * 60;
        lastMinute = false;
}

    // Update is called once per frame
    void Update()
    {
        if (timerActive == true)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 120 && lastMinute == false)
            {
                StartCoroutine(LastMinute());
            }

            if (currentTime <= 0)
            {
                timerActive = false;
                StartCoroutine(TimeOver());
                Debug.Log("Time Over");
            }

        }

        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

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

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerScoreItems[i].SetActive(true);
        }

        timerActive = true;
        countdownTimeText.text = "";
    }

    IEnumerator LastMinute()
    {
        countdownTimeText.text = "LAST 2 MINUTES!";

        yield return new WaitForSeconds(1.5f);

        countdownTimeText.text = "";
        background.GetComponent<VideoPlayer>().playbackSpeed = 8;
        lastMinute = true;
    }

    IEnumerator TimeOver()
    {
        countdownTimeText.text = "Time's Up!";

        yield return new WaitForSeconds(1f);

        countdownTimeText.text = "";
        inGamePanels[0].SetActive(false);
        inGamePanels[1].SetActive(true);
    }
}
