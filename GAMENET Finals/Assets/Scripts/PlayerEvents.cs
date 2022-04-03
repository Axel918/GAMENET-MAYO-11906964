using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerEvents : MonoBehaviourPunCallbacks
{
    public enum RaiseEventsCode
    {
        WhoWonEventCode = 0
    }

    void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        Debug.Log("Enabled");
    }

    void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
        Debug.Log("Disabled");
    }

    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)RaiseEventsCode.WhoWonEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;

            int viewId = (int)data[0];

            TextMeshProUGUI playerStandingText = GameManager.instance.playerStanding;

            if (viewId == photonView.ViewID)    // This is you!
            {
                playerStandingText.text = "YOU WIN! YOU REACHED 1ST PLACE.";
            }
            else
            {
                playerStandingText.text = "YOU LOSE!";
            }
        }
    }

    public void PlayerStanding()
    {
        string nickName = photonView.Owner.NickName;
        int viewId = photonView.ViewID;

        // event data
        object[] data = new object[] { viewId };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOption = new SendOptions
        {
            Reliability = false
        };

        PhotonNetwork.RaiseEvent((byte)RaiseEventsCode.WhoWonEventCode, data, raiseEventOptions, sendOption);
    }
}
