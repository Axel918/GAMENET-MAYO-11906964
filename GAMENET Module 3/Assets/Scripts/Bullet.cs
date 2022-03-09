using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && !other.gameObject.GetComponent<PhotonView>().IsMine)
        {
            other.collider.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 5);

            Destroy(gameObject);
        }
        
       
    }


}
