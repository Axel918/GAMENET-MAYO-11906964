using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Linq;


public class ScoreManager : MonoBehaviourPunCallbacks
{
    public static ScoreManager instance;

    public TextMeshProUGUI[] playerRankText;
    public List<GameObject> players;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SortScore()
    {
        for (int lastSortedIndex = players.Count - 1; lastSortedIndex > 0; lastSortedIndex--)
        {
            for (int i = 0; i < lastSortedIndex; i++)
            {
                if (players[i].GetComponent<PlayerStatus>().playerScore < players[i + 1].GetComponent<PlayerStatus>().playerScore)
                {
                    GameObject temp = players[i];
                    players[i] = players[i + 1];
                    players[i + 1] = temp;
                }
            }
        }

        PresentResults();
    }

    private void PresentResults()
    {
        int order = 0;
        int place = 1;

        foreach (GameObject go in players)
        {
            Debug.Log(go.GetComponent<PlayerStatus>().playerName + " | " + go.GetComponent<PlayerStatus>().playerScore);

            playerRankText[order].text = "#" + place + " | " + go.GetComponent<PlayerStatus>().playerName + " | " + go.GetComponent<PlayerStatus>().playerScore;

            place++;
            order++;
        }

        Debug.Log("Winner: " + players[0].GetComponent<PlayerStatus>().playerName);

        players[0].GetComponent<PlayerEvents>().PlayerStanding();
    }
}
