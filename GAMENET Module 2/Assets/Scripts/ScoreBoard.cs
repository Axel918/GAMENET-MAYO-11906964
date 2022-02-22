using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ScoreBoard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container;
    [SerializeField] GameObject scoreBoardItemPrefab;

    Dictionary<Player, ScoreBoardItem> scoreBoardItems = new Dictionary<Player, ScoreBoardItem>();

    public static ScoreBoard instance;

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
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            AddScoreBoardItem(player);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Adds the player to the scoreboard
    void AddScoreBoardItem(Player player)
    {
        ScoreBoardItem item = Instantiate(scoreBoardItemPrefab, container).GetComponent<ScoreBoardItem>();
        item.Initialize(player);
        scoreBoardItems[player] = item;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddScoreBoardItem(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemoveScoreBoardItem(otherPlayer);
    }

    void RemoveScoreBoardItem(Player player)
    {
        Destroy(scoreBoardItems[player].gameObject);
        scoreBoardItems.Remove(player);
    }

    public void UpdateKillCount(Player player)
    {
        scoreBoardItems[player].killCount += 1;
        scoreBoardItems[player].SetKillCount(player);
    }
}
