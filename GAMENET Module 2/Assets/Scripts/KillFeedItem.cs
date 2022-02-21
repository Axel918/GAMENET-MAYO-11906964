using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class KillFeedItem : MonoBehaviour
{
    public TextMeshProUGUI killFeedText;

    public string GetKillFeed(Player player, Player otherPlayer)
    {
        killFeedText.text = player.NickName + " has killed " + otherPlayer.NickName;

        return killFeedText.text;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
