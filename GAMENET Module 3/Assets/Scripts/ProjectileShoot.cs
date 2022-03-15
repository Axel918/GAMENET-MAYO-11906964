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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
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
        //Vector3 direction = targetPoint - firePoint.position;*/

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