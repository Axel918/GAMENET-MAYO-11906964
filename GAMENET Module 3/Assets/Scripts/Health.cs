using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Health : MonoBehaviourPunCallbacks
{
    [Header("HP Variables")]
    public float startHealth;
    private float health;
    public Image healthBar;
    public bool isAlive;

    public enum RaiseEventsCode
    {
        WhoDiedEventCode = 0
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)RaiseEventsCode.WhoDiedEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;

            string nickNameOfKilledPlayer = (string)data[0];
            int viewId = (int)data[1];

            Debug.Log(nickNameOfKilledPlayer + " has been eliminated!");

            GameObject killEvent = DeathRaceGameManager.instance.killEventText;
            killEvent.SetActive(true);

            if (viewId == photonView.ViewID)    // This is you!
            {
                killEvent.GetComponent<Text>().text = nickNameOfKilledPlayer + " has been killed!";
            }
            else
            {
                killEvent.GetComponent<Text>().text = "You have been killed!";
            }

            StartCoroutine(DeativateObject(killEvent));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
        healthBar.fillAmount = health / startHealth;
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void TakeDamage(float damage, PhotonMessageInfo info)
    {
        this.health -= damage;
        this.healthBar.fillAmount = health / startHealth;

        if (health <= 0 && isAlive)
        {
            Death();
            isAlive = false;
        }
    }

    public void Death()
    {
        GetComponent<PlayerSetup>().camera.transform.parent = null;
        GetComponent<VehicleMovement>().enabled = false;
        DeathRaceGameManager.instance.playersLeft.Remove(this.gameObject);
        DeathRaceGameManager.instance.PlayersRemaining();

        string nickName = photonView.Owner.NickName;
        int viewId = photonView.ViewID;

        // event data
        object[] data = new object[] { nickName, viewId };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOption = new SendOptions
        {
            Reliability = false
        };

        PhotonNetwork.RaiseEvent((byte)RaiseEventsCode.WhoDiedEventCode, data, raiseEventOptions, sendOption);
    }

    [PunRPC]
    IEnumerator DeativateObject(GameObject gmob)
    {
        yield return new WaitForSeconds(1.5f);
        gmob.SetActive(false);
    }
}
