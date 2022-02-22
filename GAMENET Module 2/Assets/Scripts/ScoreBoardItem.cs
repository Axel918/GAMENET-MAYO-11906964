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

    public void Initialize(Player player)
    {
        usernameText.text = player.NickName;
        killCountText.text = "Kills: " + killCount.ToString("0");
    }

    // Update is called once per frame
    void Update()
    {
        if (killCount >= 10)
        {
            GameManager.instance.GameOver();
        }
    }
}
