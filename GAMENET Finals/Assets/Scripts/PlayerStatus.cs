using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerStatus : MonoBehaviourPunCallbacks
{
    public int playerScore;
    public string playerName;
    private bool hasSubmittedData;
    public int playerActorNumber;

    // Start is called before the first frame update
    void Start()
    {
        playerScore = 0;
        playerName = photonView.Owner.NickName;
        hasSubmittedData = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (TimerManager.instance.GetCurrentTime() <= 0 && hasSubmittedData == false)
        {
            EvaluateScore();
            hasSubmittedData = true;
        }*/

        playerScore = GameManager.instance.playerScoreItems[playerActorNumber].GetComponent<PlayerScoreItem>().currentScore;
    }

    public void EvaluateScore()
    {
        ScoreManager.instance.AddData(playerName, playerScore);
        //ScoreManager.instance.
    }
}
