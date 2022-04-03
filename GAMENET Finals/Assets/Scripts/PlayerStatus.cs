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
    private int playerActorNumber;
    public GameObject explosion;

    // Power-up booleans
    private bool canKill;
   
    public float r;
    public float g;
    public float b;

    // Start is called before the first frame update
    void Start()
    {
        playerActorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        playerName = photonView.Owner.NickName;
        playerScore = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // If player collides with a tile
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

        // If player collides with another player
        if (collider.tag == "Player" && canKill == true && !collider.gameObject.GetComponent<PhotonView>().IsMine)
        {
            Debug.Log("You hit an enemy");
            photonView.RPC("KillOpposingPlayer", RpcTarget.AllBuffered);
            collider.gameObject.GetComponent<PhotonView>().RPC("PlayerKilled", RpcTarget.AllBuffered);
        }

        // If player picks up an power-up
        if (collider.tag == "Power-Up")
        {
            PowerUps power = collider.GetComponent<PowerUps>();
            
            if (power.type == PowerUps.PowerUpType.SPEED_UP)
            {
                photonView.RPC("SpeedUp", RpcTarget.AllBuffered);
            }
            else if (power.type == PowerUps.PowerUpType.SLOW_DOWN)
            {
                photonView.RPC("SlowDown", RpcTarget.AllBuffered);
            }
            else if (power.type == PowerUps.PowerUpType.KNOCK_OUT)
            {
                photonView.RPC("KnockOut", RpcTarget.AllBuffered);
            }

            Destroy(collider.gameObject);
        }
    }

    [PunRPC]
    public void SpeedUp()
    {
        StartCoroutine(SpeedUpTimer());
    }

    [PunRPC]
    public void SlowDown()
    {
        foreach (GameObject go in GameManager.instance.playerGO)
        {
            if (this.gameObject != go)
            {
                go.GetComponent<PlayerMovement>().speed = 2.5f;
                go.GetComponent<PlayerStatus>().statusEffects[1].SetActive(true);
                go.GetComponent<PlayerStatus>().StartCoroutine(SlowDownTimer());
            }
        }
    }

    [PunRPC]
    public void KnockOut()
    {
        canKill = true;
        statusEffects[2].SetActive(true);
  
        StartCoroutine(KnockOutTimer());
    }

    [PunRPC]
    public void KillOpposingPlayer()
    {
        canKill = false;
        statusEffects[2].SetActive(false);
        StopCoroutine(KnockOutTimer());

        // Spawn explosion
        GameObject explosionPrefab = Instantiate(explosion, this.transform.position, Quaternion.identity);

        // Destroy explosion
        Destroy(explosionPrefab, 1.0f);
    }

    // When player is killed
    [PunRPC]
    public void PlayerKilled()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(Respawn());
        }

        StartCoroutine(ChangeColor());
    }

    // Timer for the Speed Up Power-up
    IEnumerator SpeedUpTimer()
    {
        gameObject.GetComponent<PlayerMovement>().speed = 10;
        statusEffects[0].SetActive(true);

        yield return new WaitForSeconds(5f);
        
        gameObject.GetComponent<PlayerMovement>().speed = 5;
        statusEffects[0].SetActive(false);
    }

    // Timer for the Slow Down Power-up
    IEnumerator SlowDownTimer()
    {
        yield return new WaitForSeconds(5f);

        foreach (GameObject go in GameManager.instance.playerGO)
        {
            if (this.gameObject != go)
            {
                go.GetComponent<PlayerMovement>().speed = 5;
                go.GetComponent<PlayerStatus>().statusEffects[1].SetActive(false);
            }
        }
    }

    IEnumerator KnockOutTimer()
    {
        yield return new WaitForSeconds(5f);

        canKill = false;
        statusEffects[2].SetActive(false);
    }

    IEnumerator Respawn()
    {
        GetComponent<PlayerMovement>().enabled = false;

        yield return new WaitForSeconds(2f);

        transform.position = SpawnManager.instance.spawnPoints[playerActorNumber - 1].position;
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<SpriteRenderer>().color = new Color(r, g, b);
    }

    // Change opposing player's color when hit.
    IEnumerator ChangeColor()
    {
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        yield return new WaitForSeconds(2f);
        GetComponent<SpriteRenderer>().color = new Color(r, g, b);
    }

    // Evaluates the Player Score
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
    }
}
