using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LaserShoot : MonoBehaviourPunCallbacks
{
    public GameObject car;
    public Transform firePoint;
    public LineRenderer laser;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKey(KeyCode.J))
            {
                laser.enabled = true;
                LaserFire();
            }
            else if (Input.GetKeyUp(KeyCode.J))
            {
                laser.enabled = false;
            }
        }
    }

    public void LaserFire()
    {
        RaycastHit hit;

        if (Physics.Raycast(car.transform.position, car.transform.forward, out hit, 200))
        {
            Debug.Log(hit.collider.gameObject.name);
            laser.SetPosition(0, firePoint.position);
            laser.SetPosition(1, hit.point);

            if (hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
            {
                hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 0.1f);
            }
        }
        else
        {
            laser.SetPosition(0, firePoint.position);
            laser.SetPosition(1, firePoint.position);
        }
    }
}
