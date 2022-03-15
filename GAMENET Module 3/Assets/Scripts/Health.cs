using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Health : MonoBehaviourPunCallbacks
{
    [Header("HP Variables")]
    public float startHealth;
    private float health;
    public Image healthBar;
    private bool isAlive;

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
        if (photonView.IsMine)
        {
            this.GetComponent<VehicleMovement>().enabled = false;
        }
    }
}
