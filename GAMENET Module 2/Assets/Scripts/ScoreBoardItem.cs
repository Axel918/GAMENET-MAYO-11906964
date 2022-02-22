using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class ScoreBoardItem : MonoBehaviour
{
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI killCountText;
    public int killCount;

    // Initialize player name and kill count
    public void Initialize(Player player)
    {
        usernameText.text = player.NickName;
        killCountText.text = "Kills: " + killCount.ToString("0");
    }

    // Updated the player kill count
    public void SetKillCount(Player player)
    {
        killCountText.text = "Kills: " + killCount.ToString("0");
    }

    // Update is called once per frame
    void Update()
    {
        // Game ends if one player reaches 10 kills
        if (killCount >= 10)
        {
            GameManager.instance.GameOver();
        }
    }
}
