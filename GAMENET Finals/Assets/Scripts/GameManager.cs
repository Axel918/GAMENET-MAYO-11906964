using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject[] playerPrefabs;
    public GameObject[] playerScoreItems;

    public static GameManager instance;


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

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

            PhotonNetwork.Instantiate(playerPrefabs[actorNumber - 1].name, SpawnManager.instance.spawnPoints[actorNumber - 1].position, Quaternion.identity);
         
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void SetTime(float time)
    {
        /*if (time > 0)
        {
            timerText.text = time.ToString("F1");
        }
        else
        {
            timerText.text = "";
        }*/
    }

    public void StartSession()
    {
        
        
    }
}
