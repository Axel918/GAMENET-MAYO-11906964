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

    private Health lifeDetector;
    private CountdownManager countdownMng;
    private VehicleMovement movement;

    // Start is called before the first frame update
    void Start()
    {
        lifeDetector = this.GetComponent<Health>();
        countdownMng = this.GetComponent<CountdownManager>();
        movement = this.GetComponent<VehicleMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine && lifeDetector.isAlive && movement.isControlEnabled == true)
        {
            if (Input.GetKey(KeyCode.J))
            {
                photonView.RPC("LaserEnabler", RpcTarget.All, true);
                LaserFire();
            }
            else if (Input.GetKeyUp(KeyCode.J))
            {
                photonView.RPC("LaserEnabler", RpcTarget.All, false);
            }
        }
        
    }

    public void LaserFire()
    {
        RaycastHit hit;

        if (Physics.Raycast(car.transform.position, car.transform.forward, out hit, 200))
        {
            Debug.Log(hit.collider.gameObject.name);
            photonView.RPC("LaserRenderer", RpcTarget.All, 0, firePoint.position);
            photonView.RPC("LaserRenderer", RpcTarget.All, 1, hit.point);

            if (hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
            {
                hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 0.1f);
            }
        }
        else
        {
            photonView.RPC("LaserRenderer", RpcTarget.All, 0, firePoint.position);
            photonView.RPC("LaserRenderer", RpcTarget.All, 1, firePoint.position);
        }
    }

    [PunRPC]
    public void LaserRenderer(int index, Vector3 position)
    {
        laser.SetPosition(index, position);
    }

    [PunRPC]
    public void LaserEnabler(bool enable)
    {
        laser.enabled = enable;
    }
}
