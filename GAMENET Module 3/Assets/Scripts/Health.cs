using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Health : MonoBehaviour
{
    [Header("HP Variables")]
    public float startHealth = 100;
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
    public void TakeDamage(int damage, PhotonMessageInfo info)
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

    }
}
