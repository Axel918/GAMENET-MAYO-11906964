using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerStatus : MonoBehaviourPunCallbacks
{
    public GameObject[] tiles;
    public int playerScore;
    public string playerName;
    private bool hasSubmittedData;
    public int playerActorNumber;
   
    public float r;
    public float g;
    public float b;

    // Start is called before the first frame update
    void Start()
    {
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Tile")
        {
            if (collider.GetComponent<SpriteRenderer>().color == new Color(r, g, b))
            {
                Debug.Log("Color is the same");
                return;
            }
            else
            {
                collider.GetComponent<SpriteRenderer>().color = new Color(r, g, b);
                Debug.Log("New Color");
            }
        }

        if (collider.tag == "Player")
        {
            Debug.Log("You hit an enemy");
        }
    }

    public void EvaluateScore()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].GetComponent<SpriteRenderer>().color == new Color(r, g, b))
            {
                playerScore++;
            }
        }

        ScoreManager.instance.AddData(playerName, playerScore);
    }
}
