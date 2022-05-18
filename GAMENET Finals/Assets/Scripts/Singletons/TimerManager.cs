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

    private TimeSpan time;

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

        time = TimeSpan.FromSeconds(currentTime);

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
            time = TimeSpan.FromSeconds(currentTime);

            if (Mathf.Floor(currentTime) <= 120 && lastMinute == false)
            {
                StartCoroutine(LastMinute());
            }

            if (currentTime > 1f && Mathf.Floor(currentTime) <= 10f)
            {
                //TimeSpan time1 = TimeSpan.FromSeconds(currentTime);
                countdownTimeText.text = time.Seconds.ToString("0");
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

            //TimeSpan time = TimeSpan.FromSeconds(currentTime);
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

        AudioManager.instance.Stop("bgm");
        AudioManager.instance.ModifyPitch("bgm", 1.25f);
        AudioManager.instance.Play("bgm");

        yield return new WaitForSeconds(1.5f);

        countdownTimeText.text = "";
        //background.GetComponent<VideoPlayer>().playbackSpeed = 8;

        foreach (GameObject go in whiteWalls)
        {
            go.GetComponent<LerpColor>().enabled = true;
        }

        lastMinute = true;
    }
}
