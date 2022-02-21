using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class ScoreBoardItem : MonoBehaviour
{
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI killCountText;

    public void Initialize(Player player)
    {
        usernameText.text = player.NickName;
    }
}
