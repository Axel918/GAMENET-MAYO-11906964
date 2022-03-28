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
    public Dictionary<string, int> playerData;

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
        playerData = new Dictionary<string, int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SortScore()
    {
        var topPlayers = playerData.OrderByDescending(pair => pair.Value).Take(PhotonNetwork.PlayerList.Length);

        int order = 0;
        int place = 1;

        // Present the Results
        foreach (var item in playerData.OrderByDescending(r => r.Value).Take(PhotonNetwork.PlayerList.Length))
        {
            Debug.Log(item.Key + " | " + item.Value);
            playerRankText[order].text = "#" + place + " | " + item.Value.ToString() +  " | " + item.Key.ToString();
            place++;
            order++;
        }
    }

    public void AddData(string name, int score)
    {
        playerData.Add(name, score);
    }
}
