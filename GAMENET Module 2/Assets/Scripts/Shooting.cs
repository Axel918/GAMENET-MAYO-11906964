using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Shooting : MonoBehaviourPunCallbacks
{
    public Camera camera;
    public GameObject hitEffectPrefab;

    [Header("HP Related Stuff")]
    public float startHealth = 100;
    private float health;
    public Image healthBar;

    private Animator animator;

    private GameObject scoreBoard;

    private bool isAlive;

    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
        healthBar.fillAmount = health / startHealth;
        animator = this.GetComponent<Animator>();
        scoreBoard = GameObject.Find("ScoreBoard");
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire()
    {
        RaycastHit hit;
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (Physics.Raycast(ray, out hit, 200))
        {
            Debug.Log(hit.collider.gameObject.name);
            photonView.RPC("CreateHitEffects", RpcTarget.All, hit.point);

            if (hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
            {
                hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 25);
            }
        }
    }

    [PunRPC]
    public void TakeDamage(int damage, PhotonMessageInfo info)
    {
        this.health -= damage;
        this.healthBar.fillAmount = health / startHealth;

        if (health <= 0 && isAlive)
        {
            Die();                                                                                  // Player that was killed dies
            ScoreBoard.instance.UpdateKillCount(info.Sender);                                       // Killer gets a point
            KillFeed.instance.AddKillFeedItem(info.Sender, info.photonView.Owner);                  // Notifies everyone who was killed
            GameManager.instance.playerVictor = info.Sender;
            photonView.RPC("IsNotAlive", RpcTarget.AllBuffered, false);
            Debug.Log(info.Sender.NickName + " killed " + info.photonView.Owner.NickName);
        }
    }
    
    [PunRPC]
    public void CreateHitEffects(Vector3 position)
    {
        GameObject hitEffectGameObject = Instantiate(hitEffectPrefab, position, Quaternion.identity);
        Destroy(hitEffectGameObject, 0.2f);
    }

    public void Die()
    {
        if (photonView.IsMine)
        {
            animator.SetBool("isDead", true);
            StartCoroutine(RespawnCountdown());
        }
    }

    IEnumerator RespawnCountdown()
    {
        GameObject respawn = GameObject.Find("Respawn Canvas");
        GameObject respawnText = GameObject.Find("Respawn Text");
        float respawnTime = 5.0f;

        while (respawnTime > 0f)
        {
            yield return new WaitForSeconds(1.0f);
            respawnTime--;

            transform.GetComponent<PlayerMovementController>().enabled = false;
            respawn.GetComponent<Image>().color = new Color(0, 0, 0, 80);
            respawnText.GetComponent<Text>().text = "You are killed. Respawning in " + respawnTime.ToString(".00");
        }

        animator.SetBool("isDead", false);
        respawn.GetComponent<Image>().color = new Color(0,0,0,0);
        respawnText.GetComponent<Text>().text = "";

        // Position to a random spawn point
        this.transform.position = SpawnManager.instance.spawnPoints[SpawnManager.instance.GetRandomNumber()].position;
        transform.GetComponent<PlayerMovementController>().enabled = true;

        photonView.RPC("IsNotAlive", RpcTarget.AllBuffered, true);
        photonView.RPC("RegainHealth", RpcTarget.AllBuffered);
    }

    // Regains player health
    [PunRPC]
    public void RegainHealth()
    {
        health = 100;
        healthBar.fillAmount = health / startHealth;
    }

    [PunRPC]
    public void IsNotAlive(bool value)
    {
        isAlive = value;
    }
}
