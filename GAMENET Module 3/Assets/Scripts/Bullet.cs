using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPunCallbacks
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !other.gameObject.GetComponent<PhotonView>().IsMine)
        {
            Debug.Log("SHOOT TRIGGER");
            other.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 5f);

            Destroy(gameObject);
        }
    }
}
