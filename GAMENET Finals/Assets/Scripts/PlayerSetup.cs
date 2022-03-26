using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject camera;
    
    public TextMeshProUGUI playerNameText;
    private PlayerMovement playerMovement;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();

        animator.SetBool("isLocalPlayer", photonView.IsMine);

        if (photonView.IsMine)
        {
            playerMovement.enabled = true;
            camera.GetComponent<Camera>().enabled = true;
        }
        else
        {
            playerMovement.enabled = false;
            camera.GetComponent<Camera>().enabled = false;
        }

        // Set player name
        playerNameText.text = photonView.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
