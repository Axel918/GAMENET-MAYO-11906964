using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public enum ShootType
{
    MACHINE_GUN,
    PROJECTILE,
    LASER
}

public class Shooting : MonoBehaviour
{
    public ShootType shootType;
    
    public Camera camera;

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
        if (Input.GetKey(KeyCode.J))
        {
            if (shootType == ShootType.MACHINE_GUN)
            {
                Debug.Log("Shoot");
                //MachineGunFire();
            }
        }
        
    }

    public void MachineGunFire()
    {
        RaycastHit hit;
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (Physics.Raycast(ray, out hit, 200))
        {
            Debug.Log(hit.collider.gameObject.name);
            //photonView.RPC("CreateHitEffects", RpcTarget.All, hit.point);

            if (hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
            {
                hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 5);
            }
        }
    }

    public void ProjectileFire()
    {

    }

    public void LaserFire()
    {

    }

    [PunRPC]
    public void TakeDamage(int damage, PhotonMessageInfo info)
    {
        this.health -= damage;
        this.healthBar.fillAmount = health / startHealth;

        if (health <= 0 && isAlive)
        {
            
        }
    }
}
