using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RaycastShoot : MonoBehaviourPunCallbacks
{
    public GameObject car;

    public GameObject machineGunEffectPrefab;
    public Transform[] firePoints;

    public float reloadTime;
    private float currentReloadTime;

    // Start is called before the first frame update
    void Start()
    {
        currentReloadTime = reloadTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKey(KeyCode.J) && currentReloadTime <= 0)
            {
                Debug.Log("Shoot");
                MachineGunFire();
                currentReloadTime = reloadTime;
            }
        }

        if (currentReloadTime > 0)
        {
            currentReloadTime -= Time.deltaTime;
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

    [PunRPC]
    public void MachineGunEffects(Vector3 position)
    {
        GameObject hitEffectGameObject = Instantiate(machineGunEffectPrefab, position, Quaternion.identity);
        hitEffectGameObject.transform.parent = gameObject.transform;
        Destroy(hitEffectGameObject, 0.2f);
    }
}
