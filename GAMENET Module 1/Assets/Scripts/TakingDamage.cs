using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TakingDamage : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Image healthBar;
    
    private float startHealth;

    public float health;
    
    // Start is called before the first frame update
    void Start()
    {
        startHealth = 100;
        health = startHealth;
        healthBar.fillAmount = health / startHealth;
    }

    [PunRPC]
    public void takeDamage(int damage)
    {
        health -= damage;
        Debug.Log(health);

        healthBar.fillAmount = health / startHealth;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (photonView.IsMine)
        {
            GameManager.instance.LeaveRoom();
        }
    }
}
