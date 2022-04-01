using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerStatus : MonoBehaviourPunCallbacks
{
    public GameObject[] tiles;
    public GameObject[] statusEffects;
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

        if (collider.tag == "Power-Up")
        {
            PowerUps power = collider.GetComponent<PowerUps>();
            
            if (power.type == PowerUps.PowerUpType.SPEED_UP)
            {
                photonView.RPC("SpeedUp", RpcTarget.AllBuffered);
            }
            else if (power.type == PowerUps.PowerUpType.SLOW_DOWN)
            {
                foreach (GameObject go in GameManager.instance.playerGO)
                {
                    if (this.gameObject != go)
                    {
                        go.GetComponent<PlayerMovement>().speed /= 2;
                    }
                }
            }
            else if (power.type == PowerUps.PowerUpType.KNOCK_OUT)
            {

            }

            Destroy(collider.gameObject);
        }
    }

    [PunRPC]
    public void SpeedUp()
    {
        gameObject.GetComponent<PlayerMovement>().speed = 10;
        statusEffects[0].SetActive(true);
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
