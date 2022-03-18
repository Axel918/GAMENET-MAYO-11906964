using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ProjectileShoot : MonoBehaviourPunCallbacks
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float reloadTime;
    private float currentReloadTime;

    private Health lifeDetector;
    private CountdownManager countdownMng;
    private VehicleMovement movement;

    // Start is called before the first frame update
    void Start()
    {
        currentReloadTime = reloadTime;
        lifeDetector = this.GetComponent<Health>();
        countdownMng = this.GetComponent<CountdownManager>();
        movement = this.GetComponent<VehicleMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine && lifeDetector.isAlive && movement.isControlEnabled == true)
        {
            if (Input.GetKeyDown(KeyCode.J) && currentReloadTime <= 0)
            {
                ProjectileFire();
                currentReloadTime = reloadTime;
            }
        }

        if (currentReloadTime > 0)
        {
            currentReloadTime -= Time.deltaTime;
        }
    }

    public void ProjectileFire()
    {
        photonView.RPC("ShootBullets", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void ShootBullets()
    {
        // Spawn prefab
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Rotate Bullet forward
        bullet.transform.forward = firePoint.forward;

        // Add force
        bullet.GetComponent<Rigidbody>().AddForce(firePoint.forward * 50f, ForceMode.Impulse);

        // Destroy Bullet
        Destroy(bullet, 1.0f);
    }
}
