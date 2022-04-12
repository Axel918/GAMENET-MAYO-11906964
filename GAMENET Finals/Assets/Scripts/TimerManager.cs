using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;

    public GameObject[] whiteWalls;

    // Timer
    private float currentTime;
    public int startMinutes;
    public TextMeshProUGUI currentTimeText;
    public bool timerActive;
    private bool lastMinute;

    public GameObject background;
    public TextMeshProUGUI countdownTimeText;

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
        currentTime = startMinutes * 60;
        lastMinute = false;

        foreach (GameObject go in whiteWalls)
        {
            go.GetComponent<LerpColor>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive == true)
        {
            currentTime -= Time.deltaTime;

            if (Mathf.Floor(currentTime) <= 120 && lastMinute == false)
            {
                StartCoroutine(LastMinute());
            }

            if (currentTime <= 0.9f)
            {
                timerActive = false;
                StartCoroutine(GameManager.instance.TimeOver());
                Debug.Log("Time Over");

                foreach (GameObject go in GameManager.instance.playerGO)
                {
                    go.GetComponent<PlayerSetup>().GetAnimator().enabled = false;
                    go.GetComponent<PlayerSetup>().GetPlayerMovement().enabled = false;
                }
            }

            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            currentTimeText.text = time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
        }
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    IEnumerator LastMinute()
    {
        countdownTimeText.text = "LAST 2 MINUTES!";

        GameManager.instance.bgm.GetComponent<AudioSource>().Stop();
        GameManager.instance.bgm.GetComponent<AudioSource>().pitch = 1.25f;
        GameManager.instance.bgm.GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(1.5f);

        countdownTimeText.text = "";
        background.GetComponent<VideoPlayer>().playbackSpeed = 8;

        foreach (GameObject go in whiteWalls)
        {
            go.GetComponent<LerpColor>().enabled = true;
        }

        lastMinute = true;
    }
}
