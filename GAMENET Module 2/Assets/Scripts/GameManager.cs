using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    public static GameManager instance;

    public GameObject[] panels;
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI returnText;
    public Player playerVictor;

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
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, SpawnManager.instance.spawnPoints[SpawnManager.instance.GetRandomNumber()].position, Quaternion.identity);
        }

        panels[panels.Length - 1].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        StartCoroutine(ShowGameOverPanel());
        StartCoroutine(LeaveRoom());
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(0.75f);
        panels[panels.Length - 1].SetActive(true);
        winnerText.text = playerVictor.NickName + " wins!";
    }

    IEnumerator LeaveRoom()
    {
        float returnTime = 5.0f;

        while (returnTime > 0f)
        {
            yield return new WaitForSeconds(1.0f);
            returnTime--;

            returnText.text = "Returning to Lobby in " + returnTime.ToString(".00");
        }

        PhotonNetwork.LeaveRoom();
    }
}
