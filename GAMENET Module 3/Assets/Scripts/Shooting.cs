using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
/*
public enum ShootType
{
    MACHINE_GUN,
    PROJECTILE,
    LASER
}

public class Shooting : MonoBehaviourPunCallbacks
{
    public ShootType shootType;
    
    public GameObject car;

    [Header("HP Variables")]
    public float startHealth = 100;
    private float health;
    public Image healthBar;

    private bool isAlive;

    public GameObject machineGunEffectPrefab;
    public Transform[] firePoints;
    public GameObject[] bulletPrefab;

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
        if (photonView.IsMine)
        {
            if (Input.GetKey(KeyCode.J) && shootType == ShootType.MACHINE_GUN)
            {
               //Debug.Log("Shoot");
               MachineGunFire(); 
            }
            else if (Input.GetKeyDown(KeyCode.J) && shootType == ShootType.PROJECTILE)
            {
                ProjectileFire();
            }
        }
        
        
    }

    public void MachineGunFire()
    {
        RaycastHit hit;

        if (Physics.Raycast(car.transform.position, car.transform.forward, out hit, 200))
        {
            Debug.Log(hit.collider.gameObject.name);
            photonView.RPC("MachineGunEffects", RpcTarget.All, firePoints[0].position);
            photonView.RPC("MachineGunEffects", RpcTarget.All, firePoints[1].position);

            if (hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
            {
                hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 5);
            }
        }
    }

    public void ProjectileFire()
    {
        /*RaycastHit hit;
        Vector3 targetPoint = new Vector3();

        if (Physics.Raycast(car.transform.position, car.transform.forward, out hit, 200))
        {
            targetPoint = hit.point;
            Debug.Log(hit.collider.gameObject.name);
        }
        else
        {
            Debug.Log("Out of distance");
            return;
        }

        // Calculate direction
        //Vector3 direction = targetPoint - firePoints[0].position;

        photonView.RPC("ShootBullets", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void ShootBullets()
    {
        // Spawn prefab
        GameObject bullet = Instantiate(bulletPrefab[0], firePoints[0].position, Quaternion.identity);

        // Rotate Bullet forward
        bullet.transform.forward = firePoints[0].forward;

        // Add force
        bullet.GetComponent<Rigidbody>().AddForce(firePoints[0].forward * 50f, ForceMode.Impulse);

        // Destroy Bullet
        Destroy(bullet, 1.0f);
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
            Death();
            isAlive = false;
        }
    }

    [PunRPC]
    public void MachineGunEffects(Vector3 position)
    {
        GameObject hitEffectGameObject = Instantiate(machineGunEffectPrefab, position, Quaternion.identity);
        hitEffectGameObject.transform.parent = gameObject.transform;
        Destroy(hitEffectGameObject, 0.2f);
    }

    public void Death()
    {
        
    }
}
*/
